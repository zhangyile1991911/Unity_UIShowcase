using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace UISystem.Core
{
    [Serializable,CreateAssetMenu(menuName = "UIConfig/CreateModuleConfig")]
    public class UIModuleConfig : ScriptableObject
    {
        public string uiCodeScopeName = "OutGame";
        public string uiWindowScopeName = "UIExample.Window";
        public string uiComponentScopeName = "UIExample.Component";
    }
}