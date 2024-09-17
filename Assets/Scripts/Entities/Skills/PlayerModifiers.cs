using System;
using Entities.Combat;

namespace Entities.Skills
{
    public static class PlayerModifiers
    {
        public static PassiveModifier For(Character character, PassiveSkill skill)
        {
            return (EffectorType: skill.effectorType, EffectType: skill.effectType) switch
            {
                _ => skill.With(() => { }),
            };
        }
    }
}