using Interfaces;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// A 2D projectile that moves in a straight line.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Projectile : Attack
    {
        private BoxCollider2D _box;

        public void Start()
        {
            Destroy(gameObject, Lifetime);

            _box = GetComponent<BoxCollider2D>();
            _box.isTrigger = true;
        }

        public void Update()
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Aggressor))
            {
                return;
            }

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Vector2 impact = transform.position;

            switch (Effect)
            {
                case OnHitEffect.None:
                    damageable?.TakeDamage(Damage, impact);
                    Destroy(gameObject);
                    break;
                case OnHitEffect.Pierce:
                    damageable?.TakeDamage(Damage, impact);
                    break;
                case OnHitEffect.Explode:
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, EffectValue);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collision.gameObject.CompareTag(Aggressor))
                        {
                            continue;
                        }

                        IDamageable otherDamageable = collider.GetComponent<IDamageable>();
                        otherDamageable?.TakeDamage(Damage, impact);
                    }

                    Destroy(gameObject);

                    GameObject explosionEffect = Instantiate(EffectPrefab, transform.position, Quaternion.identity);
                    Animator animator = explosionEffect.GetComponent<Animator>();
                    Destroy(explosionEffect, animator.GetCurrentAnimatorStateInfo(0).length);
                    break;
            }
        }
    }
}
