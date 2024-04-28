using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The player class, which contains all the player's data and functions.
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int Experience { get; private set; }

    public CulitvationStage CultivationStage { get; private set; }
    public CultivationRealm CultivationRealm { get; private set; }

    private Camera _camera;
    private PlayerAttackController _playerAttackController;
    private PlayerController _playerController;
    private PlayerPassiveSkillController _playerPassiveSkillController;

    public void Start()
    {
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

    public void LearnSkill(Skill skill)
    {
        _playerAttackController.LearnSkill(skill);
    }

    public void LearnPassiveSkill(PassiveSkill skill)
    {
        _playerPassiveSkillController.LearnSkill(skill);
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

    public void TakeDamage(int damage)
    {
        // Flash red and shake the camera
        var renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
        _camera.GetComponent<CameraShake>().Shake(0.1f, 0.1f);

        // Take damage
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void AdvanceCultivationStage()
    {
        CultivationStage++;
        Experience -= PlayerLevelStages.StageExperienceThresholds[CultivationStage];
        PlayerEvents.PlayerLeveledUp(this);
    }

    private void AdvanceCultivationRealm()
    {
        CultivationRealm++;
        CultivationStage = CulitvationStage.First;
        Experience -= PlayerLevelStages.RealmExperienceThresholds[CultivationRealm];
        PlayerEvents.PlayerLeveledUp(this);
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
