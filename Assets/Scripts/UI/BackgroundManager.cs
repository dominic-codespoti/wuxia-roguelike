using UnityEngine;
using System.Collections.Generic;
using Common;
using UI;

namespace World.Systems
{
    public class BackgroundManager : Singleton<BackgroundManager>
    {
        public int shapeCount = 100;
        public float minSize = 0.1f;
        public float maxSize = 1f;
        public float parallaxFactor = 0.1f;
        public float depthMin = 1f;
        public float depthMax = 10f;
        public float viewportBuffer = 0.1f; // Buffer around the viewport

        private List<ShapeInfo> shapes = new List<ShapeInfo>();
        private Vector3 lastCameraPosition;
        private Color[] colors;

        private struct ShapeInfo
        {
            public Transform transform;
            public Vector3 relativePosition;
            public float depth;
        }

        void Start()
        {
            lastCameraPosition = Camera.main.transform.position;
            RenewBackground();
        }

        void Update()
        {
            Vector3 cameraDelta = Camera.main.transform.position - lastCameraPosition;
            
            for (int i = 0; i < shapes.Count; i++)
            {
                ShapeInfo shape = shapes[i];
                Vector3 newPosition = Camera.main.transform.position + shape.relativePosition;
                newPosition -= cameraDelta * (parallaxFactor / shape.depth);

                // Wrap around logic
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(newPosition);
                if (viewportPosition.x < -viewportBuffer) viewportPosition.x = 1 + viewportBuffer;
                if (viewportPosition.x > 1 + viewportBuffer) viewportPosition.x = -viewportBuffer;
                if (viewportPosition.y < -viewportBuffer) viewportPosition.y = 1 + viewportBuffer;
                if (viewportPosition.y > 1 + viewportBuffer) viewportPosition.y = -viewportBuffer;

                newPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
                newPosition.z = shape.depth;

                shape.transform.position = newPosition;
                shape.relativePosition = newPosition - Camera.main.transform.position;
                shapes[i] = shape;
            }

            lastCameraPosition = Camera.main.transform.position;
        }

        public void RenewBackground()
        {
            UpdateColors();
            ClearShapes();
            GenerateShapes();
        }

        private void UpdateColors()
        {
            Color baseColor = SkyboxController.Instance.skyboxColor;
            colors = new Color[]
            {
                baseColor * 1.1f,
                baseColor * 0.9f,
                baseColor * 1.2f,
                baseColor * 0.8f
            };
        }

        private void ClearShapes()
        {
            foreach (var shape in shapes)
            {
                Destroy(shape.transform.gameObject);
            }
            shapes.Clear();
        }

        private void GenerateShapes()
        {
            for (int i = 0; i < shapeCount; i++)
            {
                GameObject shapeObj = new GameObject("Shape_" + i);
                shapeObj.transform.SetParent(transform);

                SpriteRenderer spriteRenderer = shapeObj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = CreateRandomShape();
                spriteRenderer.color = colors[Random.Range(0, colors.Length)];

                float size = Random.Range(minSize, maxSize);
                shapeObj.transform.localScale = new Vector3(size, size, 1);

                float depth = Random.Range(depthMin, depthMax);
                Vector3 randomViewportPosition = new Vector3(
                    Random.value * (1 + 2 * viewportBuffer) - viewportBuffer,
                    Random.value * (1 + 2 * viewportBuffer) - viewportBuffer,
                    depth
                );
                Vector3 randomWorldPosition = Camera.main.ViewportToWorldPoint(randomViewportPosition);
                randomWorldPosition.z = depth;

                shapeObj.transform.position = randomWorldPosition;

                shapes.Add(new ShapeInfo
                {
                    transform = shapeObj.transform,
                    relativePosition = randomWorldPosition - Camera.main.transform.position,
                    depth = depth
                });
            }
        }

        private Sprite CreateRandomShape()
        {
            Texture2D texture = new Texture2D(32, 32);
            texture.filterMode = FilterMode.Point;

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, Color.white);
                }
            }

            switch (Random.Range(0, 3))
            {
                case 0: // Triangle
                    for (int y = 0; y < texture.height; y++)
                    {
                        for (int x = 0; x < y; x++)
                        {
                            texture.SetPixel(x, y, Color.clear);
                        }
                    }
                    break;
                case 1: // Circle
                    float radius = texture.width / 2f;
                    Vector2 center = new Vector2(radius, radius);
                    for (int y = 0; y < texture.height; y++)
                    {
                        for (int x = 0; x < texture.width; x++)
                        {
                            if (Vector2.Distance(new Vector2(x, y), center) > radius)
                            {
                                texture.SetPixel(x, y, Color.clear);
                            }
                        }
                    }
                    break;
                // case 2 is square, which is already filled
            }

            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}