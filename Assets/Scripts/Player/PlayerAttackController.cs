using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A 2D player attack controller.
/// </summary>
public class PlayerAttackController : MonoBehaviour
{
  [Header("Default Projectile")]
  public GameObject DefaultProjectilePrefab;
  public GameObject DefaultEffectPrefab;
  public List<Skill> Skills;
  public BoxCollider2D Origin;

  private Skill _currentSkill;
  private float _currentCooldown;
  private Camera _mainCamera;

  private void Start()
  {
    _currentSkill = new Skill
    {
      name = "Default Attack",
      projectilePrefab = DefaultProjectilePrefab,
      cooldown = 0.5f,
      effectPrefab = DefaultEffectPrefab
    };

    _mainCamera = Camera.main;
  }

  private void Update()
  {
    // Handle skill selection (e.g., using number keys)
    for (int i = 0; i < Skills.Count; i++)
    {
      if (Input.GetKeyDown(KeyCode.Alpha1 + i))
      {
        SelectSkill(Skills[i]);
      }
    }

    // Decrement the cooldown timer
    if (_currentCooldown > 0)
    {
      _currentCooldown -= Time.deltaTime;
    }

    // Check if the player wants to attack and the current skill is not on cooldown
    if (Input.GetMouseButtonDown(0) && _currentCooldown <= 0)
    {
      ShootProjectile();
      _currentCooldown = _currentSkill.cooldown;
    }
  }

  private void SelectSkill(Skill skill)
  {
    _currentSkill = skill;
  }

  private void ShootProjectile()
  {
    // Get the mouse position in world coordinates
    Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f; // Set the z-coordinate to 0 for 2D

    // Calculate the direction from the mouse position to the player
    Vector3 direction = (mousePosition - transform.position).normalized;

    // Instantiate the projectile at the player's position, slightly ahead, and rotate it to face the cursor
    var startingPosition = Origin.bounds.center;
    var position = startingPosition + direction * 0.2f;
    GameObject projectile = Instantiate(_currentSkill.projectilePrefab, position, Quaternion.identity);
    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
  }
}

[System.Serializable]
public class Skill
{
  public string name;
  public GameObject projectilePrefab;
  public GameObject effectPrefab;
  public float cooldown;
}
