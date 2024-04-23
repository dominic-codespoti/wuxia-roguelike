using UnityEngine;

/// <summary>
/// A 2D enemy, which follows the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
class Enemy : MonoBehaviour, IDamageable
{
  public GameObject ExperienceOrbPrefab;
  public int TotalHealth = 10;
  public int Experience = 10;

  public int MaxHealth { get; private set; }
  public int CurrentHealth { get; private set; }

  public void Start()
  {
    MaxHealth = TotalHealth;
    CurrentHealth = MaxHealth;
  }

  public void TakeDamage(int damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth <= 0)
    {
      var orb = Instantiate(ExperienceOrbPrefab, transform.position, Quaternion.identity);
      orb.GetComponent<ExperienceOrb>().Experience = Experience;

      Destroy(gameObject);
    }
  }
}
