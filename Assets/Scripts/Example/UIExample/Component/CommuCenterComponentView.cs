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
    [UI(0,"Assets/Resources/OutGame/UIExample/Components/CommuCenterComponent.prefab")]
    public partial class CommuCenterComponent : UIComponent
    {
		public Animation Anim_ButtonLayer;
		public OClickButton OBtn_Star;
		public OClickButton OBtn_Idol;
		public OClickButton OBtn_Support;
		public OClickButton OBtn_Event;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			Anim_ButtonLayer = go.transform.Find("Anim_ButtonLayer").GetComponent<Animation>();
			OBtn_Star = go.transform.Find("Anim_ButtonLayer/OBtn_Star").GetComponent<OClickButton>();
			OBtn_Idol = go.transform.Find("Anim_ButtonLayer/OBtn_Idol").GetComponent<OClickButton>();
			OBtn_Support = go.transform.Find("Anim_ButtonLayer/OBtn_Support").GetComponent<OClickButton>();
			OBtn_Event = go.transform.Find("Anim_ButtonLayer/OBtn_Event").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}