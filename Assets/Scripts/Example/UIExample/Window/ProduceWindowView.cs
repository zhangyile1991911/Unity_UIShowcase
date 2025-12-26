using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
//using SuperScrollView;
using UISystem.Core;
using UISystem.Widget;

namespace Example.UIExample.Window
{
    /// <summary>
    /// Auto Generate Class!!!
    /// </summary>
    [UI((int)UIEnum.ProduceWindow,"Assets/Resources/OutGame/UIExample/Windows/ProduceWindow.prefab")]
    public partial class ProduceWindow : UIWindow
    {
		public OClickButton OBtn_Regular;
		public OClickButton OBtn_Archive;
		public OClickButton OBtn_Ranking;
		public OClickButton OBtn_Seminar;

        public override void Init(GameObject go)
        {
            uiGo = go;
            UIEnum = UIEnum.ProduceWindow;
			OBtn_Regular = go.transform.Find("OBtn_Regular").GetComponent<OClickButton>();
			OBtn_Archive = go.transform.Find("OBtn_Archive").GetComponent<OClickButton>();
			OBtn_Ranking = go.transform.Find("OBtn_Ranking").GetComponent<OClickButton>();
			OBtn_Seminar = go.transform.Find("OBtn_Seminar").GetComponent<OClickButton>();

            base.Init(go);
        }
    }
}
