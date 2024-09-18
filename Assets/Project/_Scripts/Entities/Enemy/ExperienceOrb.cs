using Project._Scripts.Common.Eventing;
using UnityEngine;

namespace Project._Scripts.Entities.Enemy
{
    /// <summary>
    /// A class for experience orbs that enemies drop when they die.
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    class ExperienceOrb : MonoBehaviour
    {
        [field: SerializeField] public int Experience { get; private set; } = 10;

        private CircleCollider2D _circle;

        public void Start()
        {
            float size = Random.Range(0.2f, 0.5f);
            transform.localScale = new Vector3(size, size, 1);

            _circle = GetComponent<CircleCollider2D>();
            _circle.isTrigger = true;
        }

        public void SetExperience(int experience)
        {
            Experience = experience;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventBus.Publish(new Events.PlayerGainedExperience(Experience));
                Destroy(gameObject);
            }
        }
    }
}
