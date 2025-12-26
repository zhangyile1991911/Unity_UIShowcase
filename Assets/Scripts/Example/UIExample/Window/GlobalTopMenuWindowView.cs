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
    [UI((int)UIEnum.GlobalTopMenuWindow,"Assets/Resources/OutGame/UIExample/Windows/GlobalTopMenuWindow.prefab")]
    public partial class GlobalTopMenuWindow : UIWindow
    {
		public Transform Ins_MainFooterMenu;
		public Transform Ins_SubFooterMenu;
		public Transform Ins_TopRes;

        public override void Init(GameObject go)
        {
            uiGo = go;
            UIEnum = UIEnum.GlobalTopMenuWindow;
			Ins_MainFooterMenu = go.transform.Find("Ins_MainFooterMenu").GetComponent<Transform>();
			Ins_SubFooterMenu = go.transform.Find("Ins_SubFooterMenu").GetComponent<Transform>();
			Ins_TopRes = go.transform.Find("Ins_TopRes").GetComponent<Transform>();

            base.Init(go);
        }
    }
}
