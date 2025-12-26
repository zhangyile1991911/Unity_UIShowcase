using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using UISystem.Core;
//using SuperScrollView;
using UISystem.Widget;

namespace Example.UIExample.Component
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    [UI(0,"Assets/Resources/OutGame/UIExample/Components/SubFooterMenu.prefab")]
    public partial class SubFooterMenu : UIComponent
    {
		public OClickButton OBtn_home;
		public OClickButton OBtn_Help;
		public OClickButton OBtn_Back;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			OBtn_home = go.transform.Find("OBtn_home").GetComponent<OClickButton>();
			OBtn_Help = go.transform.Find("OBtn_Help").GetComponent<OClickButton>();
			OBtn_Back = go.transform.Find("OBtn_Back").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}