using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem.Widget
{
    // TODO : Subscribeが重複する懸念あり
    public class UIToggleGroup : MonoBehaviour
    {
        [SerializeField] private List<UIToggle> toggles = new List<UIToggle>();
        public List<UIToggle> Toggles => toggles;
        public readonly ReactiveProperty<int> SelectedIndex = new(0);

        private void Start()
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                BindToggle(toggles[i]);
            }

            SelectedIndex.Subscribe(idx =>
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    toggles[i].IsOn = (i == idx);
                }
            }).AddTo(this);
        }

        private void BindToggle(UIToggle toggle)
        {
            toggle.OnValueChangeAsObservable
            .Where(isOn => isOn)
            .Subscribe(_ =>
            {
                int index = toggles.IndexOf(toggle);
                if (index >= 0)
                {
                    SelectedIndex.Value = index;
                }
            })
            .AddTo(toggle);  // Bind to toggle's lifecycle
        }


        public void AddToggle(UIToggle toggle)
        {
            if (toggles.Contains(toggle))
                return;

            toggles.Add(toggle);
            BindToggle(toggle);
        }

        public void RemoveToggle(UIToggle toggle)
        {
            int index = toggles.IndexOf(toggle);
            if (index == -1)
                return;

            toggles.RemoveAt(index);

            if (SelectedIndex.Value == index)
            {
                SelectedIndex.Value = -1;
            }
            else if (SelectedIndex.Value > index)
            {
                SelectedIndex.Value -= 1;
            }
        }

        public void ClearToggles()
        {
            toggles.Clear();
            SelectedIndex.Value = -1;
        }
    }
}