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
    [UI("Assets/Resources/OutGame/UIExample/Components/TopResComponent.prefab")]
    public partial class TopResComponent : UIComponent
    {
		public Image Img_PLv;
		public Transform Ins_Turn;
		public Transform Ins_Money;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			Img_PLv = go.transform.Find("Img_PLv").GetComponent<Image>();
			Ins_Turn = go.transform.Find("Ins_Turn").GetComponent<Transform>();
			Ins_Money = go.transform.Find("Ins_Money").GetComponent<Transform>();

            base.Init(go);
    	}
    }
}