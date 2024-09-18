using UnityEngine;

namespace Project._Scripts.Entities.Combat
{
    [CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
    public class Skill : ScriptableObject
    {
        public string Name;
        public string Description;
        public Attack AttackPrefab;
        public GameObject EffectPrefab;
        public float Cooldown;
        public int Damage;
        public Sprite Image;
        public int ShotCount = 1;
        public SkillRarity Rarity;
    }
}

