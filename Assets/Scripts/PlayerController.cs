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
  }

  public void Update()
  {
    var horizontal = Input.GetAxis("Horizontal");
    var vertical = Input.GetAxis("Vertical");
    var isDashing = Input.GetButtonDown("Jump");

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
        break;
      case MovementState.Walking:
        body.velocity = new Vector2(horizontal, vertical) * speed;
        break;
      case MovementState.Dashing:
        body.AddForce(new Vector2(horizontal, vertical) * dashForce, ForceMode2D.Impulse);
        break;
    }
  }

  public void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log($"Collision with {collision.gameObject.name}");
    if (collision.gameObject.CompareTag("Wall"))
    {
      body.velocity = Vector2.zero;
    }
  }
}

enum MovementState
{
  Idle,
  Walking,
  Dashing
}

