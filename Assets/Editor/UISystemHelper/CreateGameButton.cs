using UISystem.Widget;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using R3.Triggers;

public class CreateGameButton
{
    // TODO : 汎用化
    private const string UISpritePath = "UI/Skin/UISprite.psd";
    
    [MenuItem("GameObject/UI/Common/ClickButton",false,0)]
    public static OClickButton CreateClickButton(MenuCommand menuCommand)
    { 
        return CreateOutGameButtonImplement<OClickButton>(menuCommand,"OClickButton","ClickButton");
    }
    
    [MenuItem("GameObject/UI/Common/LongPressButton",false,1)]
    public static UILongButton CreateLongButton(MenuCommand menuCommand)
    {
        return CreateOutGameButtonImplement<UILongButton>(menuCommand,"OLongPressButton","LongPressButton",12);
    }
    [MenuItem("GameObject/UI/Common/ToggleButton",false,0)]
    public static UIToggle CreateToggleButton(MenuCommand menuCommand)
    {
        return CreateOutGameButtonImplement<UIToggle>(menuCommand,"Toggle","Toggle");
    }
    [MenuItem("GameObject/UI/Common/ToggleGroup", false, 1)]
    public static UIToggleGroup CreateToggleGroup(MenuCommand menuCommand)
    {
        return CreateUIObject<UIToggleGroup>(menuCommand, "ToggleGroup");
    }

    private static T CreateOutGameButtonImplement<T>(MenuCommand menuCommand, string buttonDefaultName, string textDefaultName, int fontSize = 14) where T : UIButton
    {
        GameObject go = new GameObject(buttonDefaultName, typeof(RectTransform));
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(160, 30);


        Canvas canvas = EnsureCanvas();

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        if (go.transform.parent == null)
        {
            go.transform.SetParent(canvas.transform, false);
        }

        Image image = AddImageComponent(go);

        T btn = go.AddComponent<T>();
        btn.ButtonImage = image;
        btn.TouchImage = image;

        ObservableEventTrigger trigger = go.AddComponent<ObservableEventTrigger>();
        btn.Trigger = trigger;

        CreateButtonText(go, textDefaultName, fontSize);

        Undo.RegisterCreatedObjectUndo(go, $"Create {buttonDefaultName}");
        Selection.activeObject = go;

        return btn;
    }

    private static T CreateUIObject<T>(MenuCommand menuCommand, string buttonDefaultName) where T : MonoBehaviour
    {
        GameObject go = new GameObject(buttonDefaultName, typeof(RectTransform));

        Canvas canvas = EnsureCanvas();
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        if (go.transform.parent == null)
        {
            go.transform.SetParent(canvas.transform, false);
        }

        T component = go.AddComponent<T>();
        return component;
    }
    
    private static Canvas EnsureCanvas()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            // Canvas生成
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // Canvasのレイヤを設置
            canvasGO.layer = LayerMask.NameToLayer("UI");

            // EventSystemに紐付け
            if (Object.FindFirstObjectByType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }
        return canvas;
    }

    private static Image AddImageComponent(GameObject go)
    {
        Image image = go.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(UISpritePath);
        image.type = Image.Type.Sliced;
        return image;
    }

    private static void CreateButtonText(GameObject parent,string defaultText,int fontSize)
    {
        GameObject textGO = new GameObject("Text(TMP)");
        GameObjectUtility.SetParentAndAlign(textGO,parent);
        
        TextMeshProUGUI textMesh = textGO.AddComponent<TextMeshProUGUI>();
        textMesh.text = defaultText;
        textMesh.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textMesh.verticalAlignment = VerticalAlignmentOptions.Middle;
        textMesh.fontSize = fontSize;
        textMesh.color = Color.black;
    }
    
    
}
