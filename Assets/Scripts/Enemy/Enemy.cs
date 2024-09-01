using Effects;
using Interfaces;
using UnityEngine;
using Utilities;

namespace Enemy
{
    /// <summary>
    /// A 2D enemy, which follows the player.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    class Enemy : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public GameObject ExperienceOrbPrefab { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int CurrentHealth { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Armor { get; private set; }

        private Player.Player _player;
        private Rigidbody2D _rigidbody;

        public void Start()
        {
            CurrentHealth = MaxHealth;

            gameObject.AddComponent<EnemyHitIndicator>();

            _rigidbody = GetComponent<Rigidbody2D>();
            _player = FindObjectOfType<Player.Player>();
        }

        public void Update()
        {
            var direction = _player.transform.position - transform.position;
            var velocity = direction.normalized * MovementSpeed;
            _rigidbody.velocity = velocity;
        }

        public void SetLevel(int level)
        {
            var randomExpMultiplier = Random.Range(2.4f, 5.6f);
            Experience = level * (int)randomExpMultiplier;
            MaxHealth = level * 10;
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage, Vector2 impact)
        {
            EventBus.Publish(new Events.DamageEvent(impact, gameObject, damage));
            EventBus.Publish(new Events.DamageEvent(impact, gameObject, damage), gameObject.Id());

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                var orb = Instantiate(ExperienceOrbPrefab, transform.position, Quaternion.identity);
                orb.GetComponent<ExperienceOrb>().SetExperience(Experience);

                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Vector2 impact = transform.position - collision.transform.position;
            damageable?.TakeDamage(Damage, impact);
        }
    }
}
