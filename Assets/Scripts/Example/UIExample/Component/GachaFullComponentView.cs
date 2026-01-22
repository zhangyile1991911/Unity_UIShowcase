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
    [UI("Assets/Resources/OutGame/UIExample/Components/GachaFullComponent.prefab")]
    public partial class GachaFullComponent : UIComponent
    {
		public Image Img_BG;
		public OClickButton OBtn_Gacha;
		public OClickButton OBtn_Plus;
		public OClickButton OBtn_Sub;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			Img_BG = go.transform.Find("Img_BG").GetComponent<Image>();
			OBtn_Gacha = go.transform.Find("OBtn_Gacha").GetComponent<OClickButton>();
			OBtn_Plus = go.transform.Find("OBtn_Plus").GetComponent<OClickButton>();
			OBtn_Sub = go.transform.Find("OBtn_Sub").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}