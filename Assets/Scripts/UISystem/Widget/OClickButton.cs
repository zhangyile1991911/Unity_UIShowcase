using UnityEngine;
using R3;

namespace UISystem.Widget
{
    public class OClickButton : UIButton
    {
        protected override void Awake()
        {
            base.Awake();
            OnClickAsObservable.Subscribe(PlaySound).AddTo(this);
        }
        
        // クリックサウンド
        [SerializeField] protected string clickSoundName = "button_click_1";
        protected virtual void PlaySound(Unit unit)
        {
         
        }   
    }    
}

   
