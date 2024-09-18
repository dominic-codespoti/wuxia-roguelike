using UnityEngine;

namespace Project._Scripts.Entities.Combat
{
    /// <summary>
    /// A generic 2D attack.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class Attack : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; } = 10f;
        [field: SerializeField] public float Lifetime { get; private set; } = 2f;
        [field: SerializeField] public OnHitEffect Effect { get; private set; } = OnHitEffect.None;
        [field: SerializeField] public float EffectValue { get; private set; } = 2;
        [field: SerializeField] public GameObject EffectPrefab { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public bool IsCrit { get; private set; }
        [field: SerializeField] public string Aggressor { get; private set; }

        public void Setup(int damage, string aggressor, bool isCrit)
        {
            Damage = damage;
            IsCrit = isCrit;
            Aggressor = aggressor;
        }
    }
}
