using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem.Widget
{
    public class UILongButton : UIButton
    {
        public Observable<Unit> OnLongPressAsObservable => onLongPressSubject.AsObservable();
        private readonly Subject<Unit> onLongPressSubject = new();
        
        public Observable<Unit> OnCancelAsObservable => onCancelSubject.AsObservable();
        private readonly Subject<Unit> onCancelSubject = new();

        private bool isHandled = false;
        private bool isPressed = false;
        private float pressedTime = 0;
        
        [Tooltip("Seconds")]
        public float LongPressThreshold = 5f;
        
        public void Start()
        {
            OnClickAsObservable.Subscribe(OnClick);
            OnReleasedEvent.Subscribe(OnReleased);
        }

        private void OnClick(Unit clickedUnit)
        {
            isHandled = false;
            isPressed = true;
            pressedTime = Time.realtimeSinceStartup;

            Debug.Log($"UILongButton First Clicked {Time.realtimeSinceStartup}");
            //longClickDisposable = Observable.Timer(TimeSpan.FromSeconds(throttleSeconds)).Subscribe(OnLongClicked);
        }

        private void OnReleased(PointerEventData eventData)
        {
            if (!isHandled)
            {
                Debug.Log("OLongPressButton::OnPointerUp Cancelled");
                onCancelSubject.OnNext(Unit.Default);
            }
            isHandled = false;
            isPressed = false;

            Debug.Log($"UILongButton Released {eventData.clickTime}");
            onCancelSubject.OnNext(Unit.Default);
        }

        private void Update()
        {
            if (!isPressed)
            {
                return;
            }

            if (Time.realtimeSinceStartup - pressedTime >= LongPressThreshold)
            {
                isPressed = false;
                isHandled = true;
                Debug.Log($"OLongPressButton::OnLongPress {Time.realtimeSinceStartup} {LongPressThreshold}");
                onLongPressSubject.OnNext(Unit.Default);
            }
        }
    }    
}

