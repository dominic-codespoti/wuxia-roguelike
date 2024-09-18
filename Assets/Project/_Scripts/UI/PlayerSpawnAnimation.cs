using System.Collections;
using Project._Scripts.Common.Eventing;
using UnityEngine;

namespace Project._Scripts.UI
{
    public class PlayerSpawnAnimation : MonoBehaviour
    {
        [SerializeField] private float spawnDuration = 1.5f;    // Total time for the animation
        [SerializeField] private float floatDistance = 0.5f;    // How much the player floats up/down
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            EventBus.Subscribe<Events.LevelGenerated>(evt => StartSpawnAnimation());
        }

        private void StartSpawnAnimation()
        {
            StartCoroutine(SpawnAnimation());
        }

        private IEnumerator SpawnAnimation()
        {
            float timer = 0f;
        
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);

            while (timer < spawnDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / spawnDuration;
                
                // Fade in by changing the alpha linearly
                float newAlpha = Mathf.Lerp(0f, 1f, progress);
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, newAlpha);

                yield return null;
            }

            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        }
    }
}
