using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UISystem.Core;
using UISystem.Widget;
using Example.UIExample.Component;
using System;
using Cysharp.Threading.Tasks;

namespace Example.UIExample.Window
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    [UILifeTime(UILifeTimeType.Permanent)]
    public partial class GlobalTopMenuWindow : UIWindow
    {
        private TopResComponent _topRes;
        private FooterMenuComponent _mainFooterMenu;
        private SubFooterMenu _subFooterMenu;

        private bool _isIdle = true;

        internal readonly struct TransitionLocker : IDisposable
        {
            readonly GlobalTopMenuWindow _window;

            public TransitionLocker(GlobalTopMenuWindow window)
            {
                _window = window;
                _window._isIdle = false;
            }
            public void Dispose()
            {
                _window._isIdle = true;
            }
        }
        

        public override void OnCreate()
        {
            base.OnCreate();

            _topRes = this.NewComponentController<TopResComponent>(Ins_TopRes.gameObject);
            _mainFooterMenu = this.NewComponentController<FooterMenuComponent>(Ins_MainFooterMenu.gameObject);
            _subFooterMenu = this.NewComponentController<SubFooterMenu>(Ins_SubFooterMenu.gameObject);
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            _topRes.OnShow(null);
            _mainFooterMenu.OnShow(null);

            _mainFooterMenu.OBtn_Idol.ClickCondition((x)=>_isIdle,OnClickIdol,Token);
            _mainFooterMenu.OBtn_Home.ClickCondition((x)=>_isIdle,OnClickHome,Token);
            _mainFooterMenu.OBtn_Contest.ClickCondition((x)=>_isIdle,OnClickContest,Token);
            _mainFooterMenu.OBtn_Commu.ClickCondition((x)=>_isIdle,OnClickCommunication,Token);
            _mainFooterMenu.OBtn_Gacha.ClickCondition((x)=>_isIdle,OnClickGachaFull,Token);

            _subFooterMenu.OBtn_home.ClickOnceInSecond(OnClickBackHome,Token);
            _subFooterMenu.OBtn_Help.ClickOnceInSecond(OnClickHelp,Token);
            _subFooterMenu.OBtn_Back.ClickOnceInSecond(OnClickBack,Token);
        }
    
        public override void OnHide()
        {
            base.OnHide();
        }
    
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        private async UniTask OnClickIdol()
        {
            HomeWindow home = UIManager.Get<HomeWindow>();
            using(new TransitionLocker(this))
            {
                await home.TransitToIdol();
            }
        }

        private async UniTask OnClickHome()
        {
            HomeWindow home = UIManager.Get(typeof(HomeWindow)) as HomeWindow;
            using(new TransitionLocker(this))
            {
                await home.TransitToHome();
            }
        }
        
        private async UniTask OnClickContest()
        {
            HomeWindow home = UIManager.Get<HomeWindow>();
            using (new TransitionLocker(this))
            {
                await home.TransitToContest();
            }
        }

        private async UniTask OnClickCommunication()
        {
            HomeWindow home = UIManager.Get<HomeWindow>();
            using (new TransitionLocker(this))
            {
               await home.TransitToCommunication();
            }
        }

        private async UniTask OnClickGachaFull()
        {
            HomeWindow home = UIManager.Get<HomeWindow>();
            using (new TransitionLocker(this))
            {
                await home.TransitToGachaFull();
            }
        }

        private async UniTask OnClickBackHome()
        {
            UIWindow topWindow = UIManager.CurrentTopWindow();
            await topWindow.TransitionWithLoading<HomeWindow>();
            SwitchFooterToMain();
        }

        private async UniTask OnClickHelp()
        {
            
        }



        private async UniTask OnClickBack()
        {
            
        }



        public void SwitchFooterToMain()
        {
            _mainFooterMenu.OnShow(null);
            _subFooterMenu.OnHide();
        }

        public void SwitchFooterToSub()
        {
            _subFooterMenu.OnShow(null);
            _mainFooterMenu.OnHide();
        }

        
    }
}