using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Example.UIExample.Window;
using UnityEngine;

namespace UISystem.Core
{
    public static class UITransition
    {
        public enum TransitionAnimType
        {
            DotGridAnim,
        }

        public enum TransitionSlideDirection
        {
            Left,Right,Top,Bottom
        }
        
        const string LeftFadeOutAnimationName = "LeftFadeOut";
        const string RightFadeOutAnimationName = "RightFadeOut";
        
        public static UIManager _uiManager;
        
        //元画面が左へ移動と共にフェードアウト
        public static async UniTaskVoid TransitionToLeftFadeOut<T>(this UIWindow uiWindow,
            UIOpenParam openParam = null,
            UILayer uiLayer = UILayer.Bottom,
            bool isRecord = true)where T : UIWindow
        {
            Type openWindow = typeof(T);
            Type currentTop = _uiManager.PeekHistory();
            if (currentTop != null)
            {
                if (openWindow == currentTop) return;
            
                UIWindow currentTopWindow = _uiManager.Get(currentTop) as UIWindow;
                Animation animation = currentTopWindow.uiTran.GetComponent<Animation>();
                animation.Play(LeftFadeOutAnimationName);
                await UniTask.WaitUntil(() => animation.isPlaying == false,cancellationToken: uiWindow.Token);    
            }
            
            UIWindow nextWindow = await _uiManager.OpenUIAsync<T>(null,UILayer.Center) as UIWindow;
            if (isRecord)
            {
                _uiManager.RecordUI(openWindow);
            }
        }
        
        public static async UniTaskVoid TransitionToRightFadeOut<T>(this UIWindow uiWindow,
            UIOpenParam openParam = null,
            Action<IUIBase> onFinish = null,
            UILayer uiLayer = UILayer.Bottom,
            bool isRecord = true)where T : UIWindow
        {
            Type currentTop = _uiManager.PeekHistory();
            Type openWindowType = typeof(T);
            if (currentTop != null)
            {
                if (openWindowType == currentTop) return;
            
                UIWindow currentTopWindow = _uiManager.Get(currentTop) as UIWindow;
                Animation animation = currentTopWindow.uiTran.GetComponent<Animation>();
                animation.Play(RightFadeOutAnimationName);
                await UniTask.WaitUntil(() => animation.isPlaying == false,cancellationToken: uiWindow.Token);    
            }
           
            UIWindow nextWindow = await _uiManager.OpenUIAsync<T>(null,UILayer.Center) as UIWindow;
            if (isRecord)
            {
                _uiManager.RecordUI(openWindowType);
            }
        }

        public static async void SwitchFooterPattern(this UIWindow uIWindow,
            bool isMain)
        {
            GlobalTopMenuWindow topMenuWindow = _uiManager.Get<GlobalTopMenuWindow>();
            if(isMain)
            {
                topMenuWindow.SwitchFooterToMain();
            }
            else
            {
                topMenuWindow.SwitchFooterToSub();
            }
        }

        public static async UniTask TransitionWithLoading<T>(this UIWindow uIWindow,
            UIOpenParam openParam = null,
            UILayer uiLayer = UILayer.Center,
            bool isRecord = true)where T : UIWindow
        {
            //1. LoadingWindowを表示しておく
            //2. 遷移先を読み込んでから表示する
            //3. LoadingWindowを非表示する
            var openWindowType = typeof(T);
            //LoadingWindowは常駐ですから、ゲーム開始時に生成されてインメモリーに置いておいた
            _uiManager.OpenUI<LoadingWindow>(null,null,UILayer.Top);

            _uiManager.CloseWindow(uIWindow);

            await _uiManager.OpenUIAsync<T>(openParam,uiLayer);
            //仮に遷移先が重いです
            await UniTask.WaitForSeconds(1.5f);
            if(isRecord)
            {
                _uiManager.RecordUI(openWindowType);
            }

            _uiManager.CloseUI(typeof(LoadingWindow));

        }
        
    }   
}