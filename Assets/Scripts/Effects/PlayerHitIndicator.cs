using System.Collections;
using Common;
using Common.Eventing;
using Entities.Player;
using UnityEngine;
using World.Systems;

namespace Effects
{
    /// <summary>
    /// A hit effect that flashes the sprite white when a projectile hits an enemy.
    /// </summary>
    public class PlayerHitIndicator : MonoBehaviour
    {
        [field: SerializeField] public float FlashDuration { get; private set; } = 0.5f;
        [field: SerializeField] public float FlashInterval { get; private set; } = 0.1f;
        [field: SerializeField] public Color FlashColor { get; private set; } = Color.white;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private static string PlayerId => GameState.Instance.Player.gameObject.Id();

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;

            EventBus.Subscribe<Events.EntityDamaged>(evt => DoEffect(evt.Target.GetComponent<Player>()), PlayerId);
        }

        private void DoEffect(Player player)
        {
            player.SetInvincible(true);
            StartCoroutine(Flash(player));
        }

        private IEnumerator Flash(Player player)
        {
            float elapsedTime = 0f;
            bool isFlashing = true;

            while (elapsedTime < FlashDuration)
            {
                _spriteRenderer.color = isFlashing ? new Color(FlashColor.r, FlashColor.g, FlashColor.b, 0.3f) : _originalColor;
                isFlashing = !isFlashing;
                elapsedTime += FlashInterval;
                yield return new WaitForSeconds(FlashInterval);
            }

            _spriteRenderer.color = _originalColor;
            player.SetInvincible(false);
        }
    }
}
