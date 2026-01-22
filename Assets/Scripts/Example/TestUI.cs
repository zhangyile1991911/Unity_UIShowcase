using System;
using UnityEngine;
using UISystem.Core;
using Example.UIExample.Window;

public class TestUI : MonoBehaviour
{
    UIManager _uiManager;
    
    public TestResourceManager testResourceManager;
    private void Awake()
    {
        testResourceManager = new TestResourceManager();
        _uiManager = new UIManager(testResourceManager);
        //DIを使わず場合　手動で渡す
        UITransition._uiManager = _uiManager;
        UIHelper._resourceManager = testResourceManager;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //LoadingWindowを生成しておく
        _uiManager.OpenUI<LoadingWindow>((ui) =>
        {
            ui.OnHide();
        },null,UILayer.Top);

        _uiManager.OpenUI<GlobalTopMenuWindow>((ui) =>
        {
            GlobalTopMenuWindow topMenuWindow = ui as GlobalTopMenuWindow;
            topMenuWindow.SwitchFooterToMain();
        },null,UILayer.Top);
        
        _uiManager.OpenUI<HomeWindow>(null,null,UILayer.Center);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
