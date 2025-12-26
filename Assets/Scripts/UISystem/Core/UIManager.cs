using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UISystem.Core
{
    public class UIManager
    {
        private LRUCache<UIEnum, IUIBase> _uiCachedDic;

        private Transform _bottom;

        private Transform _center;

        private Transform _top;
        
        private Transform _banner;
        
        private Transform _popup;

        private Transform _guide;

        private readonly IResourceManager _resManager;
        
        public CanvasScaler RootCanvasScaler => _rootCanvasScaler;
        private CanvasScaler _rootCanvasScaler;

        public Canvas RootCanvas => _rootCanvas;
        private Canvas _rootCanvas;
        
        public Vector2 ScreenSize => _rootCanvas.GetComponent<RectTransform>().sizeDelta;
       
        public int Capacity { get; private set; }

        Stack<UIEnum> _openHistory = new ();

        public void RecordUI(UIEnum uIEnum)
        {
            _openHistory.Push(uIEnum);
        }

        public UIEnum PeekHistory()
        {
            if(_openHistory.Count > 0)
            {
                return _openHistory.Peek();    
            }
            return UIEnum.InValid;
        }

        public UIWindow CurrentTopWindow()
        {
            UIEnum uiEnum = PeekHistory();
            if(uiEnum == UIEnum.InValid)
            {
                return null;
            }
            return Get(uiEnum) as UIWindow;
        }
        
        UIWindowLifeConfig _uiWindowConfig;
        //IUISystem interface begin
        public IUIBase Get(UIEnum uiName)
        {
            IUIBase ui = null;
            if (!_uiCachedDic.TryGetValue(uiName, out ui))
            {
                return null;
            }

            return ui;
        }
        
        //IUISystem interface end

        private void OnOpenUI(IUIBase ui,Action<IUIBase> onComplete,UIOpenParam openParam,UILayer layer)
        {
            Transform parentNode = GetParentNode(layer);
            if (ui.uiTran.parent != parentNode)
            {
                ui.uiTran.parent = parentNode;
            }
            ui.uiTran.SetSiblingIndex(parentNode.childCount);
            ui.OnShow(openParam);
            onComplete?.Invoke(ui);
        }
        
        public void OpenUI(UIEnum uiName,Action<IUIBase> onComplete = null,UIOpenParam openParam = null,UILayer layer = UILayer.Bottom)
        {
            IUIBase ui = null;
            if (_uiCachedDic.TryGetValue(uiName, out ui))
            {
                OnOpenUI(ui, onComplete, openParam,layer);
            }
            else
            {
                bool isPermanent = _uiWindowConfig.WindowLife(uiName);
                LoadUIAsync(uiName, loadUi=>
                {
                    OnOpenUI(loadUi,onComplete,openParam,layer);
                },layer,isPermanent).Forget();
            }
        }
        
        public UniTask<IUIBase> OpenUIAsync(UIEnum uiName,UIOpenParam openParam = null,UILayer layer = UILayer.Bottom)
        {
            IUIBase ui = null;
            if (_uiCachedDic.TryGetValue(uiName, out ui))
            {
                OnOpenUI(ui, null, openParam, layer);
                return new UniTask<IUIBase>(ui);
            }
            bool isPermanent = _uiWindowConfig.WindowLife(uiName);
            return LoadUIAsync(uiName, (loadUi) =>
            {
                OnOpenUI(loadUi, null, openParam, layer);
            }, layer, isPermanent);
        }

        public void CloseUI(UIEnum uiName)
        {
            IUIBase ui = null;
            if (_uiCachedDic.TryGetValue(uiName, out ui))
            {
                ui.OnHide();
            }
        }

        public void CloseWindow(UIWindow uiWindow)
        {
            CloseUI(uiWindow.UIEnum);
        }

        private void DestroyUI(UIEnum uiName)
        {
            _uiCachedDic.Remove(uiName);
        }

        private Transform GetParentNode(UILayer layer)
        {
            switch (layer)
            {
                case UILayer.Bottom:
                    return _bottom;
                case UILayer.Center:
                    return _center;
                case UILayer.Top:
                    return _top;
                case UILayer.Banner:
                    return _banner;
                case UILayer.Popup:
                    return _popup;
                case UILayer.Guide:
                    return _guide;
            }

            return null;
        }
        
        private void LoadUI(UIEnum uiName,Action<IUIBase> onComplete,UILayer layer = UILayer.Bottom,bool isPermanent=false)
        {
            IUIBase ui = null;
            if (!_uiCachedDic.TryGetValue(uiName, out ui))
            {
                LoadUIAsync(uiName,(loadUi)=>
                {
                    onComplete?.Invoke(loadUi);
                },layer,isPermanent).Forget();
            }
        }
        
        #if USE_DI
        [Inject]
        private IObjectResolver _objectResolver;
        GameObject InstantiatePrefab(GameObject prefab,,Transform parentNode = null)
        {
            GameObject uiGameObject = _objectResolver.Instantiate(uiPrefab,parentNode);
            uiGameObject.transform.localScale = Vector3.one;
            uiGameObject.transform.SetParent(parentNode,false);
            return uiGameObject;
        }
        IUIBase CreateUIBaseInstance(Type uiType)
        {
            IUIBase ui = Activator.CreateInstance(uiType) as IUIBase;
            //依頼性注入
            _objectResolver.Inject(ui);
            return ui;
        }
        #else
        private GameObject InstantiatePrefab(GameObject uiPrefab,Transform parentNode = null)
        {
            GameObject uiGameObject = GameObject.Instantiate(uiPrefab,parentNode);
            uiGameObject.transform.localScale = Vector3.one;
            uiGameObject.transform.SetParent(parentNode,false);
            return uiGameObject;
        }
        private IUIBase CreateUIBaseInstance(Type uiType)
        {
            IUIBase ui = Activator.CreateInstance(uiType) as IUIBase;
            if(ui is UIWindow uiWindow)
            {
                uiWindow.UIManager = this;
            }
            return ui;
        }
        #endif
        private async UniTask<IUIBase> LoadUIAsync(UIEnum uiName, Action<IUIBase> onComplete,UILayer layer,bool isPermanent)
        {
            //クラスのプロパティーを取得する
            Type uiType = Type.GetType($"{_uiWindowConfig.uiWindowScopeName}.{uiName.ToString()},{_uiWindowConfig.uiCodeScopeName}");
            var attributes = uiType.GetCustomAttributes(false);
            var uiPath = attributes
                .Where(one => one is UIAttribute)
                .Select(tmp=> (tmp as UIAttribute).ResPath).FirstOrDefault();
            
            //リソースを読み込む
            var uiPrefab = await _resManager.LoadAssetAsync<GameObject>(uiPath);
            var parentNode = GetParentNode(layer);
            
            GameObject uiGameObject = InstantiatePrefab(uiPrefab,parentNode);
            
            //クラスを生成する
            IUIBase ui = CreateUIBaseInstance(uiType);
            
            ui.uiLayer = layer;
            ui.Init(uiGameObject);
            _uiCachedDic.Add(uiName,ui,isPermanent);
            onComplete?.Invoke(ui);
            return ui;
        }
        
        #if USE_DI
        [Inject]
        private UIManager(IResourceManager resManager)
        {
            _resManager = resManager;
            OnCreate();
            CreateHierarchyAgent();
        }
        #else
        
        public UIManager(IResourceManager resManager,UIWindowLifeConfig config)
        {
            _resManager = resManager;
            _uiWindowConfig = config;
            OnCreate();
            CreateHierarchyAgent();
        }
        #endif

        private GameObject _uiModule;
        private void OnCreate()
        {
            _uiCachedDic = new LRUCache<UIEnum, IUIBase>(10);
            _uiCachedDic.OnRemove += (ui) =>
            {
                ui.OnHide();
                ui.OnDestroy();
            };
            //UIModuleノードを取得する
            _uiModule = GameObject.Find("UIModule");

            var root = _uiModule.transform.Find("UIRoot");
            _rootCanvasScaler = root.GetComponent<CanvasScaler>();
            _rootCanvas = root.GetComponent<Canvas>();
            
            _bottom = _uiModule.transform.Find("UIRoot/Bottom");
            _center = _uiModule.transform.Find("UIRoot/Center");
            _top = _uiModule.transform.Find("UIRoot/Top");
            _banner = _uiModule.transform.Find("UIRoot/Banner");
            _popup = _uiModule.transform.Find("UIRoot/Popup");
            _guide = _uiModule.transform.Find("UIRoot/Guide");
            Object.DontDestroyOnLoad(_uiModule);
        }
        
        public void HideAllUI()
        {
            _uiModule.gameObject.SetActive(false);
        }

        public void ShowAllUI()
        {
            _uiModule.gameObject.SetActive(true);
        }
        [Conditional("UNITY_EDITOR")]
        private void CreateHierarchyAgent()
        {
            var uiModule = GameObject.Find("UIModule");
            UIManagerAgent agent = uiModule.AddComponent<UIManagerAgent>();
            agent.UIManagerInstance = this;
        }
#if UNITY_EDITOR
        public List<Node<UIEnum, IUIBase>> GetAllCache()
        {
            return _uiCachedDic.GetAllNodesByOrder();
        }
#endif
    }
}
