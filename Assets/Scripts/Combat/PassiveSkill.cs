using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Passive Skill")]
public class PassiveSkill : ScriptableObject
{
    public string Name;
    public string Description;
    public EffectorType EffectorType;
    public EffectType EffectType;
    public Sprite Image;
    public int Rank;
    public List<int> Values;
    public int Value => Values.ElementAtOrDefault(Rank);
    public SkillRarity Rarity;
}

public enum EffectorType
{
    Dash,
    Attack,
    Health,
}

public enum EffectType
{
    Increase,
    Decrease,
    Multiply,
    IncreaseCount,
}
