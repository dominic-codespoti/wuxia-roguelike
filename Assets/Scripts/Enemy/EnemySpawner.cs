using UnityEngine;

/// <summary>
/// Spawns enemies within a given area.
/// </summary>
class EnemySpawner : MonoBehaviour
{
    [field: SerializeField] public GameObject[] EnemyPrefabs { get; private set; }

    [field: Range(0, 100)]
    [field: SerializeField] public float SpawnRate { get; private set; } = 1;

    [field: Range(0, 100)]
    [field: SerializeField] public float SpawnChance { get; private set; } = 50;

    [field: Range(0, 100)]
    [field: SerializeField] public float SpawnRadius { get; private set; } = 5;
    [field: SerializeField] public float FrequencyIncreaseRate { get; private set; } = 0.1f;
    [field: SerializeField] public float FrequencyMultiplier { get; private set; } = 1.1f;
    [field: SerializeField] public float EnemyLevelIncreaseRate { get; private set; } = 0.2f;
    [field: SerializeField] public int StartingEnemyLevel { get; private set; } = 1;

    private float elapsedTime = 0f;

    public void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0, SpawnRate);
    }

    public void SpawnEnemy()
    {
        elapsedTime += Time.deltaTime;

        float currentSpawnRate = SpawnRate / Mathf.Pow(FrequencyMultiplier, elapsedTime * FrequencyIncreaseRate);

        if (Random.Range(0, 100) < SpawnChance)
        {
            var enemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];
            var spawnPosition = transform.position + Random.insideUnitSphere * SpawnRadius;

            int enemyLevel = (int)(StartingEnemyLevel + elapsedTime * EnemyLevelIncreaseRate);

            var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetLevel(enemyLevel);
        }

        Invoke(nameof(SpawnEnemy), currentSpawnRate);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }
}
