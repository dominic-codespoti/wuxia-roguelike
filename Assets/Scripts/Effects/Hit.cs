using UnityEngine;

/// <summary>
/// A hit effect that flashes the sprite white when a projectile hits an enemy.
/// </summary>
public class Hit : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    public void Play()
    {
        StartCoroutine(DoHitEffect());
    }

    private System.Collections.IEnumerator DoHitEffect()
    {
        Color targetColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);
        _spriteRenderer.color = targetColor;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _originalColor;
    }
}
