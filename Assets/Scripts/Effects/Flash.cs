using UnityEngine;
using System.Collections;

/// <summary>
/// A hit effect that flashes the sprite white when a projectile hits an enemy.
/// </summary>
public class FlashEffect : MonoBehaviour
{
    [SerializeField] private float _flashDuration = 0.5f;
    [SerializeField] private float _flashInterval = 0.1f;
    [SerializeField] private Color _flashColor = Color.white;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        StartCoroutine(FlashCoroutine());
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
