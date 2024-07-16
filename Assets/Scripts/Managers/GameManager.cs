using Systems;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            InitializeGame();
        }

        private static void InitializeGame()
        {
            var saveSystem = new SaveSystem();
            var player = saveSystem.LoadPlayer();
            var checkpoint = player.checkpoint;
            
            if (checkpoint > 0)
                SceneManager.LoadScene(checkpoint);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}