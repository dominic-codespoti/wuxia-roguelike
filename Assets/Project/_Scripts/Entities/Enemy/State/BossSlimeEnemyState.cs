using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Enemy.Bosses;
using Project._Scripts.Entities.Enemy.State.Base;
using Project._Scripts.World.Systems;
using UnityEngine;

namespace Project._Scripts.Entities.Enemy.State
{
    public class ChargeBossRoamState : IEnemyState<ChargeAttackBoss>
    {
        private float _stateTimer;
        private Vector3 _roamDirection;

        public void Enter(ChargeAttackBoss boss) 
        {
            _stateTimer = boss.RoamDuration;
            _roamDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }

        public void Execute(ChargeAttackBoss boss)
        {
            _stateTimer -= Time.deltaTime;
            boss.transform.position += _roamDirection * boss.RoamSpeed * Time.deltaTime;
        }

        public void Exit(ChargeAttackBoss boss)
        {
            
        }

        public IEnemyState<ChargeAttackBoss> CheckTransitions(ChargeAttackBoss boss)
        {
            if (_stateTimer <= 0)
            {
                return new ChargeBossWindupState();
            }

            return this;
        }
    }

    public class ChargeBossWindupState : IEnemyState<ChargeAttackBoss>
    {
        private float _stateTimer;

        public void Enter(ChargeAttackBoss boss)
        {
            _stateTimer = boss.ChargeWindupTime;
            boss.ToggleInvulnerability(true);
            boss.SpriteRenderer.color = Color.red;
        }

        public void Execute(ChargeAttackBoss boss)
        {
            _stateTimer -= Time.deltaTime;
            float squishFactor = Mathf.Sin(_stateTimer * boss.SquishFrequency) * boss.SquishAmplitude;
            boss.transform.localScale = new Vector3(boss.OriginalScale.x - squishFactor, boss.OriginalScale.y + squishFactor, boss.OriginalScale.z);
        }

        public void Exit(ChargeAttackBoss boss)
        {
            boss.transform.localScale = boss.OriginalScale;
            boss.SpriteRenderer.color = boss.OriginalColor;
        }

        public IEnemyState<ChargeAttackBoss> CheckTransitions(ChargeAttackBoss boss)
        {
            if (_stateTimer <= 0)
            {
                return new ChargeBossChargeState();
            }

            return this;
        }
    }
    
    public class ChargeBossChargeState : IEnemyState<ChargeAttackBoss>
    {
        private bool _hasHit;
        private Vector3 _chargeDirection;

        public ChargeBossChargeState()
        {
            
        }

        public void Enter(ChargeAttackBoss boss)
        {
            _chargeDirection = (GameState.Instance.Player.transform.position - boss.transform.position).normalized;
            boss.ToggleInvulnerability(false);
            _hasHit = false;
        }

        public void Execute(ChargeAttackBoss boss)
        {
            // Simple dash movement
            Vector3 newPosition = boss.transform.position + _chargeDirection * boss.ChargeSpeed * Time.deltaTime;
            
            // Check for collision with any collider
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(newPosition, boss.AttackRange);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject != boss.gameObject && collider.TryGetComponent<IDamageable>(out _))
                {
                    _hasHit = true;
                    if (collider.TryGetComponent<Player.Player>(out var player))
                    {
                        var playerId = GameState.Instance.Player.gameObject.Id();
                        EventBus.Publish(new Events.EntityDamaged(_chargeDirection, player.gameObject, boss.Stats.Attack), playerId);
                    }
                    return;
                }
            }
            
            // If we haven't hit anything, update the position
            boss.transform.position = newPosition;
        }

        public void Exit(ChargeAttackBoss boss)
        {
            boss.SpriteRenderer.color = boss.OriginalColor;
            boss.transform.localScale = boss.OriginalScale;
        }

        public IEnemyState<ChargeAttackBoss> CheckTransitions(ChargeAttackBoss boss)
        {
            if (_hasHit)
            {
                return new ChargeBossRoamState();
            }

            return this;
        }
    }
}