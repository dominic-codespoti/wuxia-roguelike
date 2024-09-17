using System.Collections;
using Common.Eventing;
using Entities.Player;
using UnityEngine;
using World.Systems;

namespace World
{
    public class Door : MonoBehaviour
    {
        public Vector2 otherNewPosition;
        public BoxCollider2D body;
        public Room otherRoom;
        public Room thisRoom;

        private readonly AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private const float DoorOpenDuration = 0.3f;
        private const float TransitionDuration = 0.5f;
        private const float DoorOpenAngle = 90f;

        public void Configure(Vector2 otherNewPosition, Room otherRoom, Room thisRoom)
        {
            this.otherNewPosition = otherNewPosition;
            this.otherRoom = otherRoom;
            this.thisRoom = thisRoom;

            body = gameObject.AddComponent<BoxCollider2D>();
            body.isTrigger = true;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(otherNewPosition, Vector3.one);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameState.Instance.CanUseDoor && collision.gameObject.GetComponent<Player>() != null)
            {
                StartCoroutine(TransitionPlayer(collision.transform));
            }
        }

        private IEnumerator TransitionPlayer(Transform playerTransform)
        {
            yield return StartCoroutine(AnimateDoorUnscaled(true));
            yield return StartCoroutine(FadeScreenUnscaled(true));

            EventBus.Publish(new Events.PlayerEnteredRoom(otherRoom), otherRoom.Id);
            playerTransform.position = otherNewPosition;
            
            yield return StartCoroutine(FadeScreenUnscaled(false));
            yield return StartCoroutine(AnimateDoorUnscaled(false));

            EventBus.Publish(new Events.PlayerExitedRoom(thisRoom), thisRoom.Id);
        }

        private IEnumerator AnimateDoorUnscaled(bool opening)
        {
            float startAngle = opening ? 0 : DoorOpenAngle;
            float endAngle = opening ? DoorOpenAngle : 0;
            float startTime = Time.unscaledTime;
            
            while (Time.unscaledTime - startTime < DoorOpenDuration)
            {
                float t = (Time.unscaledTime - startTime) / DoorOpenDuration;
                float angle = Mathf.Lerp(startAngle, endAngle, _transitionCurve.Evaluate(t));
                transform.rotation = Quaternion.Euler(0, 0, angle);

                yield return null;
            }

            transform.rotation = Quaternion.Euler(0, 0, endAngle);
        }

        private IEnumerator FadeScreenUnscaled(bool fadeOut)
        {
            EventBus.Publish(new Events.FadeStarted());

            float startTime = Time.unscaledTime;
            Color startColor = fadeOut ? Color.clear : Color.black;
            Color endColor = fadeOut ? Color.black : Color.clear;

            while (Time.unscaledTime - startTime < TransitionDuration)
            {
                float t = (Time.unscaledTime - startTime) / TransitionDuration;
                Color currentColor = Color.Lerp(startColor, endColor, _transitionCurve.Evaluate(t));
                EventBus.Publish(new Events.FadeAdjusted(currentColor));

                yield return null;
            }

            EventBus.Publish(new Events.FadeAdjusted(endColor));
            EventBus.Publish(new Events.FadeEnded());
        }
    }
}