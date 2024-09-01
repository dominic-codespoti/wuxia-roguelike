using Interfaces;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class ShadowEmitter : MonoBehaviour
    {
        public Vector2 shadowOffset = new Vector2(0f, -0.1f);
        public Color shadowColor = new Color(255, 255, 255, 0.5f);
        public float maxShadowHeight = 3f;
        public float maxShadowWidth = 10f;
        public float minShadowScale = 0.2f;

        private GameObject shadowObject;
        private SpriteRenderer shadowRenderer;
        private Maybe<IDynamicHeight> dynamicHeight;

        public void Start()
        {
            dynamicHeight = gameObject.MaybeGetComponent<IDynamicHeight>();

            shadowObject = new GameObject("Shadow");
            shadowObject.transform.parent = transform;

            shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();

            Sprite shadowSprite = CreateShadowSprite();
            shadowRenderer.sprite = shadowSprite;
            shadowRenderer.color = shadowColor;
            shadowRenderer.sortingOrder = 3;
        }

        public void Update()
        {
            shadowObject.transform.localPosition = (Vector3)shadowOffset;

            dynamicHeight.Match(
                some: dh =>
                {
                    float height = dh.GetHeight();
                    shadowObject.transform.localPosition -= new Vector3(0, height, 0);

                    float normalizedHeight = Mathf.Clamp01(height / maxShadowHeight);
                    float shadowScale = Mathf.Lerp(1f, minShadowScale, normalizedHeight);
                    shadowObject.transform.localScale = new Vector3(shadowScale, shadowScale, 1f);
                },
                none: () => { });
        }

        private Sprite CreateShadowSprite()
        {
            Vector3 shadowScale = GenerateShadowCube();

            Rect rectangle = new Rect(0, 0, shadowScale.x, shadowScale.y);
            Texture2D shadowTexture = new Texture2D((int)shadowScale.x, (int)shadowScale.y);

            Color[] pixels = new Color[shadowTexture.width * shadowTexture.height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = shadowColor;
            }
            shadowTexture.SetPixels(pixels);
            shadowTexture.Apply();

            Sprite shadowSprite = Sprite.Create(shadowTexture, rectangle, Vector2.one * 0.5f);
            return shadowSprite;
        }

        private Vector3 GenerateShadowCube()
        {
            float width = (int)maxShadowWidth;
            float height = (int)maxShadowHeight;
            return new Vector3(width, height, 1f);
        }
    }
}
