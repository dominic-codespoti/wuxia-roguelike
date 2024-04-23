using UnityEngine;

/// <summary>
/// A 2D projectile that moves in a straight line.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
  public float speed = 10f;
  public float lifetime = 2f;

  private void Start()
  {
    Destroy(gameObject, lifetime);
  }

  private void Update()
  {
    transform.Translate(Vector3.up * speed * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Player"))
    {
      return;
    }

    IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
    if (damageable != null)
    {
      damageable.TakeDamage(1);
    }

    Destroy(gameObject);
  }
}
