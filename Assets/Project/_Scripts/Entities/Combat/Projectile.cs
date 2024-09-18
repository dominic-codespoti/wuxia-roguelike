using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using UnityEngine;

namespace Project._Scripts.Entities.Combat
{
    /// <summary>
    /// A 2D projectile that moves in a straight line.
    /// </summary>
    public class Projectile : Attack
    {
        private BoxCollider2D _box;
        private Rigidbody2D _rigidbody;

        public void Start()
        {
            Destroy(gameObject, Lifetime);

            _box = gameObject.AddComponent<BoxCollider2D>();
            _box.isTrigger = true;

            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.isKinematic = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public void Update()
        {
            _rigidbody.velocity = transform.up * Speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If the projectile hits the aggressor, ignore it
            if (collision.gameObject.CompareTag(Aggressor))
            {
                return;
            }

            // If the object hit is not hittable, ignore it
            if (!collision.gameObject.TryGetComponent(out IHittable hittable))
            {
                return;
            }

            // If the object hit is damageable, deal damage to it
            if (hittable is IDamageable damageable)
            {
                var impact = transform.position;
                EventBus.Publish(new Events.EntityDamaged(impact, collision.gameObject, Damage));
                EventBus.Publish(new Events.EntityDamaged(impact, collision.gameObject, Damage), collision.gameObject.Id());
            }

            Destroy(gameObject);
        }
    }
}
