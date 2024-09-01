using System.Collections.Generic;
using Combat;
using UnityEngine;
using Utilities;

namespace Player
{
    /// <summary>
    /// A 2D player attack controller.
    /// </summary>
    public class PlayerAttackController : MonoBehaviour
    {
        [field: SerializeField] public List<Skill> Skills { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }

        private Skill _currentSkill;
        private float _currentCooldown;
        private Maybe<Skill> _currentSecondarySkill;
        private float _currentSecondaryCooldown;
        private Camera _mainCamera;
        private Player _player;
        private int _additionalShots;
        private float _vampirePercentage;
        private float _criticalChancePercentage;
        private BoxCollider2D _origin;

        public void Start()
        {
            _mainCamera = Camera.main;
            _player = GetComponent<Player>();
            _currentSkill = Skills[0];
            _currentSecondarySkill = Maybe<Skill>.None;
            _additionalShots = 0;
            _vampirePercentage = 0f;
            _criticalChancePercentage = 0f;
            _origin = GetComponent<BoxCollider2D>();
        }

        public void Update()
        {
            HandleInput(0, _currentSkill, ref _currentCooldown, false);
            HandleInput(1, _currentSecondarySkill, ref _currentSecondaryCooldown, true);
        }

        public void BuffCriticalChance(float percentage)
        {
            _criticalChancePercentage += percentage;
        }

        public void BuffVampire(float percentage)
        {
            _vampirePercentage += percentage;
        }

        public void BuffAttack(int attack)
        {
            Attack += attack;
        }

        public void AddProjectileToAttacks()
        {
            _additionalShots++;
        }

        public void LearnSkill(Skill skill)
        {
            Skills.Add(skill);
            _currentSecondarySkill = skill;
        }

        private void HandleInput(int key, Skill skill, ref float cooldown, bool isSecondary)
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(key) && cooldown <= 0)
            {
                HandleAttack(isSecondary);
                cooldown = skill.Cooldown;
            }
        }

        private void HandleAttack(bool isSecondary = false)
        {
            var skill = isSecondary ? _currentSecondarySkill.GetValueOrDefault(null) : _currentSkill;
            if (skill == null)
            {
                return;
            }

            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Vector3 direction = (mousePosition - transform.position).normalized;
            var startingPosition = _origin.bounds.center;
            var shots = _additionalShots + skill.ShotCount;

            float spacing = 0.1f;
            float halfShots = shots / 2f;

            for (int i = 1; i <= shots; i++)
            {
                var isCrit = Random.value < _criticalChancePercentage;

                var damage = skill.Damage + Attack;
                if (isCrit)
                {
                    damage *= (int) 1.4;
                }

                float offset = (i - halfShots) * spacing;
                Vector3 spawnPosition = startingPosition + (Vector3)Vector3.Cross(direction, Vector3.forward) * offset;

                if (skill.AttackPrefab is Melee m)
                {
                    Melee melee = Instantiate(skill.AttackPrefab, spawnPosition, Quaternion.identity).GetComponent<Melee>();
                    melee.Setup(damage, "Player", isCrit);
                    melee.Swing(transform.position, mousePosition);
                } 
                else if (skill.AttackPrefab is Projectile p)
                {
                    Projectile projectile = Instantiate(skill.AttackPrefab, spawnPosition, Quaternion.identity).GetComponent<Projectile>();
                    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    projectile.Setup(damage, "Player", isCrit);
                }
            }
        }
    }
}
