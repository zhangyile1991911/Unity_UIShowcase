using System;
using Cysharp.Threading.Tasks;
using Example.UIExample.Component;
using R3;
using UISystem.Core;

namespace Example.UIExample.Window
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    public partial class HomeWindow : UIWindow
    {
        private BGComponent _bgComponent;
        //
        private HomeCenterComponent _homeCenterComponent;
        private IdolCenterComponent _idolCenterComponent;
        private ContestCenterComponent _contestCenterComponent;
        private CommuCenterComponent _commuCenterComponent;
        private GachaFullComponent _gachaFullComponent;
        

        private UIComponent _currentShowCenter;
        
       
        
        public override void OnCreate()
        {
            base.OnCreate();
            //ゼロから生成される
            _homeCenterComponent = this.CreateUIComponent<HomeCenterComponent>(RectTran_CenterNode);
            _currentShowCenter = _homeCenterComponent;
            //プレハブ内に既存ノードに基づいて制御クラスを生成する
            _bgComponent = this.NewComponentController<BGComponent>(Ins_BG.gameObject);
            // _topResComponent = this.NewComponentController<TopResComponent>(Ins_TopRes.gameObject);
            // _footerMenuComponent = this.NewComponentController<FooterMenuComponent>(Ins_FooterMenu.gameObject);
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            
            _currentShowCenter.OnShow(null);
        }
        
        public override void OnHide()
        {
            base.OnHide();
        }
    
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        
        public async UniTask TransitToIdol()
        {
            _idolCenterComponent ??= await this.CreateUIComponentAsync<IdolCenterComponent>(RectTran_CenterNode);
            
            switch (_currentShowCenter)
            {
                case IdolCenterComponent:
                    return;
                case HomeCenterComponent homeCenter:
                    await homeCenter.RightFadeOut();
                    homeCenter.OnHide();
                    await UniTask.WaitUntil(()=>homeCenter.uiGo.activeSelf == false);
                    break;
                default:
                    _currentShowCenter.OnHide();
                    await UniTask.WaitUntil(()=>_currentShowCenter.uiGo.activeSelf == false);
                    break;
            }

            _currentShowCenter = _idolCenterComponent;
            _currentShowCenter.OnShow(null);
        }

        public async UniTask TransitToHome()
        {
            switch (_currentShowCenter)
            {
                case HomeCenterComponent:
                    return;
                case IdolCenterComponent:
                case CommuCenterComponent:
                    _currentShowCenter.OnHide();
                    _homeCenterComponent.OnShow(null);
                    await _homeCenterComponent.RightFadeIn();
                    break;
                case ContestCenterComponent:
                case GachaFullComponent:
                    _currentShowCenter.OnHide();
                    _homeCenterComponent.OnShow(null);
                    await _homeCenterComponent.LeftFadeIn();
                    break;
                default:
                    break;
            }
            _currentShowCenter = _homeCenterComponent;
        }
        
        public async UniTask TransitToCommunication()
        {
            
            _commuCenterComponent ??= await this.CreateUIComponentAsync<CommuCenterComponent>(RectTran_CenterNode);
        
            switch (_currentShowCenter)
            {
                case CommuCenterComponent:
                    return;
                case HomeCenterComponent homeCenter:
                    await homeCenter.LeftFadeOut();
                    homeCenter.OnHide();
                    await UniTask.WaitUntil(()=>homeCenter.uiGo.activeSelf == false);
                    break;
                default:
                    _currentShowCenter.OnHide();
                    await UniTask.WaitUntil(()=>_currentShowCenter.uiGo.activeSelf == false);
                    break;
            }

            _currentShowCenter = _commuCenterComponent;
            _currentShowCenter.OnShow(null);
        }
        
        public async UniTask TransitToContest()
        {
            _contestCenterComponent ??= await this.CreateUIComponentAsync<ContestCenterComponent>(RectTran_CenterNode);
            
            switch (_currentShowCenter)
            {
                case ContestCenterComponent:
                    return;
                case HomeCenterComponent homeCenter:
                    await homeCenter.LeftFadeOut();
                    homeCenter.OnHide();
                    await UniTask.WaitUntil(()=>homeCenter.uiGo.activeSelf == false);
                    break;
                default:
                    _currentShowCenter.OnHide();
                    await UniTask.WaitUntil(()=>_currentShowCenter.uiGo.activeSelf == false);
                    break;
            }

            _currentShowCenter = _contestCenterComponent;
            _currentShowCenter.OnShow(null);
        }
        
        public async UniTask TransitToGachaFull()
        {
            _gachaFullComponent ??= await this.CreateUIComponentAsync<GachaFullComponent>(RectTran_CenterNode);
            
            switch (_currentShowCenter)
            {
                case GachaFullComponent:
                    return;
                case HomeCenterComponent homeCenter:
                    await homeCenter.RightFadeOut();
                    homeCenter.OnHide();
                    await UniTask.WaitUntil(()=>homeCenter.uiGo.activeSelf == false);
                    break;
                default:
                    _currentShowCenter.OnHide();
                    await UniTask.WaitUntil(()=>_currentShowCenter.uiGo.activeSelf == false);
                    break;
            }

            _currentShowCenter = _gachaFullComponent;
            _currentShowCenter.OnShow(null);
        }
    }
}