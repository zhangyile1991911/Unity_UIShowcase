using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace UISystem.Core
{
    [Serializable,CreateAssetMenu(menuName = "UIConfig/CreateWindowLifeConfig")]
    public class UIWindowLifeConfig : ScriptableObject
    {
        public List<UIEnum> uiWindowList;
        public string uiCodeScopeName = "OutGame";
        public string uiWindowScopeName = "UIExample.Window";
        public string uiComponentScopeName = "UIExample.Component";
        public bool WindowLife(UIEnum windowType)
        {
            foreach (var one in uiWindowList)
            {
                if (one == windowType)
                    return true;
            }
            return false;
        }
    }
}