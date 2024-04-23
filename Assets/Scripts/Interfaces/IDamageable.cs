/// <summary>
/// Interface for objects that can take damage.
/// </summary>
internal interface IDamageable
{
  /// <summary>
  /// The amount of damage the object can take.
  /// </summary>
  int MaxHealth { get; }

  /// <summary>
  /// The current health of the object.
  /// </summary>
  int CurrentHealth { get; }

  /// <summary>
  /// The object takes damage.
  /// </summary>
  void TakeDamage(int damage);
}
