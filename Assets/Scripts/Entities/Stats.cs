using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
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