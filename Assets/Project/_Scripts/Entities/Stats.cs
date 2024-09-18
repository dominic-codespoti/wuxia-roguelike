using System;
using UnityEngine;

namespace Project._Scripts.Entities
{
    [Serializable]
    public class Stats
    {
        [field: SerializeField] public int Health { get; set; }
        [field: SerializeField] public int Mana { get; set; }
        [field: SerializeField] public int Attack { get; set; }
        [field: SerializeField] public int Speed { get; set; }
        [field: SerializeField] public int CriticalChance { get; set; }
    }
}