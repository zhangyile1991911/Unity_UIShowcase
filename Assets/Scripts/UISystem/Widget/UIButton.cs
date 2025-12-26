using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem.Widget
{
    public class UIButton : UITouchableBase, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image buttonImage;

        public Observable<Unit> OnClickAsObservable => OnClickEvent.AsUnitObservable();
        public Observable<Unit> OnClickDownAsObservable => OnClickDownEvent.AsUnitObservable();
        public Observable<Unit> OnReleaseAsObservable => OnReleasedEvent.AsUnitObservable();

        public Observable<Vector2> OnClickToPosition => OnClickEvent.Select(x => x.position);
        
#if UNITY_EDITOR
        public Image ButtonImage
        {
            get => buttonImage;
            set
            {
                buttonImage = value;
            }
        }
#endif

        public void SetSprite(Sprite sprite) => buttonImage.sprite = sprite;
        public void SetVisible(bool visible) => buttonImage.enabled = visible;

        // !Mock2!
        public Observable<PointerEventData> OnPointOver => trigger.OnPointerEnterAsObservable();
        private readonly Subject<PointerEventData> onPointOverSubject = new();
        public Observable<PointerEventData> OnPointExit => trigger.OnPointerExitAsObservable();
        private readonly Subject<PointerEventData> onPointExitSubject = new();
        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointOverSubject.OnNext(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointExitSubject.OnNext(eventData);
        }
    }
}


