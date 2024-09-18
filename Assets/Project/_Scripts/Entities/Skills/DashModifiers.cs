using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Combat;

namespace Project._Scripts.Entities.Skills
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