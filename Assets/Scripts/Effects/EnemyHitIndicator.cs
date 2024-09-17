using System.Collections;
using Common;
using Common.Eventing;
using UnityEngine;

namespace Effects
{
    /// <summary>
    ///  A hit effect that flashes the sprite white when an enemy hits the player.
    /// </summary>
    public class EnemyHitIndicator : MonoBehaviour
    {
        [field: SerializeField] public float FlashDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float FlashInterval { get; private set; } = 0.1f;
        [field: SerializeField] public Color FlashColor { get; private set; } = Color.white;

        private SpriteRenderer _spriteRenderer;

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            EventBus.Subscribe<Events.EntityDamaged>(evt => DoEffect(evt.Target), gameObject.Id());
        }

        private void DoEffect(GameObject target)
        {
            StartCoroutine(Flash());
            StartCoroutine(Squish(target));
        }

        private IEnumerator Squish(GameObject target)
        {
            var originalScale = target.transform.localScale;
            transform.localScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = originalScale;
        }

        private IEnumerator Flash()
        {
            var originalColor = _spriteRenderer.color;
            float elapsedTime = 0f;
            bool isFlashing = true;

            while (elapsedTime < FlashDuration)
            {
                _spriteRenderer.color = isFlashing ? new Color(FlashColor.r, FlashColor.g, FlashColor.b, 0.3f) : originalColor;
                isFlashing = !isFlashing;
                elapsedTime += FlashInterval;
                yield return new WaitForSeconds(FlashInterval);
            }

            _spriteRenderer.color = originalColor;
        }
    }
}
