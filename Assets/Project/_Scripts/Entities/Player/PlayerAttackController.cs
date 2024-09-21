using System.Collections.Generic;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Combat;
using Project._Scripts.Entities.Skills;
using UnityEngine;

namespace Project._Scripts.Entities.Player
{
    /// <summary>
    /// A 2D player attack controller.
    /// </summary>
    [RequireComponent(typeof(ActiveSkillManager))]
    public class PlayerAttackController : MonoBehaviour, IAttackController
    {
        public List<Skill> Skills => _activeSkillManager.Skills;

        private Camera _mainCamera;
        private Player _player;
        private ActiveSkillManager _activeSkillManager;
        private List<PassiveModifier<Projectile>> _modifiers = new();

        public void Start()
        {
            _activeSkillManager = GetComponent<ActiveSkillManager>();
            _mainCamera = Camera.main;
            _player = GetComponent<Player>();
        }

        public void Update()
        {
            TryAttack(0, false);
            TryAttack(1, true);
        }

        public void LearnSkill(Skill skill)
        {
            _activeSkillManager.LearnSkill(skill);
        }
        
        public void AddModifier(PassiveModifier<Projectile> modifier)
        {
            _modifiers.Add(modifier);
        }

        private void TryAttack(int key, bool isSecondary)
        {
            if (Input.GetMouseButtonDown(key))
            {
                _activeSkillManager.TryUseSkill(isSecondary, Shoot);
            }
        }

        public void Shoot(bool isSecondary, Skill skill)
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector3 direction = (mousePosition - transform.position).normalized;
            var startingPosition = transform.position;
            var damage = skill.Damage + _player.Stats.Attack;

            float projectileOffset = 1f;
            Vector3 spawnPosition = startingPosition + direction * projectileOffset;

            Projectile projectile = Instantiate(skill.AttackPrefab, spawnPosition, Quaternion.identity).GetComponent<Projectile>();
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            projectile.Setup(damage, "Player", DefaultPath);
            
            foreach (var modifier in _modifiers)
            {
                modifier.Modify(projectile);
            }
        }
        
        private static void DefaultPath(Projectile projectile, Rigidbody2D rigidbody)
        {
            rigidbody.velocity = projectile.transform.up * projectile.Speed;
        }
    }
}
