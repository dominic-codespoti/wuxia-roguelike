using Effects;
using UnityEngine;

namespace Systems
{
    public class DamageTextGenerator : MonoBehaviour
    {
        [field: SerializeField] public GameObject DamageTextPrefab { get; private set; }

        private void OnEnable()
        {
            EventBus.Subscribe<Events.DamageEvent>(evt => OnDamageTaken(evt));
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<Events.DamageEvent>();
        }

        private void OnDamageTaken(Events.DamageEvent damageEvent)
        {
            Vector3 position = damageEvent.Target.transform.position;
            position.y += 0.1f;
            GameObject damageTextObject = Instantiate(DamageTextPrefab, position, Quaternion.identity);
            damageTextObject.transform.SetParent(damageEvent.Target.transform);
            DamageText damageText = damageTextObject.GetComponent<DamageText>();
            damageText.SetText(damageEvent.Damage.ToString());
        }
    }
}
