using Enums;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CSVGeneratorWindow : EditorWindow
{
    private Vector2 scrollPos;
    private List<RowData> rows = new ();
    private string textFieldValue = "";

    [MenuItem("VNDK/Editor")]
    public static void ShowWindow()
    {
        GetWindow<CSVGeneratorWindow>("Scenario Editor");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < rows.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Character", GUILayout.Width(75));
            rows[i].name = EditorGUILayout.TextField(rows[i].name, GUILayout.Width(100));
            
            GUILayout.Space(25);
            
            EditorGUILayout.LabelField("Action", GUILayout.Width(50));
            rows[i].status = (Action)EditorGUILayout.EnumPopup(rows[i].status, GUILayout.Width(100));
            
            GUILayout.Space(25);
            
            EditorGUILayout.LabelField("Marker", GUILayout.Width(50));
            rows[i].marker = (MarkerPosition)EditorGUILayout.EnumPopup(rows[i].marker, GUILayout.Width(75));
            
            GUILayout.Space(25);
            
            EditorGUILayout.LabelField("Dialogue", GUILayout.Width(60));
            rows[i].message = EditorGUILayout.TextField(rows[i].message, GUILayout.Width(750));

            if (GUILayout.Button("Remove", GUILayout.Width(75))) 
                rows.RemoveAt(i);
            
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button("Add Line")) 
            rows.Add(new RowData());
        
        EditorGUILayout.EndScrollView();
            
        textFieldValue = EditorGUILayout.TextField("Enter text:", textFieldValue);
        
        if (GUILayout.Button("Generate Scenario"))
            Generate(textFieldValue);
    }

    private void Generate(string fileName)
    {
        var directoryPath = "Assets/Texts";
        var filePath = Path.Combine(directoryPath, $"{fileName}.txt");
        
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var row in rows)
            {
                var line = $"{row.name};{row.status};{row.marker};{row.message};";
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
        public MarkerPosition marker;
        public string message;
    }
}