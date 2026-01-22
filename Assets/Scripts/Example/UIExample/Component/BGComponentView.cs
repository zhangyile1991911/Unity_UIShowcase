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
    [UI("Assets/Resources/OutGame/UIExample/Components/BGComponent.prefab")]
    public partial class BGComponent : UIComponent
    {
		public Image Img_Bg;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			Img_Bg = go.transform.Find("Img_Bg").GetComponent<Image>();

            base.Init(go);
    	}
    }
}