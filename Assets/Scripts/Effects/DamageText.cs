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
        
        public TextMeshPro textMesh;

        private void Start()
        {
            textMesh = GetComponent<TextMeshPro>();

            Destroy(gameObject, LifeTime);
        }

        public void SetText(string text)
        {
            textMesh.text = text;
        }

        private void Update()
        {
            transform.position += Vector3.up * FloatSpeed * Time.deltaTime;

            var color = textMesh.color;
            color.a -= Time.deltaTime / LifeTime;
            textMesh.color = color;
        }
    }
}
