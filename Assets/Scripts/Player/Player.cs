using Combat;
using Interfaces;
using UnityEngine;
using Utilities;

namespace Player
{
    /// <summary>
    /// The player class, which contains all the player's data and functions.
    /// </summary>
    public class Player : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int CurrentHealth { get; private set; }
        [field: SerializeField] public int Experience { get; private set; }
        [field: SerializeField] public CulitvationStage CultivationStage { get; private set; }
        [field: SerializeField] public CultivationRealm CultivationRealm { get; private set; }

        private Camera _camera;
        private PlayerAttackController _playerAttackController;
        private PlayerController _playerController;
        private PlayerPassiveSkillController _playerPassiveSkillController;

        public void Start()
        {
            EventBus.Subscribe<Events.SkillLearned>((evt) => LearnSkill(evt.Skill));
            EventBus.Subscribe<Events.PassiveSkillLearned>((evt) => LearnPassiveSkill(evt.PassiveSkill));

            CultivationStage = CulitvationStage.First;
            CultivationRealm = CultivationRealm.BodyRefinement;
            Experience = 0;
            CurrentHealth = MaxHealth;

            _camera = Camera.main;
            _playerController = GetComponent<PlayerController>();
            _playerAttackController = GetComponent<PlayerAttackController>();
            _playerPassiveSkillController = GetComponent<PlayerPassiveSkillController>();
        }

        public void BuffHealth(int health)
        {
            CurrentHealth += health;
            MaxHealth += health;
        }

        public void GainExperience(int amount)
        {
            Experience += amount;

            while (true)
            {
                if (Experience >= PlayerLevelStages.StageExperienceThresholds[CultivationStage])
                {
                    if (CultivationStage == CulitvationStage.Third)
                    {
                        if (CultivationRealm < CultivationRealm.ImmortalAscension && Experience >= PlayerLevelStages.RealmExperienceThresholds[CultivationRealm])
                        {
                            AdvanceCultivationRealm();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        AdvanceCultivationStage();
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public Maybe<Skill> GetSkill(string name)
        {
            var skill = _playerAttackController.Skills.Find(skill => skill.Name == name);
            return skill != null ? skill : Maybe<Skill>.None;
        }

        public Maybe<PassiveSkill> GetPassiveSkill(string name)
        {
            var skill = _playerPassiveSkillController.Skills.Find(skill => skill.Name == name);
            return skill != null ? skill : Maybe<PassiveSkill>.None;
        }

        public void TakeDamage(int damage, Vector2 impact)
        {
            EventBus.Publish(new Events.PlayerDamaged(this, damage, impact));

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                EventBus.Publish(new Events.PlayerDied(this));
            }
        }

        private void LearnSkill(Skill skill)
        {
            _playerAttackController.LearnSkill(skill);
        }

        private void LearnPassiveSkill(PassiveSkill skill)
        {
            _playerPassiveSkillController.LearnSkill(skill);
        }

        private void AdvanceCultivationStage()
        {
            CultivationStage++;
            Experience = 0;
            MaxHealth += 2;
            CurrentHealth = MaxHealth;

            EventBus.Publish(new Events.PlayerLeveledUp(this));
        }

        private void AdvanceCultivationRealm()
        {
            CultivationRealm++;
            CultivationStage = CulitvationStage.First;
            Experience = 0;
            MaxHealth += 10;
            CurrentHealth = MaxHealth;

            EventBus.Publish(new Events.PlayerLeveledUp(this));
        }
    }
}
