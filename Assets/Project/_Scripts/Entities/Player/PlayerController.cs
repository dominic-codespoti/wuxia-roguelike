using System.Collections;
using System.Collections.Generic;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Common.Interfaces;
using Project._Scripts.Entities.Skills;
using UnityEngine;

namespace Project._Scripts.Entities.Player
{
    /// <summary>
    /// A 2D player controller.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour, IMovementController
    {
        [field: SerializeField] public float Speed { get; private set; } = 5.0f;
        [field: SerializeField] public float DashForce { get; private set; } = 10.0f;
        [field: SerializeField] public float AccelerationTime { get; private set; } = 0.2f;
        [field: SerializeField] public float DecelerationTime { get; private set; } = 0.1f;
        [field: SerializeField] public float FrictionCoefficient { get; private set; } = 0.5f;
        [field: SerializeField] public GameObject DashEffectPrefab { get; private set; }
        [field: SerializeField] public MovementState MovementState { get; private set; }
        [field: SerializeField] public MovementDirection MovementDirection { get; private set; }
        [field: SerializeField] public float DashDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float DashCooldown { get; private set; } = 1.0f;

        private Animator _animator;
        private Rigidbody2D _body;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider;
        private Vector2 _targetVelocity;
        private float _velocityRatio;
        private MovementDirection _previousMovementDirection;
        private Color _originalColor;
        private Vector2 _dashDirection;
        private float _dashCooldownTimer;
        private List<PassiveModifier> _dashModifiers = new();
        
        public void AddDashModifier(PassiveModifier modifier)
        {
            _dashModifiers.Add(modifier);
        }

        public void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _originalColor = _spriteRenderer.color;

            MovementDirection = MovementDirection.Down;
            MovementState = MovementState.Idle;
            _previousMovementDirection = MovementDirection.Down;

            EventBus.Subscribe<Events.EntityDamaged>(evt => ReceiveImpact(evt.Impact), gameObject.Id());
        }

        public void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var startDash = Input.GetKeyDown("space");

            MovementDirection = GetDirection(horizontal, vertical);
            MovementState = (horizontal, vertical, startDash) switch
            {
                (0, 0, false) => MovementState.Idle,
                (_, _, true) => MovementState.Dashing,
                _ => MovementState.Walking
            };

            _targetVelocity = new Vector2(horizontal, vertical) * Speed;
            _velocityRatio = (_body.velocity.magnitude + _targetVelocity.magnitude) / (_body.velocity.magnitude + Speed);
            _body.velocity = ApplyFriction(_body.velocity);

            if (MovementDirection != _previousMovementDirection)
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
                    SetAnimatorParameters("Walking", MovementDirection);
                    break;
                case MovementState.Dashing:
                    HandleDash(startDash);
                    break;
            }

            _previousMovementDirection = GetDirection(horizontal, vertical);
            _dashCooldownTimer -= Time.deltaTime;
        }
        
        public void ReceiveImpact(Vector2 impact)
        {
            float width = _boxCollider.size.x;
            float height = _boxCollider.size.y;

            Vector2 impactDirection = -impact.normalized;

            Vector2 knockbackDistance = new Vector2(
                impactDirection.x * width,
                impactDirection.y * height
            );

            StartCoroutine(ApplyKnockback(knockbackDistance, 0.1f));
        }
        
        private IEnumerator ApplyKnockback(Vector2 knockbackDistance, float knockbackDuration)
        {
            Vector2 startPosition = _body.position;
            Vector2 targetPosition = startPosition + knockbackDistance;
            float elapsedTime = 0f;

            MovementState = MovementState.Idle;

            while (elapsedTime < knockbackDuration)
            {
                elapsedTime += Time.deltaTime;

                _body.MovePosition(Vector2.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration));

                yield return null;
            }

            _body.MovePosition(targetPosition);

            MovementState = MovementState.Walking;
        }

        private void HandleDash(bool startDash)
        {
            if (startDash && _dashCooldownTimer <= 0f)
            {
                _dashDirection = _body.velocity.normalized;
                StartCoroutine(HandleDashing(DashDuration));
                _dashCooldownTimer = DashCooldown;
            }
        }

        private IEnumerator HandleDashing(float dashDuration)
        {
            foreach (var modifier in _dashModifiers)
            {
                modifier.Modify();
            }

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
                        _body.velocity = _dashDirection * DashForce;                    }
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

        private MovementDirection GetDirection(float horizontal, float vertical)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                return horizontal > 0 ? MovementDirection.Right : MovementDirection.Left;
            }
            else
            {
                return vertical > 0 ? MovementDirection.Up : MovementDirection.Down;
            }
        }

        private void SetAnimatorParameters(string state, MovementDirection? direction = null)
        {
            _animator.SetBool("IsIdle", state == "Idle");
            _animator.SetBool("IsWalkingLeft", direction == MovementDirection.Left && state == "Walking");
            _animator.SetBool("IsWalkingRight", direction == MovementDirection.Right && state == "Walking");
            _animator.SetBool("IsWalkingUp", direction == MovementDirection.Up && state == "Walking");
            _animator.SetBool("IsWalkingDown", direction == MovementDirection.Down && state == "Walking");

            if (direction == MovementDirection.Left)
            {
                _spriteRenderer.flipX = true;
            }
            else if (direction == MovementDirection.Right)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

    public enum MovementState
    {
        Idle,
        Walking,
        Dashing
    }

    public enum MovementDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}