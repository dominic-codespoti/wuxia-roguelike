using UnityEngine;

/// <summary>
/// The player class, which contains all the player's data and functions.
/// </summary>
class Player : MonoBehaviour, IDamageable
{
  public int Level;
  public int Experience;
  public int TotalHealth = 100;

  public int MaxHealth { get; private set; }
  public int CurrentHealth { get; private set; }

  public void Start()
  {
    Level = 1;
    Experience = 0;
    MaxHealth = TotalHealth;
    CurrentHealth = MaxHealth;
  }

  public void GainExperience(int amount)
  {
    Experience += amount;
    if (Experience >= 100)
    {
      LevelUp();
    }
  }

  public void TakeDamage(int damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth <= 0)
    {
      Die();
    }
  }

  private void LevelUp()
  {
    Level++;
    Experience = 0;
    MaxHealth += 10;
    CurrentHealth = MaxHealth;
  }

  private void Die()
  {
    Debug.Log("Player died");
  }
}
