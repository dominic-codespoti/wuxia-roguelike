using System;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using UnityEngine;

namespace Project._Scripts.Entities.Combat
{
    /// <summary>
    /// A 2D projectile that moves in a straight line.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: SerializeField] public float Lifetime { get; private set; } = 2f;
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public string Aggressor { get; private set; }
        [field: SerializeField] public ParticleSystem HitEffect { get; private set; }
        [field: SerializeField] public Action<Projectile, Rigidbody2D> MovementAction { get; private set; }

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
            MovementAction(this, _rigidbody);
        }

        public void Setup(int damage, string aggressor, Action<Projectile, Rigidbody2D> movementAction)
        {
            Damage = damage;
            Aggressor = aggressor;
            MovementAction = movementAction;
        }
        
        public Projectile Clone()
        {
            GameObject newGameObject = Instantiate(gameObject);
            Projectile newProjectile = newGameObject.GetComponent<Projectile>();

            // Copy all serializable fields
            newProjectile.Speed = Speed;
            newProjectile.Lifetime = Lifetime;
            newProjectile.Damage = Damage;
            newProjectile.Aggressor = Aggressor;
            newProjectile.HitEffect = HitEffect;

            // Copy the movement action
            newProjectile.MovementAction = MovementAction;

            return newProjectile;
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

            var pos = transform.position + new Vector3(0, 0, -1);
            var rot = Quaternion.identity;
            var effect = Instantiate(HitEffect, pos, rot);
            effect.Play();

            Destroy(gameObject);
        }
    }
}
