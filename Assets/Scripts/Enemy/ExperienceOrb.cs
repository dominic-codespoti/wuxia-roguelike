using UnityEngine;

/// <summary>
/// A class for experience orbs that enemies drop when they die.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
class ExperienceOrb : MonoBehaviour
{
  [field: SerializeField] public int Experience { get; private set; } = 10;

  public void SetExperience(int experience)
  {
    Experience = experience;
  }

  public void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      other.gameObject.GetComponent<Player>().GainExperience(Experience);
      Destroy(gameObject);
    }
  }
}
