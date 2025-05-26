using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;


namespace Utils.Editor
{
    public class CreateLevelDialog : EditorWindow
    {
        private TextField _input;

        private GameObject _rootObject;
        private string _workingDirectory;
        private string _scenePath;
        private string _sceneName;
        private Transform[] _playerSpawners;
        private GameObject _objectRoot;
        private GameObject _playerRoot;


        [MenuItem("Tools/DS/Create level")]
        public static void OpenCreateLevelDialog()
        {
            // called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<CreateLevelDialog>();
            wnd.titleContent = new GUIContent("Create New Level");
            wnd.minSize = new Vector2(350, 200);
            wnd.maxSize = new Vector2(350, 200);
        }

        public void CreateGUI()
        {
            _input = new TextField("Level name");
            var button = new Button(OnCreate);
            button.text = "Create";
            rootVisualElement.Add(_input);
            rootVisualElement.Add(button);
        }

        private void OnCreate()
        {
            const string SCENE_TEMPLATE = "Assets/Scenes/Levels/{0}.unity";
            _sceneName = _input.text;
            _scenePath = string.Format(SCENE_TEMPLATE, _sceneName);

            if (EditorSceneManager.GetSceneByPath(_scenePath).IsValid())
            {
                EditorUtility.DisplayDialog("Error", $"Scene with this name {_scenePath} already exists", "OK");
            }
            else
            {
                CreateScene();
            }
        }

        private void CreateScene()
        {
            AssetDatabase.CreateFolder($"Assets/Scenes/Levels", _sceneName);
            _workingDirectory = $"Assets/Scenes/Levels/{_sceneName}";
            AssetDatabase.CreateFolder(_workingDirectory, "Settings");
            var settingsDirectory = Path.Combine(_workingDirectory, "Settings");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Library/Prefabs/TemplateLevel.prefab");
            _rootObject = PrefabUtility.InstantiatePrefab(rootPrefab) as GameObject;
            _rootObject.name = "LevelRoot";

            var levelSettingsPath = Path.Combine(settingsDirectory, $"{_sceneName}_LevelSettings.asset");
            var spawnSettingsPath = Path.Combine(settingsDirectory, $"{_sceneName}_SpawnSettings.asset");
            AssetDatabase.CopyAsset("Assets/Library/Settings/Templates/LevelSettingsTemplate.asset",
                levelSettingsPath);
            AssetDatabase.CopyAsset("Assets/Library/Settings/Templates/MonsterSpawnSettingsTemplate.asset",
                spawnSettingsPath);
            var context = _rootObject.GetComponent<SceneContext>();

            List<ScriptableObjectInstaller> list = new();
            list.Add(AssetDatabase.LoadAssetAtPath<ScriptableObjectInstaller>(levelSettingsPath));
            list.Add(AssetDatabase.LoadAssetAtPath<ScriptableObjectInstaller>(spawnSettingsPath));

            context.ScriptableObjectInstallers = list;


            PrefabUtility.SaveAsPrefabAssetAndConnect(_rootObject, $"{_workingDirectory}/{_sceneName}.prefab",
                InteractionMode.AutomatedAction);
            EditorSceneManager.SaveScene(scene, _scenePath);
            AddSceneToBuild();


            Close();
        }

        private void AddSceneToBuild()
        {
            var scenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i];
            }

            var editorScene = new EditorBuildSettingsScene(_scenePath, true);
            //Log scene always goes last
            scenes[EditorBuildSettings.scenes.Length] = scenes[EditorBuildSettings.scenes.Length - 1];
            scenes[EditorBuildSettings.scenes.Length - 1] = editorScene;
            EditorBuildSettings.scenes = scenes;
        }

        private GameObject GeneratePrefab(string prefabName, Action<GameObject> createCallback)
        {
            var prefab = new GameObject(prefabName);
            prefab.transform.SetParent(_rootObject.transform);
            createCallback?.Invoke(prefab);
            PrefabUtility.SaveAsPrefabAssetAndConnect(prefab, $"{_workingDirectory}/{_sceneName}_{prefabName}.prefab",
                InteractionMode.AutomatedAction);
            return prefab;
        }
    }
}