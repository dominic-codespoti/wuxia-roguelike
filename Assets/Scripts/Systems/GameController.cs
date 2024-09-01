using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    class GameController : MonoBehaviour
    {
        private const string GameScene = "Game";
        private const string StartScene = "Scene";
        private const string HubScene = "Hub";

        public void Start()
        {
            EventBus.Subscribe<Events.PlayerDied>(_ => GameOver());
        }

        private void GameOver()
        {
            SceneManager.LoadScene(HubScene);
        }
    }
}
