using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem.Widget
{
    public enum TouchableTransition
    {
        None,
        ColorTint,
        SpriteSwap,
        Animation
    }
    public abstract class UITouchableBase : MonoBehaviour
    {
        protected Observable<PointerEventData> OnClickEvent
            => trigger.OnPointerClickAsObservable()
                .Where(x => x.button == PointerEventData.InputButton.Left)
                .Do(_ => PlayClickAnimation());
        protected Observable<PointerEventData> OnClickDownEvent => trigger.OnPointerDownAsObservable();
        protected Observable<PointerEventData> OnReleasedEvent => trigger.OnPointerUpAsObservable();
        [SerializeField] protected ObservableEventTrigger trigger;
    
        protected virtual void Awake()
        {
            if (ignoreAlphaTouch)
                touchImage.alphaHitTestMinimumThreshold = alphaThreshold;
        }
            
        [Header("透明の部分の当たり判定を無視する設定\nImageに登録した画像の設定をreadableにする必要があります")]
        [SerializeField] private bool ignoreAlphaTouch;
        [SerializeField] private float alphaThreshold;
    
        [SerializeField, Header("当たり判定用の画像")] 
        private Image touchImage;
    
        // エディター内生成機能用
    #if UNITY_EDITOR
        public Image TouchImage
        {
            get => touchImage;
            set
            {
                touchImage = value;
            }
        }
        public ObservableEventTrigger Trigger
        {
            get => trigger;
            set
            {
                trigger = value;
            }
        }
    #endif
        public void SetActive(bool active) => touchImage.enabled = active;
    
        [SerializeField] string clickAnimKey = "Click";
        [SerializeField] private Animator clickAnimator;
        public void PlayClickAnimation()
        {
            if (clickAnimator != null)
                clickAnimator.SetTrigger(clickAnimKey);
        }
    }
}


