using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Combat
{
    [CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Passive Skill")]
    public class PassiveSkill : ScriptableObject
    {
        public string name;
        public string description;
        public EffectorType effectorType;
        public EffectType effectType;
        public Sprite image;
        public int rank;
        public List<int> values;
        public int Value => values.ElementAtOrDefault(rank);
        public SkillRarity rarity;
        
        public PassiveModifier<T> With<T>(Action<T> action)
        {
            return new PassiveModifier<T>(action, this);
        }
        
        public PassiveModifier With(Action action)
        {
            return new PassiveModifier((_) => action(), this);
        }
    }

    public enum EffectorType
    {
        Dash,
        Attack,
        Player,
    }

    public enum EffectType
    {
        IncreaseCount,
        BoostSpeed,
        BoostAttack,
        BoostHealth,
        BoostCritChance,
    }
}