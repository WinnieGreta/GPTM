using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils.Editor
{
    [InitializeOnLoad]
    public static class PlayFromTheFirstScene
    {
        private const string PLAY_FROM_FIRST_MENU_STR = "Edit/Always Start From Scene 0 &p";

        private static bool PlayFromFirstScene
        {
            get { return EditorPrefs.HasKey(PLAY_FROM_FIRST_MENU_STR) && EditorPrefs.GetBool(PLAY_FROM_FIRST_MENU_STR); }
            set { EditorPrefs.SetBool(PLAY_FROM_FIRST_MENU_STR, value); }
        }

        static PlayFromTheFirstScene()
        {
            // enabling "Start from first scene" by default
            if (!EditorPrefs.HasKey(PLAY_FROM_FIRST_MENU_STR))
            {
                PlayFromFirstScene = true;
            }
            LoadFirstSceneAtGameBegins();

            EditorBuildSettings.sceneListChanged += LoadFirstSceneAtGameBegins;
        }

        [MenuItem(PLAY_FROM_FIRST_MENU_STR, false, 150)]
        private static void PlayFromFirstSceneCheckMenu()
        {
            PlayFromFirstScene = !PlayFromFirstScene;
            Menu.SetChecked(PLAY_FROM_FIRST_MENU_STR, PlayFromFirstScene);

            ShowNotifyOrLog(PlayFromFirstScene ? "Play from scene 0" : "Play from current scene");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(PLAY_FROM_FIRST_MENU_STR, true)]
        private static bool PlayFromFirstSceneCheckMenuValidate()
        {
            Menu.SetChecked(PLAY_FROM_FIRST_MENU_STR, PlayFromFirstScene);
            return true;
        }

        private static void LoadFirstSceneAtGameBegins()
        {
            if (!PlayFromFirstScene)
            {
                EditorSceneManager.playModeStartScene = default;
                return;
            }

            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
                PlayFromFirstScene = false;
                return;
            }

            SceneAsset theScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            EditorSceneManager.playModeStartScene = theScene;
        }

        private static void ShowNotifyOrLog(string msg)
        {
            if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
            {
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            }
            else
            {
                Debug.Log(msg); // When there's no scene view opened, we just print a log
            }
        }
    }
}