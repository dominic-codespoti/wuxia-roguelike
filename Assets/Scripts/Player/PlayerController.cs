using UnityEngine;

/// <summary>
/// A 2D player controller.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
class PlayerController : MonoBehaviour
{
  [Range(0.0f, 10.0f)]
  public float Speed = 5.0f;

  [Range(0.0f, 10.0f)]
  public float DashForce = 7.0f;

  [Range(0.0f, 1.0f)]
  public float AccelerationTime = 0.2f;

  [Range(0.0f, 1.0f)]
  public float DecelerationTime = 0.1f;

  [Range(0.0f, 1.0f)]
  public float FrictionCoefficient = 0.5f;

  public GameObject DashEffectPrefab;
  public MovementState MovementState;
  public Direction Direction;

  private Animator _animator;
  private Rigidbody2D _body;
  private SpriteRenderer _spriteRenderer;
  private BoxCollider2D _boxCollider;
  private Vector2 _targetVelocity;
  private float _velocityRatio;
  private Direction _previousDirection;

  public void Start()
  {
    _body = GetComponent<Rigidbody2D>();
    _body.gravityScale = 0;
    _body.freezeRotation = true;

    _spriteRenderer = GetComponent<SpriteRenderer>();

    _boxCollider = GetComponent<BoxCollider2D>();

    _animator = GetComponent<Animator>();

    Direction = Direction.Down;
    MovementState = MovementState.Idle;
    _previousDirection = Direction.Down;
  }

  public void Update()
  {
    var horizontal = Input.GetAxis("Horizontal");
    var vertical = Input.GetAxis("Vertical");
    var isDashing = Input.GetKeyDown("space");

    Direction = GetDirection(horizontal, vertical);
    MovementState = (horizontal, vertical, isDashing) switch
    {
      (0, 0, false) => MovementState.Idle,
        (_, _, true) => MovementState.Dashing,
        _ => MovementState.Walking
    };

    _targetVelocity = new Vector2(horizontal, vertical) * Speed;
    _velocityRatio = (_body.velocity.magnitude + _targetVelocity.magnitude) / (_body.velocity.magnitude + Speed);
    _body.velocity = ApplyFriction(_body.velocity);

    if (Direction != _previousDirection)
    {
        _body.velocity = Vector2.zero;
    }

    switch (MovementState)
    {
      case MovementState.Idle:
        _body.velocity = Vector2.Lerp(_body.velocity, Vector2.zero, DecelerationTime);
        SetAnimatorParameters("Idle");
        break;
      case MovementState.Walking:
        _body.velocity = Vector2.Lerp(_body.velocity, _targetVelocity, AccelerationTime * _velocityRatio);
        SetAnimatorParameters("Walking", Direction);
        break;
      case MovementState.Dashing:
        _body.AddForce(_targetVelocity.normalized * DashForce, ForceMode2D.Impulse);
        break;
    }

    _previousDirection = GetDirection(horizontal, vertical);
  }

  private Direction GetDirection(float horizontal, float vertical)
  {
    if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
    {
      return horizontal > 0 ? Direction.Right : Direction.Left;
    }
    else
    {
      return vertical > 0 ? Direction.Up : Direction.Down;
    }
  }

  private void SetAnimatorParameters(string state, Direction? direction = null)
  {
    _animator.SetBool("IsIdle", state == "Idle");
    _animator.SetBool("IsWalkingLeft", direction == Direction.Left && state == "Walking");
    _animator.SetBool("IsWalkingRight", direction == Direction.Right && state == "Walking");
    _animator.SetBool("IsWalkingUp", direction == Direction.Up && state == "Walking");
    _animator.SetBool("IsWalkingDown", direction == Direction.Down && state == "Walking");

    if (direction == Direction.Left)
    {
      _spriteRenderer.flipX = true;
    }
    else if (direction == Direction.Right)
    {
      _spriteRenderer.flipX = false;
    }
  }

  private Vector2 ApplyFriction(Vector2 velocity)
  {
    Vector2 frictionForce = -velocity.normalized * FrictionCoefficient;
    _body.AddForce(frictionForce, ForceMode2D.Force);

    return velocity;
  }
}

enum MovementState
{
  Idle,
  Walking,
  Dashing
}

enum Direction
{
  Up,
  Down,
  Left,
  Right
}
