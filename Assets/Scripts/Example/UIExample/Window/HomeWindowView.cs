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
    [UI((int)UIEnum.HomeWindow,"Assets/Resources/OutGame/UIExample/Windows/HomeWindow.prefab")]
    public partial class HomeWindow : UIWindow
    {
		public Transform Ins_BG;
		public RectTransform RectTran_CenterNode;

        public override void Init(GameObject go)
        {
            uiGo = go;
            UIEnum = UIEnum.HomeWindow;
			Ins_BG = go.transform.Find("Ins_BG").GetComponent<Transform>();
			RectTran_CenterNode = go.transform.Find("RectTran_CenterNode").GetComponent<RectTransform>();

            base.Init(go);
        }
    }
}
