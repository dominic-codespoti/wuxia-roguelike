using Interfaces;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// A script that makes an object bob up and down.
    /// </summary>
    public class Bob : MonoBehaviour, IDynamicHeight
    {
        [field: SerializeField] public float minFloatSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float maxFloatSpeed { get; private set; } = 2.0f;
        [field: SerializeField] public float floatHeight { get; private set; } = 0.1f;
        [field: SerializeField] public float pickupRange { get; private set; } = 0.0125f;
        [field: SerializeField] public float pickupSpeed { get; private set; } = 10.0f;

        private Vector3 startPosition;
        private float offset;
        private float floatSpeed;
        private Transform playerTransform;
        private bool isFollowingPlayer;

        public void Start()
        {
            startPosition = transform.position;
            offset = Random.Range(-floatHeight, floatHeight);
            floatSpeed = Random.Range(minFloatSpeed, maxFloatSpeed);
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            isFollowingPlayer = IsInRange();
        }

        public void Update()
        {
            if (!isFollowingPlayer)
            {
                offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                transform.position = startPosition + new Vector3(0f, offset, 0f);

                var isInRange = IsInRange();
                if (isInRange)
                {
                    isFollowingPlayer = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, pickupSpeed * Time.deltaTime);
            }
        }

        public float GetHeight()
        {
            return offset;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, pickupRange);
        }

        private bool IsInRange()
        {
            var distance = Vector2.Distance(transform.position, playerTransform.position);
            return distance <= pickupRange;
        }
    }
}
