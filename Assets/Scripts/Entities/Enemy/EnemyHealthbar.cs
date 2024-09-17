using Common.Interfaces;
using UnityEngine;

namespace Entities.Enemy
{
  /// <summary>
  /// A 2D enemy health bar.
  /// </summary>
  class EnemyHealthbar : MonoBehaviour
  {
    [field: SerializeField] public GameObject HealthBarPrefab { get; private set; }
    private GameObject _healthBar;
    private Enemy _damageable;
    private Vector3 _initialFillScale;

    public void Start()
    {
      _healthBar = Instantiate(HealthBarPrefab, transform.position, Quaternion.identity);
      _healthBar.transform.SetParent(transform);
      _healthBar.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
      _damageable = GetComponent<Enemy>();

      _initialFillScale = _healthBar.transform.GetChild(0).localScale;
    }

    public void Update()
    {
      if (_damageable == null)
      {
        return;
      }

      float health = _damageable.CurrentHealth;
      float maxHealth = _damageable.Stats.health;

      if (maxHealth != 0)
      {
        float fillAmount = health / maxHealth;
        Transform fillTransform = _healthBar.transform.GetChild(0);

        Vector3 newScale = new Vector3(_initialFillScale.x * fillAmount, _initialFillScale.y, _initialFillScale.z);
        fillTransform.localScale = newScale;
      }
    }
  }
}
