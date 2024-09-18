using System;
using Project._Scripts.Entities.Combat;

namespace Project._Scripts.Entities.Skills
{
    public class PassiveModifier<T>
    {
        public Action<T> Modifier;
        public PassiveSkill Skill;

        public PassiveModifier(Action<T> modifier, PassiveSkill skill)
        {
            Modifier = modifier;
            Skill = skill;
        }
        
        public virtual void Modify(T target)
        {
            Modifier(target);
        }
    }
    
    public class PassiveModifier : PassiveModifier<Unit?>
    {
        public PassiveModifier(Action<Unit?> modifier, PassiveSkill skill) : base(modifier, skill)
        {
        }

        public override void Modify(Unit? target = null)
        {
            Modifier(Unit.Empty);
        }
    }

    public struct Unit
    {
        public static Unit Empty => new Unit();
    }
}