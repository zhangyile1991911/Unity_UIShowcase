using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UISystem.Core
{
    public class UIManager
    {
        private LRUCache<Type, IUIBase> _uiCachedDic;

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

        Stack<Type> _openHistory = new ();

        public void RecordUI(Type uIEnum)
        {
            _openHistory.Push(uIEnum);
        }

        public Type PeekHistory()
        {
            if(_openHistory.Count > 0)
            {
                return _openHistory.Peek();    
            }
            return null;
        }

        public UIWindow CurrentTopWindow(bool pop = false)
        {
            Type uiType = PeekHistory();
            if(uiType == null)
            {
                return null;
            }

            if (pop)
            {
                _openHistory.Pop();
            }
            return Get(uiType) as UIWindow;
        }
        
        //IUISystem interface begin
        public IUIBase Get(Type uiName)
        {
            IUIBase ui = null;
            if (!_uiCachedDic.TryGetValue(uiName, out ui))
            {
                return null;
            }

            return ui;
        }

        public T Get<T>()where T : UIWindow
        {
            return Get(typeof(T)) as T;
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
        
        public void OpenUI<T>(Action<IUIBase> onComplete = null,UIOpenParam openParam = null,UILayer layer = UILayer.Bottom)
        {
            IUIBase ui = null;
            Type uiType = typeof(T);
            if (_uiCachedDic.TryGetValue(uiType, out ui))
            {
                OnOpenUI(ui, onComplete, openParam,layer);
            }
            else
            {
                UILifeTime uiLifeTime = uiType.GetCustomAttribute<UILifeTime>();
                LoadUIAsync(uiType, loadUi=>
                {
                    OnOpenUI(loadUi,onComplete,openParam,layer);
                },layer,uiLifeTime.IsPermanent).Forget();
            }
        }
        
        public UniTask<IUIBase> OpenUIAsync<T>(UIOpenParam openParam = null,UILayer layer = UILayer.Bottom)
        {
            IUIBase ui = null;
            Type uiType = typeof(T);
            if (_uiCachedDic.TryGetValue(uiType, out ui))
            {
                OnOpenUI(ui, null, openParam, layer);
                return new UniTask<IUIBase>(ui);
            }
            UILifeTime uiLifeTime = uiType.GetCustomAttribute<UILifeTime>();
            return LoadUIAsync(uiType, (loadUi) =>
            {
                OnOpenUI(loadUi, null, openParam, layer);
            }, layer, uiLifeTime.IsPermanent);
        }

        public void CloseUI(Type uiName)
        {
            IUIBase ui = null;
            if (_uiCachedDic.TryGetValue(uiName, out ui))
            {
                ui.OnHide();
            }
        }

        public void CloseWindow(UIWindow uiWindow)
        {
            CloseUI(uiWindow.GetType());
        }

        private void DestroyUI(Type uiName)
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
        
        private void LoadUI(Type uiType,Action<IUIBase> onComplete,UILayer layer = UILayer.Bottom,bool isPermanent=false)
        {
            IUIBase ui = null;
            if (!_uiCachedDic.TryGetValue(uiType, out ui))
            {
                LoadUIAsync(uiType,(loadUi)=>
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
        private async UniTask<IUIBase> LoadUIAsync(Type uiType, Action<IUIBase> onComplete,UILayer layer,bool isPermanent)
        {
            //クラスのプロパティーを取得する
            var attributes = uiType.GetCustomAttribute<UIAttribute>(false);
            
            //リソースを読み込む
            var uiPrefab = await _resManager.LoadAssetAsync<GameObject>(attributes.ResPath);
            var parentNode = GetParentNode(layer);
            
            GameObject uiGameObject = InstantiatePrefab(uiPrefab,parentNode);
            
            //クラスを生成する
            IUIBase ui = CreateUIBaseInstance(uiType);
            
            ui.uiLayer = layer;
            ui.Init(uiGameObject);
            _uiCachedDic.Add(uiType,ui,isPermanent);
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
        
        public UIManager(IResourceManager resManager)
        {
            _resManager = resManager;
            OnCreate();
            CreateHierarchyAgent();
        }
        #endif

        private GameObject _uiModule;
        private void OnCreate()
        {
            _uiCachedDic = new LRUCache<Type, IUIBase>(10);
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
        public List<Node<Type, IUIBase>> GetAllCache()
        {
            return _uiCachedDic.GetAllNodesByOrder();
        }
#endif
    }
}
