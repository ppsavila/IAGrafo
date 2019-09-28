using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GraphSave : EditorWindow
{
        public List<Node> graph;
        private string gameDataProjectFilePath = "/StreamingAssets/graph.json";

        [MenuItem("Window/Save Data")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(GraphSave)).Show();
        }

        void OnGUI()
        {
            if (graph != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
                EditorGUILayout.PropertyField(serializedProperty, true);

                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }

            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }
        }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            graph = JsonUtility.FromJson<List<Node>>(dataAsJson);
        }
        else
        {
            graph = new List<Node>();
        }
    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(graph);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}
