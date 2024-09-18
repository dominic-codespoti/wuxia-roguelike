using Project._Scripts.Common.Interfaces;
using UnityEngine;

namespace Project._Scripts.Entities
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public int Level { get; protected set; } = 1;
        [field: SerializeField] public int Experience { get; protected set; } = 0;
        [field: SerializeField] public Stats Stats { get; protected set; }
        [field: SerializeField] public int CurrentHealth { get; protected set; }
        [field: SerializeField] public int CurrentMana { get; protected set; }
    }
}