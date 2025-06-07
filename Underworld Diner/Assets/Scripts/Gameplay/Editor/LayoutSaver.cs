using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Editor
{
    [Serializable]
    internal class PrefabData
    {
        public string guid;
        public Vector2 position;
    }

    [Serializable]
    internal class LayoutData
    {
        public List<PrefabData> layout;
    }
    
    public static class LayoutSaver
    {
        private static string SerializeLayoutToJson(IEnumerable<GameObject> furniture)
        {
            var prefabData = furniture.Select(x => new PrefabData
            {
                position = x.transform.position,
                guid = AssetDatabase.AssetPathToGUID(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(x))
            }).ToList();
            
            var layoutData = new LayoutData
            {
                layout = prefabData
            };
            
            return JsonUtility.ToJson(layoutData);
        }

        private static void SpawnItem(PrefabData prefabData, Transform parent)
        {
            var path = AssetDatabase.GUIDToAssetPath(prefabData.guid);
            var prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            instance.transform.position = prefabData.position;
        }

        private static void SetUpLayout(LayoutData layoutData)
        {
            var layoutRoot = GameObject.FindGameObjectWithTag("LayoutRoot");
            foreach (var prefabData in layoutData.layout)
            {
                SpawnItem(prefabData, layoutRoot.transform);
            }
        }
        
        [MenuItem("Edit/DS/Save Layout")]
        private static void SaveLayout()
        {
            var levelLayout = GameObject.FindGameObjectsWithTag("SerializeLayout");
            var layoutJson = SerializeLayoutToJson(levelLayout);
            Debug.Log("Saved Layout: " + layoutJson);

            var path = EditorUtility.SaveFilePanel("LayoutJson", 
                $"{Application.dataPath}/Library/Layouts",
                "layout.json",
                "json");
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Failed saving file - invalid path");
                return;
            }

            using (var savedFile = File.CreateText(path))
            {
                savedFile.Write(layoutJson);
            }
        }

        [MenuItem("Edit/DS/Load Layout")]
        private static void LoadLayout()
        {
            var path = EditorUtility.OpenFilePanel("LayoutJson", 
                $"{Application.dataPath}/Library/Layouts",
                "json");

            string json;
            using (var r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }
            Debug.Log("Loaded Layout: " + json);
            
            var layout = JsonUtility.FromJson<LayoutData>(json);
            ClearLayout();
            SetUpLayout(layout);
        }

        [MenuItem("Edit/DS/Clear Layout")]
        private static void ClearLayout()
        {
            var levelLayout = GameObject.FindGameObjectsWithTag("SerializeLayout");
            foreach (var table in levelLayout)
            {
                GameObject.DestroyImmediate(table);
            }
        }
    }
}