using UnityEngine;
using Entities.Enemy.State;
using World.Systems;

namespace Entities.Enemy.Bosses
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
        public bool IsInvulnerable { get; set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public float RoamSpeed => roamSpeed;
        public float RoamDuration => roamDuration;
        public float ChargeWindupTime => chargeWindupTime;
        public float ChargeSpeed => chargeSpeed;
        public float ChargeDuration => chargeDuration;
        public float SquishFrequency => squishFrequency;
        public float SquishAmplitude => squishAmplitude;

        public override void Configure(int level)
        {
            base.Configure(level);

            IsBoss = true;
            StateMachine = new EnemyStateMachine(this, new ChargeBossRoamState());

            OriginalScale = transform.localScale;
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeHit(int damage, Vector2 impact)
        {
            if (!IsInvulnerable)
            {
                TakeHit(damage, impact);
            }
        }
    }
}