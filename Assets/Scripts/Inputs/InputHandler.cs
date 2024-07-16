using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inputs
{
    public class InputHandler : MonoBehaviour
    {
        private void Update()
        {
        
            if (Input.GetKeyUp(KeyCode.F2))
            {
                var currentScene = SceneManager.GetActiveScene().name;
            
                var hash = Random.Range(0, 10000);
            
                ScreenCapture.CaptureScreenshot(currentScene + "_" + hash + ".png", 4); 
            }
        
            if (Input.GetKeyUp(KeyCode.F3))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}