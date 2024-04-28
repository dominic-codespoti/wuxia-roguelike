using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A 2D player attack controller.
/// </summary>
public class PlayerAttackController : MonoBehaviour
{
    [field: SerializeField] public List<Skill> Skills { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public BoxCollider2D Origin { get; private set; }

    private Skill _currentSkill;
    private float _currentCooldown;
    private Maybe<Skill> _currentSecondarySkill;
    private float _currentSecondaryCooldown;
    private Camera _mainCamera;
    private Player _player;
    private int _additionalShots;

    public void Start()
    {
        _mainCamera = Camera.main;
        _player = GetComponent<Player>();
        _currentSkill = Skills[0];
        _currentSecondarySkill = Maybe<Skill>.None;
        _additionalShots = 0;
    }

    public void Update()
    {
        // Main
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && _currentCooldown <= 0)
        {
            ShootProjectile();
            _currentCooldown = _currentSkill.Cooldown;
        }

        // Secondary
        if (_currentSecondaryCooldown > 0)
        {
            _currentSecondaryCooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1) && _currentSecondaryCooldown <= 0)
        {
            ShootProjectile(true);
            _currentSecondaryCooldown = _currentSecondarySkill.Map(skill => skill.Cooldown).Value;
        }
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

    private void ShootProjectile(bool isSecondary = false)
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;

        var skill = isSecondary ? _currentSecondarySkill.GetValueOrDefault(null) : _currentSkill;
        if (skill == null)
        {
            return;
        }

        var startingPosition = Origin.bounds.center;
        var position = startingPosition + direction * 0.2f;
        var shots = _additionalShots + skill.ShotCount;

        for (int i = 0; i <= shots; i++)
        {
            var pos = position + direction * i * 0.2f;
            Projectile projectile = Instantiate(skill.ProjectilePrefab, pos, Quaternion.identity);
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            projectile.SetDamage(skill.Damage + Attack);
        }
    }
}
