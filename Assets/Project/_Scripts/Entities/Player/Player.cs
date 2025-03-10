using System.Collections.Generic;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Entities.Combat;
using Project._Scripts.Entities.Skills;
using UnityEngine;

namespace Project._Scripts.Entities.Player
{
    /// <summary>
    /// The player class, which contains all the player's data and functions.
    /// </summary>
    public class Player : Character
    {
        private bool _isInvincible;
        private List<PassiveSkill> _passiveSkills = new();
        private PlayerAttackController _playerAttackController;
        private PlayerController _playerController;
        private List<PassiveModifier> _modifiers = new();

        public void Start()
        {
            EventBus.Subscribe<Events.SkillLearned>((evt) => LearnSkill(evt.Skill));
            EventBus.Subscribe<Events.PassiveSkillLearned>((evt) => LearnPassiveSkill(evt.PassiveSkill));
            EventBus.Subscribe<Events.PlayerGainedExperience>((evt) => GainExperience(evt.Experience));
            EventBus.Subscribe<Events.EntityDamaged>((evt) => TakeHit(evt.Damage, evt.Impact), gameObject.Id());

            _playerAttackController = GetComponent<PlayerAttackController>();
            _playerController = GetComponent<PlayerController>();
        }
        
        public void SetInvincible(bool isInvincible)
        {
            _isInvincible = isInvincible;
        }
        
        public void GainExperience(int experience)
        {
            Experience += experience;
            if (PlayerLevelTable.GetExperienceNeededForLevel(Level) <= Experience)
            {
                LevelUp();
            }
        }

        public Maybe<Skill> GetSkill(string name)
        {
            var skill = _playerAttackController.Skills.Find(skill => skill.Name == name);
            return skill != null ? skill : Maybe<Skill>.None;
        }

        public Maybe<PassiveSkill> GetPassiveSkill(string name)
        {
            var skill = _passiveSkills.Find(skill => skill.name == name);
            return skill != null ? skill : Maybe<PassiveSkill>.None;
        }
        
        public void AddModifier(PassiveModifier modifier)
        {
            _modifiers.Add(modifier);
            modifier.Modify();
        }

        private void TakeHit(int damage, Vector2 impact)
        {
            if (_isInvincible)
            {
                return;
            }
            
            EventBus.Publish(new Events.PlayerTookDamage(this, damage, impact));

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
            _passiveSkills.Add(skill);
            switch (skill.effectorType)
            {
                case EffectorType.Attack:
                    var attackMod = AttackModifiers.For(_playerAttackController, skill);
                    _playerAttackController.AddModifier(attackMod);
                    break;
                case EffectorType.Dash:
                    var dashMod = DashModifiers.For(_playerController, skill);
                    _playerController.AddDashModifier(dashMod);
                    break;
                case EffectorType.Player:
                    var playerMod = PlayerModifiers.For(this, skill);
                    AddModifier(playerMod);
                    break;
            }
        }

        private void LevelUp()
        {
            Experience = 0;
            CurrentHealth = Stats.Health;
            CurrentMana = Stats.Mana;
            Level += 1;

            EventBus.Publish(new Events.PlayerLeveledUp(this));
        }
    }
}
