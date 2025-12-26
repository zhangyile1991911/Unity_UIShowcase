using System;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem.Core
{
    [Serializable]
    public class UIFieldRule
    {
        public string prefixName;
        public string typeName;
    }

    [Serializable,CreateAssetMenu(menuName = "UIConfig/CreateAutoCreateInfoConfig")]
    public class UIAutoCreateInfoConfig : ScriptableObject
    {
        public List<UIFieldRule> uiInfoList;

        public string UIWindowViewTemplatePath;
        public string UIWindowControlTemplatePath;
        public string UIComponentViewTemplatePath;
        public string UIComponentControlTemplatePath;
        public string WindowScriptPath;
        public string ComponentScriptPath;
        public string WindowPrefabPath;
        public string ComponentPrefabPath;
        public string WindowEnumScriptPath;
    }
    
    
   
}