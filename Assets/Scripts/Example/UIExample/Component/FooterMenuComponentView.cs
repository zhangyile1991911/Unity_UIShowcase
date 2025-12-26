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
    [UI(0,"Assets/Resources/OutGame/UIExample/Components/FooterMenuComponent.prefab")]
    public partial class FooterMenuComponent : UIComponent
    {
		public OClickButton OBtn_Home;
		public OClickButton OBtn_Idol;
		public OClickButton OBtn_Commu;
		public OClickButton OBtn_Contest;
		public OClickButton OBtn_Gacha;

    
    	public override void Init(GameObject go)
    	{
    	    uiGo = go;
    	    
			OBtn_Home = go.transform.Find("OBtn_Home").GetComponent<OClickButton>();
			OBtn_Idol = go.transform.Find("OBtn_Idol").GetComponent<OClickButton>();
			OBtn_Commu = go.transform.Find("OBtn_Commu").GetComponent<OClickButton>();
			OBtn_Contest = go.transform.Find("OBtn_Contest").GetComponent<OClickButton>();
			OBtn_Gacha = go.transform.Find("OBtn_Gacha").GetComponent<OClickButton>();

            base.Init(go);
    	}
    }
}