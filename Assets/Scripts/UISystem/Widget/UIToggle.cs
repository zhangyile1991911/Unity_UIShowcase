using System;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace UISystem.Widget
{
    public class UIToggle : UIButton
    {
        public Observable<bool> OnValueChangeAsObservable => onValueChangeSubject.AsObservable();
        private readonly Subject<bool> onValueChangeSubject = new();

        [SerializeField] private TouchableTransition transitionType;
        public TouchableTransition TransitionType { get => transitionType; }
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.ColorTint)] private Color unselectedColor = new Color(255,255,255,255);
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.ColorTint)] private Color selectedColor = new Color(255,255,255,255);
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.ColorTint)] private TextMeshProUGUI buttonText;
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.ColorTint)] private Color unselectedTextColor = new Color(255,255,255,255);
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.ColorTint)] private Color selectedTextColor = new Color(255,255,255,255);
        // TODO : clicked color?
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.SpriteSwap)] private Sprite unselectedSprite;
        [SerializeField, ConditionalHide("transitionType", (int)TouchableTransition.SpriteSwap)] private Sprite selectedSprite;
        // TODO : clicked Sprite?

        private UIToggleGroup group;
        public UIToggleGroup Group
        {
            get { return group; }
            set
            {
                group = value;
            }
        }
        private bool isOn;
        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                isOn = value;
                TransitImage(isOn);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            OnClickAsObservable.Subscribe(OnToggleClicked);
        }
        private void TransitImage(bool isActive)
        {
            switch (transitionType)
            {
                case TouchableTransition.ColorTint:
                    Color color = isActive ? selectedColor : unselectedColor;
                    buttonImage.color = color;
                    if (buttonText)
                    {
                        Color textColor = isActive ? selectedTextColor : unselectedTextColor;
                        buttonText.color = textColor;
                    }
                    break;
                case TouchableTransition.SpriteSwap:
                    Sprite sprite = isActive ? selectedSprite : unselectedSprite;
                    buttonImage.sprite = sprite;
                    break;
                case TouchableTransition.None:
                case TouchableTransition.Animation:
                    break;
            }
        }
        private void OnToggleClicked(Unit unit)
        {
            onValueChangeSubject.OnNext(true);
        }
    }
}
