using Common;
using Common.Eventing;
using Common.Interfaces;
using Entities.Enemy.State;
using UnityEngine;
using World;
using World.Systems;

namespace Entities.Enemy
{
    /// <summary>
    /// A 2D enemy, which follows the player.
    /// </summary>
    public class Enemy : Character
    {
        [field: SerializeField] public GameObject ExperienceOrbPrefab { get; private set; }
        [field: SerializeField] public float DetectionRange { get; private set; } = 15f;
        [field: SerializeField] public float AttackRange { get; private set; } = 2f;
        [field: SerializeField] public Room Room { get; private set; }

        protected bool IsBoss;
        protected EnemyStateMachine StateMachine;

        public void Start()
        {
            CurrentHealth = Stats.Health;
            
            EventBus.Subscribe<Events.EntityDamaged>((evt) => TakeHit(evt.Damage, evt.Impact), gameObject.Id());
        }

        public void Update()
        {
            StateMachine.Update();
        }

        public virtual void Configure(int level)
        {
            StateMachine = new EnemyStateMachine(this, new RoamState());
            var randomExpMultiplier = Random.Range(2.4f, 5.6f);
            Experience = level * (int)randomExpMultiplier;
            Stats.Health = level * 10;
            CurrentHealth = Stats.Health;
        }
        
        public void SetRoom(Room room)
        {
            Room = room;
        }

        private void TakeHit(int damage, Vector2 impact)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                var orb = Instantiate(ExperienceOrbPrefab, transform.position, Quaternion.identity);
                orb.GetComponent<ExperienceOrb>().SetExperience(Experience * 3);

                Destroy(gameObject);
                
                if (IsBoss)
                {
                    EventBus.Publish(new Events.BossDied(this));
                }
            }
        }
        
        public float DistanceToPlayer()
        {
            if (GameState.Instance.CurrentRoom != Room)
            {
                return float.MaxValue;
            }

            return Vector2.Distance(transform.position, GameState.Instance.Player.transform.position);
        }

        public Vector2 DirectionToPlayer()
        {
            return (GameState.Instance.Player.transform.position - transform.position).normalized;
        }
    }
}
