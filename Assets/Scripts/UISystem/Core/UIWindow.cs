using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UISystem.Core
{
    public class UIWindow : IUIBase
    {
        public GameObject uiGo
        {
            get => _uiGo;
            set => _uiGo = value;
        }
        public Transform uiTran
        {
            get => _uiGo.transform;
        }
        private GameObject _uiGo;

        public RectTransform uiRectTran
        {
            get;
            private set;
        }
        
        public UILayer uiLayer
        {
            get => _uiLayer;
            set => _uiLayer = value;
        }
        
        public bool IsActive => uiGo.activeSelf;

        private List<UIComponent> _childComponent = new List<UIComponent>(10);
#if UNITY_EDITOR
        public int ChildComponentCount => _childComponent.Count;
        public long ShowTimestamp;
        public long ShowCount;
#endif    

#if USE_DI
        [Inject]
        protected UIManager _uiManager;
#else
        public UIManager UIManager;
#endif
        public CancellationTokenSource WindowCancellation { get; private set; }
        public CancellationToken Token => WindowCancellation.Token;
        
        private UILayer _uiLayer;
        
        public virtual void Init(GameObject go)
        {
            uiRectTran = go.GetComponent<RectTransform>();
            OnCreate();
        }

        public virtual void OnCreate()
        {

        }
        

        public virtual void OnDestroy()
        {
            //todo 清理子widget
            foreach (var component in _childComponent)
            {
                component.OnDestroy();
            }
            WindowCancellation?.Cancel();
            WindowCancellation?.Dispose();
            GameObject.Destroy(uiGo);
        }

        public virtual void OnShow(UIOpenParam openParam)
        {
            uiGo.SetActive(true);
            if (WindowCancellation != null)
            {
                WindowCancellation.Cancel();
                WindowCancellation.Dispose();
                WindowCancellation = null;
            }
            WindowCancellation = new CancellationTokenSource();
        }

        public virtual void OnHide()
        {
            uiGo.SetActive(false);
            for (int i = 0; i < _childComponent.Count; i++)
            {
                _childComponent[i]?.OnHide();
            }

            if (WindowCancellation != null)
            {
                WindowCancellation.Cancel();
                WindowCancellation.Dispose();
                WindowCancellation = null;    
            }
        }

        public virtual void OnUpdate()
        {
            
        }

        public void AddChildComponent(UIComponent uiComponent)
        {
            if (uiComponent != null)
            {
                _childComponent.Add(uiComponent);    
            }
        }

        public void RemoveChildComponent(UIComponent uiComponent)
        {
            if (uiComponent != null)
            {
                _childComponent.Remove(uiComponent);    
            }
        }
    }
}