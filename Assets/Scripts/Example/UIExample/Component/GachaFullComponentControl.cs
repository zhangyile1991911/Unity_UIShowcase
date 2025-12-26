using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UISystem.Core;
using UISystem.Widget;
using UnityEngine.Playables;

namespace Example.UIExample.Component
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    public partial class GachaFullComponent : UIComponent
    {
        private PlayableDirector _director;
        public GachaFullComponent(GameObject go,UIWindow parent):base(go,parent)
        {
    		
        }
        
        public override void OnCreate()
        {
            base.OnCreate();
            _director = uiTran.GetComponent<PlayableDirector>();
            uiRectTran.anchoredPosition = new Vector2(1080, 0);
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void OnShow(UIOpenParam openParam)
        {
            base.OnShow(openParam);
            var pa = _director.playableAsset;
            _director.initialTime = 0;
            _director.timeUpdateMode = DirectorUpdateMode.GameTime;
            _director.Play(pa,DirectorWrapMode.Hold);
        }
    
        public override void OnHide()
        {
            var pa = _director.playableAsset;
            _director.initialTime = pa.duration;
            _director.timeUpdateMode = DirectorUpdateMode.Manual;
            
            _director.Play(pa,DirectorWrapMode.Hold);
            UniTask.WaitUntil(() =>
            {
                _director.time -= Time.deltaTime;
                _director.Evaluate();
                if (_director.time <= 0.0f)
                {
                    base.OnHide();
                    return true;
                }

                return false;
            }).Forget();
            // UniTask.WaitForSeconds((float)pa.duration).ContinueWith(() =>
            // {
            //     
            // }).Forget();
        }
    
        public override void OnUpdate()
        {
            
        }
    }
}