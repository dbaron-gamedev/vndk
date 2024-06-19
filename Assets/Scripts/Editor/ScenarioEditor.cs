using Enums;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CSVGeneratorWindow : EditorWindow
{
    private List<RowData> rows = new List<RowData>();
    private Vector2 scrollPos;

    [MenuItem("VNDK/Editor")]
    public static void ShowWindow()
    {
        GetWindow<CSVGeneratorWindow>("CSV Generator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add Line"))
        {
            rows.Add(new RowData());
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < rows.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            rows[i].name = EditorGUILayout.TextField("Name", rows[i].name);
            rows[i].status = (Action)EditorGUILayout.EnumPopup("Action", rows[i].status);
            rows[i].code = EditorGUILayout.TextField("Code", rows[i].code);
            rows[i].message = EditorGUILayout.TextField("Message", rows[i].message);

            if (GUILayout.Button("Remove"))
            {
                rows.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Generate CSV"))
        {
            GenerateCSV();
        }
    }

    private void GenerateCSV()
    {
        string directoryPath = "Assets/Texts";
        string filePath = Path.Combine(directoryPath, "output.txt");

        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var row in rows)
            {
                string line = $"{row.name};{row.status};{row.code};{row.message};";
                writer.WriteLine(line);
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("CSV Generator", "CSV file generated successfully in Assets/Texts!", "OK");
    }


    [System.Serializable]
    private class RowData
    {
        public string name;
        public Action status;
        public string code;
        public string message;
    }
}