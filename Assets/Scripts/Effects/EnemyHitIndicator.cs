using System.Collections;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class EnemyHitIndicator : MonoBehaviour
    {
        [field: SerializeField] public float KnockbackForce { get; private set; } = 10f;
        [field: SerializeField] public float KnockbackDuration { get; private set; } = 0.2f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Vector2 _originalPosition;
        private bool _isKnockbackActive;

        public void Start()
        {
            _originalPosition = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            EventBus.Subscribe<Events.DamageEvent>(evt => DoEffect(evt.Target, evt.Impact), gameObject.Id());
        }

        private void DoEffect(GameObject target, Vector2 impact)
        {
            if (_isKnockbackActive)
            {
                return;
            }

            Vector2 direction = (Vector2)target.transform.position - impact;

            StartCoroutine(ApplyKnockback(direction));
            StartCoroutine(FlashColor());
            StartCoroutine(Squish());
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

        private IEnumerator Squish()
        {
            transform.localScale = new Vector3(1.2f, 0.8f, 1);
            yield return new WaitForSeconds(0.1f);
            transform.localScale = new Vector3(1, 1, 1);
        }

        private IEnumerator FlashColor()
        {
            Color targetColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);
            _spriteRenderer.color = targetColor;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _originalColor;
        }
    }
}
