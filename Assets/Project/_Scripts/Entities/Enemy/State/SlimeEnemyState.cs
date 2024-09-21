using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Enemy.State.Base;
using Project._Scripts.World.Systems;
using UnityEngine;

namespace Project._Scripts.Entities.Enemy.State
{
    public class RoamState : IEnemyState<Enemy>
    {
        private Vector2 _roamTarget;
        private float _roamTimer;

        public void Enter(Enemy enemy)
        {
            SetNewRoamTarget(enemy);
        }

        public void Execute(Enemy enemy)
        {
            if (_roamTimer <= 0)
            {
                SetNewRoamTarget(enemy);
            }
            else
            {
                _roamTimer -= Time.deltaTime;
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, _roamTarget, enemy.Stats.Speed * Time.deltaTime);
            }
        }

        public void Exit(Enemy enemy)
        {
            // Clean up any roam-specific things
        }

        public IEnemyState<Enemy> CheckTransitions(Enemy enemy)
        {
            if (enemy.DistanceToPlayer() < enemy.DetectionRange)
            {
                return new ChaseState();
            }

            return this;
        }

        private void SetNewRoamTarget(Enemy enemy)
        {
            _roamTarget = new Vector2(
                Random.Range(enemy.Room.position.x + 1, enemy.Room.position.x + enemy.Room.width - 1),
                Random.Range(enemy.Room.position.y + 1, enemy.Room.position.y + enemy.Room.height - 1)
            );
            
            _roamTimer = Random.Range(1f, 3f);
        }
    }

    public class ChaseState : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
        }

        public void Execute(Enemy enemy)
        {
            Vector2 directionToPlayer = enemy.DirectionToPlayer();
            enemy.GetComponent<Rigidbody2D>().velocity = directionToPlayer * enemy.Stats.Speed;
        }

        public void Exit(Enemy enemy)
        {
            
        }

        public IEnemyState<Enemy> CheckTransitions(Enemy enemy)
        {
            float distanceToPlayer = enemy.DistanceToPlayer();
            if (distanceToPlayer <= enemy.AttackRange)
            {
                return new AttackState();
            }
            
            return this;
        }
    }

    public class AttackState : IEnemyState<Enemy>
    {
        private bool _hasAttacked = false;
        private float _attackCooldown = 1f;
        private float _lastAttackTime = 0f;

        public void Enter(Enemy enemy)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
            _hasAttacked = false;
        }

        public void Execute(Enemy enemy)
        {
            if (!_hasAttacked && Time.time - _lastAttackTime >= _attackCooldown)
            {
                AttemptAttack(enemy);
            }
            else
            {
                Vector2 direction = enemy.DirectionToPlayer();
                enemy.GetComponent<Rigidbody2D>().velocity = direction * enemy.Stats.Speed;
            }
        }

        public void Exit(Enemy enemy)
        {
            
        }

        public IEnemyState<Enemy> CheckTransitions(Enemy enemy)
        {
            float distanceToPlayer = enemy.DistanceToPlayer();
            if (distanceToPlayer > enemy.AttackRange * 1.5f)
            {
                return new ChaseState();
            }

            return this;
        }

        private void AttemptAttack(Enemy enemy)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.AttackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.GetComponent<Player.Player>() != null)
                {
                    IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        Vector2 impact = enemy.transform.position - hitCollider.transform.position;
                        
                        var playerId = GameState.Instance.Player.gameObject.Id();
                        EventBus.Publish(new Events.EntityDamaged(impact, hitCollider.gameObject, enemy.Stats.Attack), playerId);

                        _hasAttacked = true;
                        _lastAttackTime = Time.time;
                        
                        break;
                    }
                }
            }
        }
    }
}