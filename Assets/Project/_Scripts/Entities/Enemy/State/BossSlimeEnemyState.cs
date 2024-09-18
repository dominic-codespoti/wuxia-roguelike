using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Enemy.Bosses;
using Project._Scripts.World.Systems;
using UnityEngine;

namespace Project._Scripts.Entities.Enemy.State
{
    public class ChargeBossRoamState : IEnemyState
    {
        private float _stateTimer;
        private Vector3 _roamDirection;

        public void Enter(Enemy enemy) 
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            _stateTimer = boss.RoamDuration;
            _roamDirection = (boss.transform.position - GameState.Instance.Player.transform.position).normalized;
        }

        public void Execute(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            _stateTimer -= Time.deltaTime;
            boss.transform.position += _roamDirection * boss.RoamSpeed * Time.deltaTime;
        }

        public void Exit(Enemy enemy) { }

        public IEnemyState CheckTransitions(Enemy enemy)
        {
            if (_stateTimer <= 0)
            {
                return new ChargeBossWindupState();
            }
            return this;
        }
    }

    public class ChargeBossWindupState : IEnemyState
    {
        private float _stateTimer;

        public void Enter(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            _stateTimer = boss.ChargeWindupTime;
            boss.IsInvulnerable = true;
            boss.SpriteRenderer.color = Color.red;
        }

        public void Execute(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            _stateTimer -= Time.deltaTime;
            float squishFactor = Mathf.Sin(_stateTimer * boss.SquishFrequency) * boss.SquishAmplitude;
            boss.transform.localScale = new Vector3(boss.OriginalScale.x - squishFactor, boss.OriginalScale.y + squishFactor, boss.OriginalScale.z);
        }

        public void Exit(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            boss.transform.localScale = boss.OriginalScale;
        }

        public IEnemyState CheckTransitions(Enemy enemy)
        {
            if (_stateTimer <= 0)
            {
                var chargeDirection = (GameState.Instance.Player.transform.position - enemy.transform.position).normalized;
                return new ChargeBossChargeState(chargeDirection);
            }

            return this;
        }
    }

    
    public class ChargeBossChargeState : IEnemyState
    {
        private Vector2 _chargeDirection;
        private bool _hasHit;

        public ChargeBossChargeState(Vector2 chargeDirection)
        {
            _chargeDirection = chargeDirection.normalized;
        }

        public void Enter(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            boss.IsInvulnerable = true;
            _hasHit = false;
        }

        public void Execute(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;
            
            // Simple dash movement
            Vector3 newPosition = boss.transform.position + (Vector3)_chargeDirection * boss.ChargeSpeed * Time.deltaTime;
            
            // Check for collision with any collider
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(newPosition, boss.AttackRange);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject != boss.gameObject) // Ignore self-collision
                {
                    _hasHit = true;
                    if (collider.CompareTag("Player"))
                    {
                        if (collider.TryGetComponent(out IDamageable playerDamageable))
                        {
                            EventBus.Publish(new Events.EntityDamaged(_chargeDirection, boss.gameObject, boss.Stats.Attack), collider.gameObject.Id());
                        }
                    }
                    return; // Exit the method early if we've hit something
                }
            }
            
            // If we haven't hit anything, update the position
            boss.transform.position = newPosition;
        }

        public void Exit(Enemy enemy)
        {
            ChargeAttackBoss boss = enemy as ChargeAttackBoss;
            if (boss == null) return;

            boss.IsInvulnerable = false;
            boss.SpriteRenderer.color = Color.white;
        }

        public IEnemyState CheckTransitions(Enemy enemy)
        {
            if (_hasHit)
            {
                return new ChargeBossRoamState();
            }
            return this;
        }
    }
}