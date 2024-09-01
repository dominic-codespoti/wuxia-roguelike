using System.Collections;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// A hit effect that flashes the sprite white when a projectile hits an enemy.
    /// </summary>
    public class PlayerHitIndicator : MonoBehaviour
    {
        [field: SerializeField] public float _flashDuration { get; } = 0.5f;
        [field: SerializeField] public float _flashInterval { get; } = 0.1f;
        [field: SerializeField] public Color _flashColor { get; } = Color.white;
        [field: SerializeField] public float KnockbackForce { get; private set; } = 10f;
        [field: SerializeField] public float KnockbackDuration { get; private set; } = 0.2f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private bool _isKnockbackActive;

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;

            EventBus.Subscribe<Events.PlayerDamaged>(evt => StartCoroutine(FlashCoroutine()));
        }

        private void DoEffect(GameObject target, Vector2 impact)
        {
            if (_isKnockbackActive)
            {
                return;
            }

            Vector2 direction = (Vector2)target.transform.position - impact;

            StartCoroutine(FlashCoroutine());
            StartCoroutine(ApplyKnockback(direction));
        }

        private IEnumerator ApplyKnockback(Vector2 direction)
        {
            _isKnockbackActive = true;
            float elapsedTime = 0f;

            while (elapsedTime < KnockbackDuration)
            {
                transform.position += (Vector3)(direction * KnockbackForce * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isKnockbackActive = false;
        }

        private IEnumerator FlashCoroutine()
        {
            float elapsedTime = 0f;
            bool isFlashing = true;

            while (elapsedTime < _flashDuration)
            {
                _spriteRenderer.color = isFlashing ? _flashColor : _originalColor;
                isFlashing = !isFlashing;
                elapsedTime += _flashInterval;
                yield return new WaitForSeconds(_flashInterval);
            }

            _spriteRenderer.color = _originalColor;
        }
    }
}
