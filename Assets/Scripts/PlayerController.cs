using UnityEngine;

/// <summary>
/// A 2D player controller.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
class PlayerController : MonoBehaviour
{
  [Range(0.0f, 10.0f)]
  public float speed = 5.0f;

  [Range(0.0f, 10.0f)]
  public float dashForce = 7.0f;

  public MovementState movementState;
  public Direction direction;

  private Animator animator;
  private Rigidbody2D body;
  private SpriteRenderer spriteRenderer;
  private new BoxCollider2D collider;

  public void Start()
  {
    body = GetComponent<Rigidbody2D>();
    body.gravityScale = 0;

    spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.sortingOrder = 1;

    collider = GetComponent<BoxCollider2D>();
    animator = GetComponent<Animator>();

    direction = Direction.Down;
    movementState = MovementState.Idle;
  }

  public void Update()
  {
    var horizontal = Input.GetAxis("Horizontal");
    var vertical = Input.GetAxis("Vertical");
    var isDashing = Input.GetButtonDown("Jump");

    direction = GetDirection(horizontal, vertical);
    movementState = (horizontal, vertical, isDashing) switch
    {
      (0, 0, false) => MovementState.Idle,
      (_, _, true) => MovementState.Dashing,
      _ => MovementState.Walking
    };

    switch (movementState)
    {
      case MovementState.Idle:
        body.velocity = Vector2.zero;
        SetAnimatorParameters("Idle");
        break;
      case MovementState.Walking:
        body.velocity = new Vector2(horizontal, vertical) * speed;
        SetAnimatorParameters("Walking", direction);
        break;
      case MovementState.Dashing:
        body.AddForce(new Vector2(horizontal, vertical) * dashForce, ForceMode2D.Impulse);
        break;
    }
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
    animator.SetBool("IsIdle", state == "Idle");
    animator.SetBool("IsWalkingLeft", direction == Direction.Left && state == "Walking");
    animator.SetBool("IsWalkingRight", direction == Direction.Right && state == "Walking");
    animator.SetBool("IsWalkingUp", direction == Direction.Up && state == "Walking");
    animator.SetBool("IsWalkingDown", direction == Direction.Down && state == "Walking");

    if (direction == Direction.Left)
    {
      spriteRenderer.flipX = true;
    }
    else if (direction == Direction.Right)
    {
      spriteRenderer.flipX = false;
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
