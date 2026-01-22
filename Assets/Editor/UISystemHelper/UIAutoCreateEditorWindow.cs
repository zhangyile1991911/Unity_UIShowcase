using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UISystem.Core;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class UIAutoCreateEditorWindow : EditorWindow
    {
        private string newUIName;
        private GameObject uiRootGo;

        private static List<string> UIComponentBase = new List<string>();
        private static string[] WindowBaseArray;
        private static int UIComponentBaseIndex = 0;
        private static List<string> UIWindowBase = new List<string>();
        private static string[] ComponentBaseArray;
        private static int UIWindowBaseIndex = 0;

        [MenuItem("Custom Tools/UI生成/UIコード生成ツール", false, 10)]
        static void ShowEditor()
        {
            UIAutoCreateEditorWindow window = GetWindow<UIAutoCreateEditorWindow>();
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.titleContent.text = "UIコード生成ツール";

            //継承可能な基底クラスを収集する
            UIComponentBaseIndex = 0;
            UIWindowBaseIndex = 0;
            
            UIComponentBase.Clear();
            UIComponentBase.Add(nameof(UIComponent));
            
            UIWindowBase.Clear();
            UIWindowBase.Add(nameof(UIWindow));
            Assembly GameSystem = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(s => s.GetName().Name == "UISystem");
            foreach (var one in GameSystem.GetTypes())
            {
                bool isUIComponent = one.IsSubclassOf(typeof(UIComponent));
                if (isUIComponent)
                {
                    UIComponentBase.Add(one.Name);
                }
                bool isUIWindow = one.IsSubclassOf(typeof(UIWindow));
                if (isUIWindow)
                {
                    UIWindowBase.Add(one.Name);
                }
            }

            CollectionUIBaseAssembly();
        }

        static void CollectionUIBaseAssembly()
        {
            UIComponentBase.Clear();
            UIComponentBase.Add(nameof(UIComponent));
            
            UIWindowBase.Clear();
            UIWindowBase.Add(nameof(UIWindow));
            Assembly GameSystem = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(s => s.GetName().Name == "UISystem");
            foreach (var one in GameSystem.GetTypes())
            {
                bool isUIComponent = one.IsSubclassOf(typeof(UIComponent));
                if (isUIComponent)
                {
                    UIComponentBase.Add(one.Name);
                }
                bool isUIWindow = one.IsSubclassOf(typeof(UIWindow));
                if (isUIWindow)
                {
                    UIWindowBase.Add(one.Name);
                }
            }

            WindowBaseArray = UIWindowBase.ToArray();
            ComponentBaseArray = UIComponentBase.ToArray();
        }

        private void OnGUI()
        {//创建窗口
            #region GUIStyle设置

            Color fontColor = Color.white;
            GUIStyle titleStyle = new GUIStyle() { fontSize = 18, alignment = TextAnchor.MiddleCenter };
            titleStyle.normal.textColor = fontColor;

            GUIStyle sonTittleStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleCenter };
            sonTittleStyle.normal.textColor = fontColor;

            GUIStyle leftStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleLeft };
            leftStyle.normal.textColor = fontColor;

            GUIStyle littoleStyle = new GUIStyle() { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            littoleStyle.normal.textColor = fontColor;

            #endregion
            
            GUILayout.BeginArea(new Rect(0, 0, 600, 600));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.TextArea("注意事項:\n" +
                                                 "UIの名前は「UI」を接頭辞とする必要があります\n" +
                                                 "例: UITest\n" +
                                                 "UIコンポーネントの命名規則については\n" +
                                                 "UIViewAutoCreateConfig ファイルを参照してください", 
                                                leftStyle,
                                                GUILayout.Width(600));    
                    }
                    
                    GUILayout.EndHorizontal();
            
                    GUILayout.Space(10);
            
                    GUI.skin.button.wordWrap = true;

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("コード生成",leftStyle,GUILayout.Width(600));    
                    }
                    
                    GUILayout.EndHorizontal();
            
                    GUILayout.Space(10);
            
                    GUILayout.BeginHorizontal();
                    {
                        // GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField("Prefabノード",leftStyle,GUILayout.Width(100));
                        var newuiRootGo = (GameObject)EditorGUILayout.ObjectField(uiRootGo, typeof(GameObject), true);
                        if(newuiRootGo != uiRootGo)
                        {
                            uiRootGo = newuiRootGo;
                            UpdateParentClassChoice();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
            
                    GUILayout.Space(10);
            
                    GUILayout.BeginHorizontal();
                    {
                        // GUILayout.FlexibleSpace();
                        if (GUILayout.Button("ウィンドウを生成する", GUILayout.Width(160), GUILayout.Height(35)))
                        {
                            GeneratorWindow(uiRootGo);
                        }
                        if (GUILayout.Button("コンポーネントを生成する", GUILayout.Width(160), GUILayout.Height(35)))
                        {
                            GeneratorComponent(uiRootGo);
                        }
                        GUILayout.FlexibleSpace();    
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        if(WindowBaseArray == null || ComponentBaseArray == null)
                        {
                            CollectionUIBaseAssembly();
                        }
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Window:",leftStyle,GUILayout.Width(60));
                        UIWindowBaseIndex = EditorGUILayout.Popup(UIWindowBaseIndex,WindowBaseArray,GUILayout.Width(80));
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Component:",leftStyle,GUILayout.Width(90));
                        UIComponentBaseIndex = EditorGUILayout.Popup(UIComponentBaseIndex,ComponentBaseArray,GUILayout.Width(80));
                        GUILayout.EndHorizontal();
                        GUILayout.FlexibleSpace();  
                    }
                    GUILayout.EndHorizontal();

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        private Type GetParentClassType()
        {
            if(uiRootGo == null)return null;
            string className = uiRootGo.name;
            var uiCodeAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(s => s.GetName().Name == "Example");
            foreach (var one in uiCodeAssembly.GetTypes())
            {
                if (one.Name == className)
                {
                    return one.BaseType;
                }
            }
            return null;
        }
        
        private void UpdateParentClassChoice()
        {
            var existedParentType = GetParentClassType();
            UIWindowBaseIndex =0;
            UIComponentBaseIndex =0;
            if(existedParentType != null)
            {
                if(existedParentType.IsSubclassOf(typeof(UIWindow)))
                {
                    UIWindowBaseIndex = Array.IndexOf(WindowBaseArray,existedParentType.Name);
                }
                else if(existedParentType.IsSubclassOf(typeof(UIComponent)))
                {
                    UIComponentBaseIndex = Array.IndexOf(ComponentBaseArray,existedParentType.Name);
                }
            }
        }
        
        private void GeneratorWindow(GameObject go)
        {
            var config = AssetDatabase.LoadAssetAtPath<UIAutoCreateInfoConfig>("Assets/Settings/UIAutoCreateInfoConfig.asset");
            CreateWindowPrefab(go,config);
            CreateWindowUIClass(config);
        }
        
        private void CreateWindowPrefab(GameObject gameObject,UIAutoCreateInfoConfig config)
        {
            //检查目录
            if (!AssetDatabase.IsValidFolder(config.WindowPrefabPath))
            {
                throw new System.Exception($"パス:{config.WindowPrefabPath}が該当しません");
            }

            var uiName = GetUIName();
            var localPath = string.Format("{0}/{1}.prefab", config.WindowPrefabPath, uiName);
            if (File.Exists(localPath))
            {
                Debug.Log($"{uiName}.prefabがすでに存在しています");
                return;
            }
            //确保prefab唯一
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            //创建一个prefab 并且输出日志
            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction, out prefabSuccess);
            if (prefabSuccess)
                Debug.Log($"{uiName}.prefabが生成されました");
            else
                Debug.Log($"{uiName}.prefabが生成されません");
            AssetDatabase.Refresh();
        }
        
        private void CreateWindowUIClass(UIAutoCreateInfoConfig config)
        {
            if (uiRootGo == null) throw new System.Exception("生成するノードを入れてください");
            string uiName = GetUIName();
            string parentClassName = GetUIWindowParentName();
            var targetPath = config.WindowScriptPath;
            CheckTargetPath(targetPath);
            new UIClassAutoCreate().CreateWindow(uiName,parentClassName,uiRootGo,config);
        }
        
        private void GeneratorComponent(GameObject go)
        {
            var config = AssetDatabase.LoadAssetAtPath<UIAutoCreateInfoConfig>("Assets/Settings/UIAutoCreateInfoConfig.asset");
            CreateComponentClass(config);
            CreateComponentPrefab(go,config);
        }

        private void CreateComponentClass(UIAutoCreateInfoConfig config)
        {
            if (uiRootGo == null) throw new System.Exception("プレハブのルートノードをドラッグ＆ドロップしてください。");
            string uiName = GetUIName();
            
            var parentClassName = GetUIComponentParentName();
            var targetPath = config.WindowScriptPath;
            CheckTargetPath(targetPath);
            
            new UIClassAutoCreate().CreateComponent(uiName,parentClassName,uiRootGo,config);
        }

        private void CreateComponentPrefab(GameObject gameObject,UIAutoCreateInfoConfig config)
        {
            //フォルダを確認する
            if (!Directory.Exists(config.ComponentPrefabPath))
            {
                throw new System.Exception($"{config.ComponentPrefabPath}パスは該当しません");
            }

            var uiName = GetUIName();
            var localPath = string.Format("{0}/{1}.prefab", config.ComponentPrefabPath, uiName);
            if (File.Exists(localPath))
            {
                Debug.Log($"{uiName}.prefabは既存です");
                return;
            }
            //确保prefab唯一
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            //创建一个prefab 并且输出日志
            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction, out prefabSuccess);
            if (prefabSuccess)
                Debug.Log($"{uiName}.prefabが生成されました");
            else
                Debug.Log($"{uiName}.prefabが生成されません");
            AssetDatabase.Refresh();
        }
        
        private string GetUIName()
        {
            string uiName = uiRootGo.name.Replace("UI", "");
            return uiName;
        }

        private string GetUIWindowParentName()
        {
            return UIWindowBase[UIWindowBaseIndex];
        }

        private string GetUIComponentParentName()
        {
            return UIComponentBase[UIComponentBaseIndex];
        }
        
        private void CheckTargetPath(string targetPath)
        {
            string[] road = targetPath.Split('/');
            string findPath = road[0] + "/" + road[1];
            for (int i = 2; i < road.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder(findPath+"/"+road[i]))
                {
                    AssetDatabase.CreateFolder(findPath,road[i]);
                }

                findPath = findPath + "/" + road[i];
            }
        }
    }
}