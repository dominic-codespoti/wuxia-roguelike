using System;
using Common.Interfaces;
using Entities.Combat;

namespace Entities.Skills
{
    public static class DashModifiers
    {
        public static PassiveModifier For(IMovementController movementController, PassiveSkill skill)
        {
            return (EffectorType: skill.effectorType, EffectType: skill.effectType) switch
            {
                _ => skill.With(() => { }),
            };
        }
    }
}