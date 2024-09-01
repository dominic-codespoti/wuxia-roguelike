using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerPassiveSkillController : MonoBehaviour
    {
        [SerializeField] public List<PassiveSkill> Skills;

        private Player _player;
        private PlayerAttackController _playerAttackController;
        private PlayerController _playerController;

        public void Start()
        {
            _player = GetComponent<Player>();
            _playerAttackController = GetComponent<PlayerAttackController>();
            _playerController = GetComponent<PlayerController>();
        }

        public void LearnSkill(PassiveSkill skill)
        {
            Skills.Add(skill);

            Apply(skill);
        }

        private void Apply(PassiveSkill skill)
        {
            switch (skill.EffectorType)
            {
                case EffectorType.Dash:
                    ApplyDash(skill);
                    break;
                case EffectorType.Attack:
                    ApplyAttack(skill);
                    break;
                case EffectorType.Health:
                    ApplyHealth(skill);
                    break;
            }
        }

        private void ApplyHealth(PassiveSkill skill)
        {
            switch (skill.EffectType)
            {
                case EffectType.Increase:
                    _player.BuffHealth((int)skill.Value);
                    break;
                case EffectType.Decrease:
                    _player.BuffHealth((int)-skill.Value);
                    break;
                case EffectType.Multiply:
                    _player.BuffHealth((int)(skill.Value * _player.MaxHealth));
                    break;
            }
        }

        private void ApplyAttack(PassiveSkill skill)
        {
            switch (skill.EffectType)
            {
                case EffectType.Increase:
                    _playerAttackController.BuffAttack((int)skill.Value);
                    break;
                case EffectType.Decrease:
                    _playerAttackController.BuffAttack((int)-skill.Value);
                    break;
                case EffectType.Multiply:
                    _playerAttackController.BuffAttack((int)(skill.Value * _playerAttackController.Attack));
                    break;
                case EffectType.IncreaseCount:
                    _playerAttackController.AddProjectileToAttacks();
                    break;
                case EffectType.CriticalChance:
                    _playerAttackController.BuffCriticalChance(skill.Value);
                    break;
            }
        }

        private void ApplyDash(PassiveSkill skill)
        {
            switch (skill.EffectType)
            {
                case EffectType.Increase:
                    _playerController.BuffDashForce((int)skill.Value);
                    break;
                case EffectType.Decrease:
                    _playerController.BuffDashForce((int)-skill.Value);
                    break;
                case EffectType.Multiply:
                    _playerController.BuffDashForce((int)(skill.Value * _playerController.DashForce));
                    break;
            }
        }
    }
}
