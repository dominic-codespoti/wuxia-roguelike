using Common.Interfaces;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// A script that makes an object bob up and down.
    /// </summary>
    public class Bob : MonoBehaviour, IDynamicHeight
    {
        [field: SerializeField] public float MinFloatSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float MaxFloatSpeed { get; private set; } = 2.0f;
        [field: SerializeField] public float FloatHeight { get; private set; } = 0.1f;
        [field: SerializeField] public float PickupRange { get; private set; } = 0.0125f;
        [field: SerializeField] public float PickupSpeed { get; private set; } = 10.0f;

        private Vector3 _startPosition;
        private float _offset;
        private float _floatSpeed;
        private Transform _playerTransform;
        private bool _isFollowingPlayer;

        public void Start()
        {
            _startPosition = transform.position;
            _offset = Random.Range(-FloatHeight, FloatHeight);
            _floatSpeed = Random.Range(MinFloatSpeed, MaxFloatSpeed);
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _isFollowingPlayer = IsInRange();
        }

        public void Update()
        {
            if (!_isFollowingPlayer)
            {
                _offset = Mathf.Sin(Time.time * _floatSpeed) * FloatHeight;
                transform.position = _startPosition + new Vector3(0f, _offset, 0f);

                var isInRange = IsInRange();
                if (isInRange)
                {
                    _isFollowingPlayer = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, PickupSpeed * Time.deltaTime);
            }
        }

        public float GetHeight()
        {
            return _offset;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickupRange);
        }

        private bool IsInRange()
        {
            var distance = Vector2.Distance(transform.position, _playerTransform.position);
            return distance <= PickupRange;
        }
    }
}
