using Project._Scripts.Common.Eventing;
using UnityEngine;

namespace Project._Scripts.UI
{
    public class CursorController : MonoBehaviour
    {
        private Camera _camera;

        public void Start()
        {
            EventBus.Subscribe<Events.MenuOpened>(_ => EnableCursor());
            EventBus.Subscribe<Events.MenuClosed>(_ => DisableCursor());

            _camera = Camera.main;
            Cursor.visible = false;
        }

        public void Update()
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            transform.position = mousePos;
        }

        private void EnableCursor()
        {
            Cursor.visible = true;
        }

        private void DisableCursor()
        {
            Cursor.visible = false;
        }
    }
}
