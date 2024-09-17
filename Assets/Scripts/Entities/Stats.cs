using System;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    [Serializable]
    public class Stats
    {
        public int health;
        public int mana;
        public int attack;
        public int speed;
        public int criticalChance;
    }
}