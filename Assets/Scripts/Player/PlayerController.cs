using UnityEngine;
using System.Collections;

/// <summary>
/// A 2D player controller.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 5.0f;
    [field: SerializeField] public float DashForce { get; private set; } = 10.0f;
    [field: SerializeField] public float AccelerationTime { get; private set; } = 0.2f;
    [field: SerializeField] public float DecelerationTime { get; private set; } = 0.1f;
    [field: SerializeField] public float FrictionCoefficient { get; private set; } = 0.5f;
    [field: SerializeField] public GameObject DashEffectPrefab { get; private set; }
    [field: SerializeField] public MovementState MovementState { get; private set; }
    [field: SerializeField] public Direction Direction { get; private set; }
    [field: SerializeField] public float DashDuration { get; private set; } = 0.2f;

    private Animator _animator;
    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Vector2 _targetVelocity;
    private float _velocityRatio;
    private Direction _previousDirection;
    private Color _originalColor;
    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private Vector2 _dashDirection;

    public void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _originalColor = _spriteRenderer.color;

        Direction = Direction.Down;
        MovementState = MovementState.Idle;
        _previousDirection = Direction.Down;
    }

    public void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var startDash = Input.GetKeyDown("space");

        Direction = GetDirection(horizontal, vertical);
        MovementState = (horizontal, vertical, startDash) switch
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
                HandleDash(startDash);
                break;
        }

        _previousDirection = GetDirection(horizontal, vertical);
    }

    public void BuffDashForce(float dashForce)
    {
        DashForce += dashForce;
    }

    private void HandleDash(bool startDash)
    {
        if (startDash)
        {
            _dashDirection = _body.velocity.normalized;
            StartCoroutine(HandleDashing(DashDuration));
        }
    }

    private IEnumerator HandleDashing(float dashDuration)
    {
        bool isDashing = true;
        float dashTimer = 0f;
        float remainingTime = dashDuration;
        Color targetColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0f);

        while (remainingTime > 0f)
        {
            if (isDashing)
            {
                dashTimer += Time.deltaTime;

                if (dashTimer < dashDuration)
                {
                    _body.AddForce(_dashDirection * DashForce, ForceMode2D.Force);
                }
                else
                {
                    isDashing = false;
                    _spriteRenderer.color = _originalColor;
                }
            }

            _spriteRenderer.color = Color.Lerp(_originalColor, targetColor, (dashDuration - remainingTime) / dashDuration);
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = _originalColor;
    }

    private Vector2 ApplyFriction(Vector2 velocity)
    {
        Vector2 frictionForce = -velocity.normalized * FrictionCoefficient;
        _body.AddForce(frictionForce, ForceMode2D.Force);

        return velocity;
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
