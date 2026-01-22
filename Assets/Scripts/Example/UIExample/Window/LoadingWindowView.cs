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
    [UI("Assets/Resources/OutGame/UIExample/Windows/LoadingWindow.prefab")]
    public partial class LoadingWindow : UIWindow
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
