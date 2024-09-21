using Project._Scripts.Entities.Enemy.State;
using Project._Scripts.Entities.Enemy.State.Base;
using UnityEngine;

namespace Project._Scripts.Entities.Enemy.Bosses
{
    public class ChargeAttackBoss : Boss
    {
        [SerializeField] private float roamSpeed = 5f;
        [SerializeField] private float roamDuration = 3f;
        [SerializeField] private float chargeWindupTime = 2f;
        [SerializeField] private float chargeSpeed = 50f;
        [SerializeField] private float chargeDuration = 0.5f;
        [SerializeField] private float squishFrequency = 10f;
        [SerializeField] private float squishAmplitude = 0.2f;

        public Vector3 OriginalScale { get; private set; }
        public bool IsInvulnerable { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Color OriginalColor { get; private set; }

        public float RoamSpeed => roamSpeed;
        public float RoamDuration => roamDuration;
        public float ChargeWindupTime => chargeWindupTime;
        public float ChargeSpeed => chargeSpeed;
        public float ChargeDuration => chargeDuration;
        public float SquishFrequency => squishFrequency;
        public float SquishAmplitude => squishAmplitude;
        public new EnemyStateMachine<ChargeAttackBoss> StateMachine { get; private set; }

        public override void Configure(int level)
        {
            base.Configure(level);

            IsBoss = true;
            StateMachine = new EnemyStateMachine<ChargeAttackBoss>(this, new ChargeBossRoamState());
            
            SpriteRenderer = GetComponent<SpriteRenderer>();
            OriginalScale = transform.localScale;
            OriginalColor = SpriteRenderer.color;
        }

        public override void ChangeState()
        {
            StateMachine.Update();
        }
        
        public void ToggleInvulnerability(bool state)
        {
            IsInvulnerable = state;
        }
        
        protected override void TakeHit(int damage, Vector2 impact)
        {
            if (!IsInvulnerable)
            {
                base.TakeHit(damage, impact);
            }
        }
    }
}