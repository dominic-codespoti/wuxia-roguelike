using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Teleporter : MonoBehaviour
    {
        [field: SerializeField] public string Scene { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(Scene);
            }
        }
    }
}
