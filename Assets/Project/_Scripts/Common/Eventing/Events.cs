using System.Collections.Generic;
using Project._Scripts.Entities.Combat;
using Project._Scripts.Entities.Enemy;
using Project._Scripts.Entities.Player;
using Project._Scripts.World;
using UnityEngine;

namespace Project._Scripts.Common.Eventing
{
    public class Events
    {
        public class PlayerLeveledUp : Event
        {
            public Player Player { get; }

            public PlayerLeveledUp(Player player)
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

        public class PlayerDied : Event
        {
            public Player Player { get; }
            public PlayerDied(Player player)
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

        public class EntityDamaged : Event
        {
            public Vector2 Impact;
            public GameObject Target;
            public int Damage;

            public EntityDamaged(Vector2 impact, GameObject target, int damage)
            {
                Impact = impact;
                Target = target;
                Damage = damage;
            }
        }

        public class PlayerEnteredRoom : Event
        {
            public Room RoomEntered { get; }

            public PlayerEnteredRoom(Room roomEntered)
            {
                RoomEntered = roomEntered;
            }
        }
        
        public class PlayerExitedRoom : Event
        {
            public Room RoomLeft { get; }

            public PlayerExitedRoom(Room roomLeft)
            {
                RoomLeft = roomLeft;
            }
        }
        
        public class FadeStarted : Event
        {
            public FadeStarted()
            {
            }
        }
        
        public class FadeEnded : Event
        {
            public FadeEnded()
            {
            }
        }
        
        public class FadeAdjusted : Event
        {
            public Color Color { get; }
            
            public FadeAdjusted(Color color)
            {
                Color = color;
            }
        }
        
        public class BossSpawned : Event
        {
            public Enemy Enemy { get; }
            public string Title { get; }
            public string Subtitle { get; }

            public BossSpawned(Enemy enemy, string title, string subtitle)
            {
                Enemy = enemy;
                Title = title;
                Subtitle = subtitle;
            }
        }
        
        public class BossDied : Event
        {
            public Enemy Enemy { get; }

            public BossDied(Enemy enemy)
            {
                Enemy = enemy;
            }
        }

        public class LevelGenerated : Event
        {
            public List<Room> Rooms { get; }

            public LevelGenerated(List<Room> rooms)
            {
                Rooms = rooms;
            }
        }

        public class PlayerGainedExperience : Event
        {
            public int Experience { get; }

            public PlayerGainedExperience(int experience)
            {
                Experience = experience;
            }
        }
    }
}
