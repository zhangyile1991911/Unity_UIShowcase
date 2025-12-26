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
    public partial class HomeCenterComponent : UIComponent
    {
        private Animation _animation;
        public HomeCenterComponent(GameObject go,UIWindow parent):base(go,parent)
        {
    		
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            _animation = uiTran.GetComponent<Animation>();
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            OBtn_Produce.ClickOnceInSecond(TransitionTo,token);
        }
    
        public override void OnHide()
        {
            base.OnHide();
        }
    
        public override void OnUpdate()
        {
            
        }

        public async UniTask RightFadeOut()
        {
            var state = _animation["RightToLeftFadeOut"];
            state.wrapMode = WrapMode.Clamp;
            state.time = 0.0f;
            state.speed = 1.0f;
            _animation.Play("RightToLeftFadeOut");
            await UniTask.WaitForSeconds(state.length);
        }
        
        public async UniTask RightFadeIn()
        {
            var state = _animation["RightToLeftFadeOut"];
            state.wrapMode = WrapMode.Clamp;
            state.time = state.length;
            state.speed = -1.0f;
            _animation.Play("RightToLeftFadeOut");
            await UniTask.WaitForSeconds(state.length);
        }

        public async UniTask LeftFadeOut()
        {
            var state = _animation["LeftToRightFadeOut"];
            state.wrapMode = WrapMode.Clamp;
            state.time = 0.0f;
            state.speed = 1.0f;
            _animation.Play("LeftToRightFadeOut");
            await UniTask.WaitForSeconds(state.length);
        }

        public async UniTask LeftFadeIn()
        {
            var state = _animation["LeftToRightFadeOut"];
            state.wrapMode = WrapMode.Clamp;
            state.time = state.length;
            state.speed = -1.0f;
            _animation.Play("LeftToRightFadeOut");
            await UniTask.WaitForSeconds(state.length);
        }

        public async UniTask TransitionTo()
        {
            await ParentWindow.TransitionWithLoading(UIEnum.ProduceWindow,null,UILayer.Center);
            ParentWindow.SwitchFooterPattern(false);
        }
    }
}