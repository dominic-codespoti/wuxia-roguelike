using Common;
using Common.Eventing;
using Effects;
using UnityEngine;

namespace World.Systems
{
    public class DamageTextGenerator : Singleton<DamageTextGenerator>
    {
        public GameObject damageTextPrefab;
        public float positionOffsetRange = 0.5f; // Range for random horizontal offset
        public float scaleVariance = 0.1f; // Variance in text size (scale)

        private void Start()
        {
            EventBus.Subscribe<Events.EntityDamaged>(evt => OnDamageTaken(evt));
        }

        private void OnDamageTaken(Events.EntityDamaged entityDamaged)
        {
            Vector3 position = entityDamaged.Target.transform.position;
            SpriteRenderer spriteRenderer = entityDamaged.Target.GetComponent<SpriteRenderer>();
            
            // Offset vertically above the target's sprite
            position.y += spriteRenderer.bounds.size.y / 2;

            // Add a random horizontal offset
            float randomOffset = Random.Range(-positionOffsetRange, positionOffsetRange);
            position.x += randomOffset;

            // Instantiate damage text prefab
            GameObject damageTextObject = Instantiate(damageTextPrefab, position, Quaternion.identity);
            damageTextObject.transform.SetParent(entityDamaged.Target.transform);

            // Adjust text size with a small random scale variance
            float randomScaleFactor = 1 + Random.Range(-scaleVariance, scaleVariance);
            damageTextObject.transform.localScale *= randomScaleFactor;

            // Set the text for the damage amount
            DamageText damageText = damageTextObject.GetComponent<DamageText>();
            damageText.SetText(entityDamaged.Damage.ToString());
        }
    }
}