using System.Collections;
using System.Collections.Generic;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Entities.Enemy;
using Project._Scripts.Entities.Enemy.Bosses;
using UnityEngine;

namespace Project._Scripts.World.Systems
{
    public class RoomBasedEnemySpawner : Singleton<RoomBasedEnemySpawner>
    {
        [field: SerializeField] public GameObject[] EnemyPrefabs { get; private set; } = { null };
        [field: SerializeField] public GameObject[] BossPrefabs { get; private set; }
        [field: SerializeField] public float MinDistanceFromPlayer { get; private set; } = 3f;
        [field: SerializeField] public int MinEnemiesPerRoom { get; private set; } = 3;
        [field: SerializeField] public int MaxEnemiesPerRoom { get; private set; } = 7;

        private Dictionary<Room, List<GameObject>> _roomEnemies = new();
        private Transform _enemiesParent;

        protected override void Awake()
        {
            base.Awake();
            _enemiesParent = new GameObject("Enemies").transform;
            EventBus.Subscribe<Events.PlayerEnteredRoom>(OnPlayerEnteredRoom);
        }

        private void OnPlayerEnteredRoom(Events.PlayerEnteredRoom evt)
        {
            StartCoroutine(SpawnEnemiesForRoom(evt.RoomEntered));
        }

        private IEnumerator SpawnEnemiesForRoom(Room room)
        {
            // Wait for a frame to ensure the player has fully entered the room
            yield return null;

            if (_roomEnemies.TryGetValue(room, out var roomEnemy))
            {
                // Reactivate existing enemies
                foreach (var enemy in roomEnemy)
                {
                    if (enemy != null)
                    {
                        enemy.SetActive(true);
                    }
                }
            }
            else
            {
                // Spawn new enemies based on room type
                RoomType roomType = room.type;

                List<GameObject> enemies = new List<GameObject>();

                switch (roomType)
                {
                    case RoomType.Spawn:
                        // No enemies in spawn room
                        break;
                    case RoomType.Normal:
                        int enemyCount = Random.Range(MinEnemiesPerRoom, MaxEnemiesPerRoom + 1);
                        for (int i = 0; i < enemyCount; i++)
                        {
                            GameObject enemy = SpawnEnemy(room, false);
                            enemies.Add(enemy);
                        }
                        break;
                    case RoomType.Boss:
                        GameObject boss = SpawnEnemy(room, true);
                        if (boss != null)
                        {
                            enemies.Add(boss);
                        }
                        break;
                }

                _roomEnemies[room] = enemies;
            }
        }

        private GameObject SpawnEnemy(Room room, bool isBoss)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(room);
            if (spawnPosition == Vector3.zero)
            {
                return null;
            }

            GameObject enemyPrefab = isBoss 
                ? BossPrefabs[Random.Range(0, BossPrefabs.Length)]
                : EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, _enemiesParent);

            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.SetRoom(room);
            
            if (isBoss && enemyComponent is Boss boss)
            {
                var name = BossNamer.GetRandomBossName();
                enemyComponent.Configure(CalculateEnemyLevel(room, isBoss));
                EventBus.Publish(new Events.BossSpawned(enemyComponent, name.Name, name.Subtitle));
            }
            else
            {
                enemyComponent.Configure(CalculateEnemyLevel(room, isBoss));
            }

            return enemy;
        }

        private Vector3 GetValidSpawnPosition(Room room)
        {
            Vector3 spawnPosition;
            int attempts = 0;
            const int maxAttempts = 30;

            do
            {
                spawnPosition = new Vector3(
                    Random.Range(room.Position.x + 1, room.Position.x + room.width - 1),
                    Random.Range(room.Position.y + 1, room.Position.y + room.height - 1),
                    0f
                );
                attempts++;
            } while (Vector3.Distance(spawnPosition, GameState.Instance.Player.transform.position) < MinDistanceFromPlayer && attempts < maxAttempts);

            return attempts < maxAttempts ? spawnPosition : Vector3.zero;
        }

        private int CalculateEnemyLevel(Room room, bool isBoss)
        {
            int baseLevel = GameState.Instance.Player.Level;
            return isBoss ? baseLevel + 5 : baseLevel;
        }

        public void OnDrawGizmos()
        {
            if (GameState.Instance?.CurrentRoom == null) return;

            Room currentRoom = GameState.Instance.CurrentRoom;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(currentRoom.Center, new Vector3(currentRoom.width, currentRoom.height, 0f));

            if (GameState.Instance.Player != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(GameState.Instance.Player.transform.position, MinDistanceFromPlayer);
            }
        }
    }
}