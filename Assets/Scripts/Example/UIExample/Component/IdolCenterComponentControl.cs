using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UISystem.Core;
using UISystem.Widget;

namespace Example.UIExample.Component
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    public partial class IdolCenterComponent : UIComponent
    {
        private Animation _animation;
        private CanvasGroup _canvasGroup;
        public IdolCenterComponent(GameObject go,UIWindow parent):base(go,parent)
        {
    		
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            _canvasGroup = uiTran.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _animation = uiTran.GetComponent<Animation>();
           
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            
            var state = _animation["IdolCenterShow"];
            state.wrapMode = WrapMode.Clamp;
            state.time = 0.0f;
            state.speed = 1f;
            _animation.Play("IdolCenterShow");
        }
    
        public override void OnHide()
        {
            var state = _animation["IdolCenterShow"];
            state.wrapMode = WrapMode.Clamp;
            state.time = state.length;
            state.speed = -1f;
            _animation.Play("IdolCenterShow");
            UniTask.WaitForSeconds(state.length).ContinueWith(() =>
            {
                _canvasGroup.alpha = 0;
                base.OnHide();
            }).Forget();
        }
    
        public override void OnUpdate()
        {
        }
    }
}