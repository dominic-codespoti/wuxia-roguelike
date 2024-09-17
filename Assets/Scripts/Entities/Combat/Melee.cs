using System.Collections;
using Common;
using Common.Eventing;
using Common.Interfaces;
using UnityEngine;

namespace Entities.Combat
{
    /// <summary>
    /// A 2D melee that arcs out.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Melee : Attack
    {
        [field: SerializeField] public float Length { get; private set; }
        private BoxCollider2D _box;
        private Vector3 _origin;

        public void Start()
        {
            Destroy(gameObject, Lifetime);

            _box = GetComponent<BoxCollider2D>();
            _box.isTrigger = true;
        }

        public void Swing(Vector3 origin, Vector3 mousePosition)
        {
            _origin = origin;

            Vector3 closestPoint = ClosestPointOnArc(mousePosition);
            StartCoroutine(SwingCoroutine(closestPoint));
        }

        private Vector3 ClosestPointOnArc(Vector3 point)
        {
            Vector3 direction = (point - _origin).normalized * Mathf.Min(Length, (point - _origin).magnitude);
            return _origin + direction;
        }

        private IEnumerator SwingCoroutine(Vector3 mousePosition)
        {
            Vector3 arcCenter = _origin;
            Vector3 arcRadius = (mousePosition - _origin).normalized * Length;

            float angle = 0f;
            while (angle < 360f)
            {
                Vector3 swingPoint = arcCenter + Quaternion.Euler(0, 0, angle) * arcRadius;
                transform.position = swingPoint;
                angle += 360f * Time.deltaTime * Speed;

                Vector3 lookDirection = (swingPoint - arcCenter).normalized;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDirection);
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Aggressor))
            {
                return;
            }

            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            Vector2 impact = transform.position - collision.transform.position;
            EventBus.Publish(new Events.EntityDamaged(impact, collision.gameObject, Damage), collision.gameObject.Id());
        }
    }
}
