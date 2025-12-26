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
    [UI(0,"Assets/Resources/OutGame/UIExample/Components/HomeCenterComponent.prefab")]
    public partial class HomeCenterComponent : UIComponent
    {
		public OClickButton OBtn_Produce;
		public OClickButton OBtn_Event;
		public OClickButton OBtn_Shop;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			OBtn_Produce = go.transform.Find("OBtn_Produce").GetComponent<OClickButton>();
			OBtn_Event = go.transform.Find("OBtn_Event").GetComponent<OClickButton>();
			OBtn_Shop = go.transform.Find("OBtn_Shop").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}