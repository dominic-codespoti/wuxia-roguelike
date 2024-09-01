using System.Collections;
using UnityEngine;

namespace Systems
{
    public class WaveBasedEnemySpawner : MonoBehaviour
    {
        [field: SerializeField] public GameObject[] EnemyPrefabs { get; private set; } = { null };
        [field: SerializeField] public float SpawnChance { get; private set; } = 75f;
        [field: SerializeField] public float SpawnRadius { get; private set; } = 10f;
        [field: SerializeField] public float WaveDuration { get; private set; } = 30f;
        [field: SerializeField] public float WaveCooldown { get; private set; } = 5f;
        [field: SerializeField] public float WaveFrequencyIncreaseRate { get; private set; } = 0.2f;
        [field: SerializeField] public float WaveEnemyLevelIncreaseRate { get; private set; } = 0.5f;
        [field: SerializeField] public int StartingEnemyLevel { get; private set; } = 1;

        private int currentWave = 0;
        private float waveTimer = 0f;
        private float currentWaveSpawnRate = 1f;
        private int currentEnemyLevel;
        private float waveTimeRemaining;

        public void Start()
        {
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            while (true)
            {
                yield return new WaitForSeconds(waveTimer);

                currentWave++;
                currentWaveSpawnRate = CalculateWaveSpawnRate();
                currentEnemyLevel = CalculateEnemyLevel();
                waveTimeRemaining = WaveDuration;
                waveTimer = 0f;

                while (waveTimeRemaining > 0f)
                {
                    if (Random.Range(0, 100) < SpawnChance)
                    {
                        SpawnEnemy(currentEnemyLevel);
                    }

                    yield return new WaitForSeconds(currentWaveSpawnRate);
                    waveTimeRemaining -= currentWaveSpawnRate;
                }

                waveTimer = WaveCooldown;
            }
        }

        private float CalculateWaveSpawnRate()
        {
            return 1f / Mathf.Pow(currentWave * WaveFrequencyIncreaseRate + 1f, 2f);
        }

        private int CalculateEnemyLevel()
        {
            return StartingEnemyLevel + (int)(currentWave * WaveEnemyLevelIncreaseRate);
        }

        private void SpawnEnemy(int enemyLevel)
        {
            var enemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];
            var spawnPosition = transform.position + Random.insideUnitSphere * SpawnRadius;
            var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy.Enemy>().SetLevel(enemyLevel);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SpawnRadius);
        }
    }
}
