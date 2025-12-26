using System;
using System.Linq;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using R3;
using UISystem.Core;
using UISystem.Widget;
using UnityEngine;

/**
 * 便利なメソッドが提供されている
 * ボタンのクリックメソッドを登録する
 * WindowがComponentを生成する
 */
public static class UIHelper
{
    public static IResourceManager _resourceManager;

    /// <summary>
    /// ボタンが押されるイベントを紐つける
    /// </summary>
    /// <param name="condition">条件が成立される時に呼び出される</param>
    public static void ClickCondition(this OClickButton button,
        Func<Unit,bool> condition,
        Func<UniTask> onClick,
        CancellationToken token)
    {
        button.
            OnClickAsObservable.
            TakeWhile(condition).
            Subscribe((x)=>
            {
                onClick();
            }).RegisterTo(token);
    }
    

    /// <summary>
    /// ボタンが押されるイベントを紐付ける
    /// </summary>
    /// <param name="throttleTime">ｘ秒間で一回しか呼び出さない</param>
    public static void ClickOnceInSecond(this OClickButton button,
        Func<UniTask> onClick,
        CancellationToken token,
        float throttleTime = 1.0f)
    {
        button.OnClickDownAsObservable
            .ThrottleFirst(TimeSpan.FromSeconds(throttleTime))
            .Subscribe((x)=>
            {
                onClick();
            })
            .RegisterTo(token);
    }

    private static GameObject CreateGameObject(GameObject prefab,Transform parentNode)
    {
        #if USE_DI
            return GetUILifeTimeScope().Instantiate(uiPrefab,node);
        #else
            return GameObject.Instantiate(prefab, parentNode);
        #endif
    }

    private static T CreateInstance<T>( GameObject uiInstance,UIWindow uiWindow)where T : UIComponent
    {
        T uiComponent = Activator.CreateInstance(typeof(T), new object[]{uiInstance,uiWindow}) as T;
        if (uiComponent == null)
        {
            Debug.LogError($"{typeof(T)}が生成されない");
            return null;
        }
        #if USE_DI
        //依頼性注入
        var objectResolver = GetUILifeTimeScope();
        objectResolver.Inject(uiComponent);
        #endif
        //ライフサイクル関数を呼ぶ
        uiComponent.Init(uiInstance);
        return uiComponent;
    }
    
    public static T CreateUIComponent<T>(this UIWindow uiWindow,Transform parentNode)where T : UIComponent
    {
        //クラスのプロパティーを取得する
        var typePath = $"{typeof(T)},Example";
        Type componentType = Type.GetType(typePath);
        if (componentType == null)
        {
            Debug.LogError($"CreateUIComponent {typePath} 該当componentTypeがありません");
            return null;
        }
            
        var attributes = componentType.GetCustomAttributes(false);
        var uiPath = attributes
            .Where(one => one is UIAttribute)
            .Select(tmp=> (tmp as UIAttribute).ResPath).FirstOrDefault();

        //プレハブを読み込む
        GameObject uiPrefab = _resourceManager.LoadAsset<GameObject>(uiPath);
        if (uiPrefab == null)
        {
            Debug.LogError($"CreateUIComponent {uiPath} 該当プレハブがありません");
            return null;
        }
        var uiGameObject = CreateGameObject(uiPrefab,parentNode);
        uiGameObject.transform.localPosition = Vector3.zero;
        uiGameObject.transform.localScale = Vector3.one;
        T uiComponent = CreateInstance<T>(uiGameObject,uiWindow);
        if (uiComponent == null)
        {
            Debug.LogError($"{componentType}が生成されない");
            return null;
        }
        
        return uiComponent;
    }

    public static async UniTask<T> CreateUIComponentAsync<T>(this UIWindow uiWindow,Transform parentNode)where T : UIComponent
    {
        //クラスのプロパティーを取得する
        var typePath = $"{typeof(T)},Example";
        Type componentType = Type.GetType(typePath);
        if (componentType == null)
        {
            Debug.LogError($"CreateUIComponent {typePath} 該当componentTypeがありません");
            return null;
        }
            
        var attributes = componentType.GetCustomAttributes(false);
        var uiPath = attributes
            .Where(one => one is UIAttribute)
            .Select(tmp=> (tmp as UIAttribute).ResPath).FirstOrDefault();

        //プレハブを読み込む
        GameObject uiPrefab = await _resourceManager.LoadAssetAsync<GameObject>(uiPath);
        if (uiPrefab == null)
        {
            Debug.LogError($"CreateUIComponent {uiPath} 該当プレハブがありません");
            return null;
        }
        var uiGameObject = CreateGameObject(uiPrefab,parentNode);
        uiGameObject.transform.localPosition = Vector3.zero;
        uiGameObject.transform.localScale = Vector3.one;
        T uiComponent = CreateInstance<T>(uiGameObject,uiWindow);
        if (uiComponent == null)
        {
            Debug.LogError($"{componentType}が生成されない");
            return null;
        }
        return uiComponent;
    }
    
    public static T NewComponentController<T>(this UIWindow uiWindow, GameObject uiGameObject) where T : UIComponent
    {
        var typePath = $"{typeof(T)},Example";
        Type componentType = Type.GetType(typePath);
            
        T uiComponent = CreateInstance<T>(uiGameObject,uiWindow);
        if (uiComponent == null)
        {
            Debug.LogError($"{componentType}が生成されない");
            return null;
        }
#if USE_DI
        //uiGameObjectがUnityより生成されたので、ここに手動で注入する必要がある
        GetUILifeTimeScope().InjectGameObject(uiGameObject);
        GetUILifeTimeScope().Inject(uiComponent);
#endif
        uiComponent.Init(uiGameObject);
        return uiComponent;
    }
    
    
}
