using Common;
using Common.Eventing;
using UnityEngine.SceneManagement;

namespace World.Systems
{
    class GameController : Singleton<GameController>
    {
        private const string WorldScene = "World";

        public void Start()
        {
            EventBus.Subscribe<Events.PlayerDied>(_ => GameOver());
        }

        private void GameOver()
        {
            SceneManager.LoadScene(WorldScene);
        }
    }
}
