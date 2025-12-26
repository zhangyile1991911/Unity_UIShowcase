using System.Threading;
using UnityEngine;

namespace UISystem.Core
{
    public class UIComponent : IUIBase
    {
        public GameObject uiGo
        {
            get => _uiGo;
            set
            {
                uiRectTran = value.GetComponent<RectTransform>();
                _uiGo = value;
            }
        }
    
        public Transform uiTran
        {
            get => _uiGo.transform;
        }
    
        public RectTransform uiRectTran
        {
            get;
            private set;
        }
        public UILayer uiLayer
        {
            get;
            set;
        }
    
        public bool IsActive => uiGo.active;

        #if USE_DI
        [Inject]
        protected UIManager _uiManager;
        #else      
        protected UIManager _uiManager;
        #endif
        
        protected UIWindow ParentWindow
        {
            get => _parentWindow;
        }
        private GameObject _uiGo;
        private UIWindow _parentWindow;
        protected CancellationTokenSource _linkedToParentCTS { get;private set; }
        
        public CancellationToken token => _linkedToParentCTS.Token;
        
        public UIComponent(GameObject go,UIWindow parent)
        {
            _uiGo = go;
            _parentWindow = parent;
            _parentWindow?.AddChildComponent(this);
            uiRectTran = go?.GetComponent<RectTransform>();
        }
        
        public virtual void Init(GameObject go)
        {
            OnCreate();
        }
    
        public virtual void OnCreate()
        {
    
        }
    
        public virtual void OnDestroy()
        {
            _linkedToParentCTS?.Cancel();
            _linkedToParentCTS?.Dispose();
            _parentWindow.RemoveChildComponent(this);
            GameObject.Destroy(uiGo);
        }
    
        public virtual void OnShow(UIOpenParam openParam)
        {
            uiGo.SetActive(true);
            if (_linkedToParentCTS != null)
            {
                _linkedToParentCTS.Cancel();
                _linkedToParentCTS.Dispose();
                _linkedToParentCTS = null;    
            }
            _linkedToParentCTS = CancellationTokenSource.CreateLinkedTokenSource(_parentWindow.WindowCancellation.Token);
        }
    
        public virtual void OnHide()
        {
            uiGo.SetActive(false);
            if (_linkedToParentCTS != null)
            {
                _linkedToParentCTS.Cancel();
                _linkedToParentCTS.Dispose();
                _linkedToParentCTS = null;    
            }
        }
    
        public virtual void OnUpdate()
        {
        }
    }
}