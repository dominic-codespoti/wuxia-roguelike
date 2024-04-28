using UnityEngine;

/// <summary>
/// A 2D projectile that moves in a straight line.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 10f;
    [field: SerializeField] public float Lifetime { get; private set; } = 2f;
    [field: SerializeField] public OnHitEffect Effect { get; private set; } = OnHitEffect.None;
    [field: SerializeField] public float EffectValue { get; private set; } = 2;
    [field: SerializeField] public GameObject EffectPrefab { get; private set; }

    private int _damage;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void Start()
    {
        Destroy(gameObject, Lifetime);
    }

    public void Update()
    {
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        switch (Effect)
        {
            case OnHitEffect.None:
                damageable?.TakeDamage(_damage);
                Destroy(gameObject);
                break;
            case OnHitEffect.Pierce:
                damageable?.TakeDamage(_damage);
                break;
            case OnHitEffect.Explode:
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, EffectValue);
                foreach (Collider2D collider in colliders)
                {
                    IDamageable otherDamageable = collider.GetComponent<IDamageable>();
                    otherDamageable?.TakeDamage(_damage);
                }

                GameObject explosionEffect = Instantiate(EffectPrefab, transform.position, Quaternion.identity);
                Animator animator = explosionEffect.GetComponent<Animator>();
                animator.Play("Explosion");

                Destroy(gameObject);

                break;
        }
    }
}

public enum OnHitEffect
{
    None,
    Pierce,
    Explode
}
