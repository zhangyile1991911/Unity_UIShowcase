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
    public partial class ContestCenterComponent : UIComponent
    {
        private Animation _animation;
        private CanvasGroup _canvasGroup;
        public ContestCenterComponent(GameObject go,UIWindow parent):base(go,parent)
        {
    		
        }
        
        public override void OnCreate()
        {
            base.OnCreate();   
            _animation = uiTran.GetComponent<Animation>();
            _canvasGroup = uiTran.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            var state = _animation["ContestCenterShow"];
            state.wrapMode = WrapMode.Clamp;
            state.time = 0.0f;
            state.speed = 1f;
            _animation.Play("ContestCenterShow");
            _canvasGroup.alpha = 1;
        }
    
        public override void OnHide()
        {
            var state = _animation["ContestCenterShow"];
            state.wrapMode = WrapMode.Clamp;
            state.time = state.length;
            state.speed = -2f;
            _animation.Play("ContestCenterShow");
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