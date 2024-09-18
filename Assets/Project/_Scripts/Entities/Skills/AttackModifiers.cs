using System.Collections.Generic;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Combat;
using UnityEngine;

// Include UnityEngine for GameObject and Instantiate

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
                var shots = skill.Value;
                var offset = 0.5f;

                List<Projectile> newProjectiles = new List<Projectile>();
                for (var i = 0; i < shots; i++)
                {
                    // Calculate offset position based on the index
                    Vector3 spawnPosition = projectile.transform.position + new Vector3(i * offset, 0, 0);
                    Quaternion spawnRotation = projectile.transform.rotation;

                    // Instantiate the new projectile
                    GameObject projectilePrefab = projectile.gameObject; // Assuming projectile is a MonoBehaviour with a prefab
                    GameObject newProjectileGO = GameObject.Instantiate(projectilePrefab, spawnPosition, spawnRotation);

                    // Optionally, get the Projectile component if needed
                    Projectile newProjectile = newProjectileGO.GetComponent<Projectile>();
                    newProjectiles.Add(newProjectile);
                }
            }
        }
    }
}