using System.Collections.Generic;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Combat;
using UnityEngine;

namespace Project._Scripts.Entities.Skills
{
    public static class AttackModifiers
    {
        public static PassiveModifier<Projectile> For(IAttackController attackController, PassiveSkill skill)
        {
            return skill.effectType switch
            {
                EffectType.IncreaseCount => new PassiveModifier<Projectile>(IncreaseCount, skill),
                _ => skill.With<Projectile>(_ => { }),
            };
            
            void IncreaseCount(Projectile projectile)
            {
                var shots = skill.Value + 1;
                var offset = 0.5f;

                List<Projectile> newProjectiles = new List<Projectile>();
                
                for (var i = 0; i < shots; i++)
                {
                    float xOffset = (i - (shots - 1) / 2f) * offset;
                    Vector3 spawnPosition = projectile.transform.position + new Vector3(xOffset, 0, 0);

                    Projectile newProjectile = projectile.Clone();
                    newProjectile.transform.position = spawnPosition;
                    newProjectile.transform.rotation = projectile.transform.rotation;

                    newProjectiles.Add(newProjectile);
                }

                Object.Destroy(projectile.gameObject);
            }
        }
    }
}