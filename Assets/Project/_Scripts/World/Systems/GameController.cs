using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using UnityEngine.SceneManagement;

namespace Project._Scripts.World.Systems
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
