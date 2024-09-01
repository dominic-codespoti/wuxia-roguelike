using TMPro;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Text that is emitted when an entity takes damage.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class DamageText : MonoBehaviour
    {
        [field: SerializeField] public float FloatSpeed { get; private set; } = 1f;
        [field: SerializeField] public float LifeTime { get; private set; } = 0.5f;
        [field: SerializeField] public TextMeshPro _textMesh;

        private Color _originalColor;

        private void Start()
        {
            _textMesh = GetComponent<TextMeshPro>();
            _originalColor = _textMesh.color;

            Destroy(gameObject, LifeTime);
        }

        public void SetText(string text)
        {
            _textMesh.text = text;
        }

        private void Update()
        {
            transform.position += Vector3.up * FloatSpeed * Time.deltaTime;

            var color = _textMesh.color;
            color.a -= Time.deltaTime / LifeTime;
            _textMesh.color = color;
        }
    }
}
