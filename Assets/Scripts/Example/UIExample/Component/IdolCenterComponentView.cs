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
    [UI(0,"Assets/Resources/OutGame/UIExample/Components/IdolCenterComponent.prefab")]
    public partial class IdolCenterComponent : UIComponent
    {
		public OClickButton OBtn_Memory;
		public OClickButton OBtn_Support;
		public OClickButton OBtn_Training;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			OBtn_Memory = go.transform.Find("OBtn_Memory").GetComponent<OClickButton>();
			OBtn_Support = go.transform.Find("OBtn_Support").GetComponent<OClickButton>();
			OBtn_Training = go.transform.Find("OBtn_Training").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}