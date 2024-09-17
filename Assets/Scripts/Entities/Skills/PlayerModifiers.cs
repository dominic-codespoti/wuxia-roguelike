using System;
using Entities.Combat;

namespace Entities.Skills
{
    public static class PlayerModifiers
    {
        public static PassiveModifier For(Character character, PassiveSkill skill)
        {
            return skill.effectType switch
            {
                EffectType.BoostSpeed => new PassiveModifier(BoostSpeed, skill),
                EffectType.BoostAttack => new PassiveModifier(BoostAttack, skill),
                EffectType.BoostCritChance => new PassiveModifier(BoostCritChance, skill),
                _ => skill.With(() => { }),
            };
            
            void BoostSpeed(Unit? _)
            {
                character.Stats.Speed += skill.Value;
            }
            
            void BoostAttack(Unit? _)
            {
                character.Stats.Attack += skill.Value;
            }

            void BoostCritChance(Unit? _)
            {
                character.Stats.CriticalChance += skill.Value;
            }
        }
    }
}