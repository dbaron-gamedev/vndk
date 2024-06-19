#if UNITY_EDITOR
using Core;
using Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class Sidekick : EditorWindow
    {
        private Player _player;
        private SaveSystem _saveSystem;
    
        [MenuItem("VNDK/Sidekick")]
        public static void ShowWindow()
        {
            GetWindow(typeof(Sidekick));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Take Screenshot", GUILayout.MinWidth(80)))
            {
                var currentScene = SceneManager.GetActiveScene().name;
                var hash = Random.Range(0, 10000);
                ScreenCapture.CaptureScreenshot(currentScene + "_" + hash + ".png", 4); 
            }
            
            if (GUILayout.Button("Delete Save", GUILayout.MinWidth(80)))
            {
                _saveSystem = new SaveSystem();
                _saveSystem.DeleteSave();
            }
        }
    }
}
#endif