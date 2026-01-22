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
    [UI("Assets/Resources/OutGame/UIExample/Components/ContestCenterComponent.prefab")]
    public partial class ContestCenterComponent : UIComponent
    {
		public OClickButton OBtn_Course;
		public OClickButton OBtn_Contest;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			OBtn_Course = go.transform.Find("OBtn_Course").GetComponent<OClickButton>();
			OBtn_Contest = go.transform.Find("OBtn_Contest").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}