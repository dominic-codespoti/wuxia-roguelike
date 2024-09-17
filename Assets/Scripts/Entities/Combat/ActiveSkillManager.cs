using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Entities.Combat
{
    public class ActiveSkillManager : MonoBehaviour
    {
        [field: SerializeField] public List<Skill> Skills { get; private set; }
        [field: SerializeField] public Skill CurrentSkill { get; private set; }
        [field: SerializeField] public Maybe<Skill> CurrentSecondarySkill { get; private set; }
        
        private float _currentCooldown;
        private float _currentSecondaryCooldown;
        
        public void Start()
        {
            CurrentSecondarySkill = Maybe<Skill>.None;
            CurrentSkill = Skills[0];
        }
        
        public void Update()
        {
            _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
            _currentSecondaryCooldown = Mathf.Max(0, _currentSecondaryCooldown - Time.deltaTime);
        }
        
        public void LearnSkill(Skill skill)
        {
            Skills.Add(skill);
            CurrentSecondarySkill = skill;
        }

        public void TryUseSkill(bool isSecondary, Action<bool, Skill> handle)
        {
            var skill = CanUseSkill(isSecondary);
            if (skill.HasValue)
            {
                handle(isSecondary, skill.Value);
            }
        }
        
        private Maybe<Skill> CanUseSkill(bool isSecondary)
        {
            var cooldown = isSecondary ? _currentSecondaryCooldown : _currentCooldown;
            if (cooldown <= 0)
            {
                return isSecondary ? CurrentSecondarySkill : CurrentSkill;
            }
            
            return Maybe<Skill>.None;
        }
    }
}