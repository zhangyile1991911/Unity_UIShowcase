using System.IO;
using UISystem.Core;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class UIAutoCreateEditorWindow : EditorWindow
    {
        private string newUIName;
        private GameObject uiRootGo;

        [MenuItem("Custom Tools/UI生成/UIコード生成ツール", false, 10)]
        static void ShowEditor()
        {
            UIAutoCreateEditorWindow window = GetWindow<UIAutoCreateEditorWindow>();
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.titleContent.text = "UIコード生成ツール";
        }
        [MenuItem("Custom Tools/UI生成/UIコードリフレッシュ", false, 11)]
        public static void RefreshAllClass()
        {
            UIAutoCreateInfoConfig config = AssetDatabase.LoadAssetAtPath<UIAutoCreateInfoConfig>("Assets/Editor/UIAutoCreateInfoConfig.asset");
            // Componentクラスリフレッシュ
            string[] componentPrefabPaths = Directory.GetFiles(config.ComponentPrefabPath, "*.prefab", SearchOption.AllDirectories);
            foreach (string path in componentPrefabPaths)
            {
                GameObject componentPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string uiName = componentPrefab.name.Replace("UI", "");
                new UIClassAutoCreate().CreateComponent(uiName, componentPrefab, config, true);
            }
            
            // Windowクラスリフレッシュ
            string[] windowPrefabPaths = Directory.GetFiles(config.WindowPrefabPath, "*.prefab", SearchOption.AllDirectories);
            foreach (string path in windowPrefabPaths)
            {
                GameObject windowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string uiName = windowPrefab.name.Replace("UI", "");
                new UIClassAutoCreate().CreateWindow(uiName, windowPrefab, config, true);
            }
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
                        uiRootGo = (GameObject)EditorGUILayout.ObjectField(uiRootGo, typeof(GameObject), true);
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
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
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

            var targetPath = config.WindowScriptPath;
            CheckTargetPath(targetPath);
            new UIClassAutoCreate().CreateWindow(uiName,uiRootGo,config);
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
            
            var targetPath = config.WindowScriptPath;
            CheckTargetPath(targetPath);
            
            new UIClassAutoCreate().CreateComponent(uiName,uiRootGo,config);
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