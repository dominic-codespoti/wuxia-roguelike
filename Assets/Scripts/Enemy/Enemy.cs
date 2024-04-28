using UnityEngine;

/// <summary>
/// A 2D enemy, which follows the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Hit))]
class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public GameObject ExperienceOrbPrefab { get; private set; }
    [field: SerializeField] public int Experience { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int MovementSpeed { get; private set; }

    private Player _player;
    private Rigidbody2D _rigidbody;
    private Hit _hitEffect;

    public void Start()
    {
        CurrentHealth = MaxHealth;

        _rigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
        _hitEffect = GetComponent<Hit>();
    }

    public void Update()
    {
        var direction = _player.transform.position - transform.position;
        var velocity = direction.normalized * MovementSpeed;
        _rigidbody.velocity = velocity;
    }

    public void SetLevel(int level)
    {
        Experience = level * 10;
        MaxHealth = level * 10;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        _hitEffect.Play();

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            var orb = Instantiate(ExperienceOrbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<ExperienceOrb>().SetExperience(Experience);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(1);
    }
}
