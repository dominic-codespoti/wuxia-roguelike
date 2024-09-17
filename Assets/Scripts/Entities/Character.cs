using Common.Interfaces;
using Entities.Player;
using UnityEngine;

namespace Entities
{
    public abstract class Character : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public int Level { get; protected set; } = 1;
        [field: SerializeField] public int Experience { get; protected set; } = 0;
        [field: SerializeField] public Stats Stats { get; protected set; }
        [field: SerializeField] public int CurrentHealth { get; set; }
        [field: SerializeField] public int CurrentMana { get; set; }
    }
}