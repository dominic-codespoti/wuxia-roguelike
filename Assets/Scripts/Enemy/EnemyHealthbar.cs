using UnityEngine;

/// <summary>
/// A 2D enemy health bar.
/// </summary>
class EnemyHealthbar : MonoBehaviour
{
  public GameObject healthbarPrefab;
  private GameObject healthbar;
  private IDamageable damageable;
  private Vector3 initialFillScale;

  public void Start()
  {
    healthbar = Instantiate(healthbarPrefab, transform.position, Quaternion.identity);
    healthbar.transform.SetParent(transform);
    healthbar.transform.position = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
    damageable = GetComponent<IDamageable>();

    initialFillScale = healthbar.transform.GetChild(0).localScale;
  }

  public void Update()
  {
    if (damageable == null)
    {
      return;
    }

    float health = damageable.CurrentHealth;
    float maxHealth = damageable.MaxHealth;

    if (maxHealth != 0)
    {
      float fillAmount = health / maxHealth;
      Transform fillTransform = healthbar.transform.GetChild(0);

      Vector3 newScale = new Vector3(initialFillScale.x * fillAmount, initialFillScale.y, initialFillScale.z);
      fillTransform.localScale = newScale;
    }
  }
}
