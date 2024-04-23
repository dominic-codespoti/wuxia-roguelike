using UnityEngine;

/// <summary>
/// Spawns enemies within a given area.
/// </summary>
class EnemySpawner : MonoBehaviour
{
  public GameObject[] enemyPrefabs;

  [Range(0, 100)]
  public float spawnRate = 1;

  [Range(0, 100)]
  public float spawnChance = 50;

  [Range(0, 100)]
  public float spawnRadius = 5;

  public void Start()
  {
    InvokeRepeating("SpawnEnemy", 0, spawnRate);
  }

  public void SpawnEnemy()
  {
    if (Random.Range(0, 100) < spawnChance)
    {
      var enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
      var spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
      Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, spawnRadius);
  }
}
