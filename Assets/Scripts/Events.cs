using Combat;
using UnityEngine;

public class Events
{
    public class PlayerLeveledUp : Event
    {
        public Player.Player Player { get; }

        public PlayerLeveledUp(Player.Player player)
        {
            Player = player;
        }
    }

    public class SkillLearned : Event
    {
        public Skill Skill { get; }

        public SkillLearned(Skill skill)
        {
            Skill = skill;
        }
    }

    public class PassiveSkillLearned : Event
    {
        public PassiveSkill PassiveSkill { get; }

        public PassiveSkillLearned(PassiveSkill passiveSkill)
        {
            PassiveSkill = passiveSkill;
        }
    }

    public class PlayerDamaged : Event
    {
        public Player.Player Player { get; }
        public int Damage { get; }
        public Vector2 Impact { get; }

        public PlayerDamaged(Player.Player player, int damage, Vector2 impact)
        {
            Player = player;
            Damage = damage;
            Impact = impact;
        }
    }

    public class PlayerDied : Event
    {
        public Player.Player Player { get; }
        public PlayerDied(Player.Player player)
        {
            Player = player;
        }
    }

    public class MenuOpened : Event
    {
        public MenuOpened()
        {
        }
    }

    public class MenuClosed : Event
    {
        public MenuClosed()
        {
        }
    }

    public class DamageEvent : Event
    {
        public Vector2 Impact;
        public GameObject Target;
        public int Damage;

        public DamageEvent(Vector2 impact, GameObject target, int damage)
        {
            Impact = impact;
            Target = target;
            Damage = damage;
        }
    }
}
