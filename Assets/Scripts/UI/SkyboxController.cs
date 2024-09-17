#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using Common;
using UnityEngine;
using World.Systems;

namespace UI
{
    public class SkyboxController : Singleton<SkyboxController>
    {
        public Color skyboxColor = Color.white;
        public Sprite targetSprite;
        public float darkOrLightFactor = 2f;
        
        private bool _needsRenewal = false;

        private void Update()
        {
            if (_needsRenewal)
            {
                SetSkyboxColor();
                _needsRenewal = false;
            }
        }

        public void OnValidate()
        {
            if (targetSprite != null)
            {
                MakeTextureReadable(targetSprite.texture);
                skyboxColor = GetMostPrevalentColor(targetSprite);
                skyboxColor /= darkOrLightFactor;
                _needsRenewal = true;
            }
        }

        private void SetSkyboxColor()
        {
            if (Camera.main != null)
            {
                Camera.main.backgroundColor = skyboxColor;
                Camera.main.clearFlags = CameraClearFlags.SolidColor;
            }

            BackgroundManager.Instance.RenewBackground();
        }

        private static Color GetMostPrevalentColor(Sprite sprite)
        {
            Texture2D texture = sprite.texture;

            if (texture == null || !texture.isReadable)
            {
                Debug.LogError("Texture is not readable and couldn't be made readable.");
                return Color.black;
            }

            Color[] pixels = texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
                                               (int)sprite.rect.width, (int)sprite.rect.height);

            Dictionary<Color, int> colorCount = new Dictionary<Color, int>();
            Color prevalentColor = Color.black;
            int maxCount = 0;

            foreach (Color pixel in pixels)
            {
                if (colorCount.ContainsKey(pixel))
                {
                    colorCount[pixel]++;
                }
                else
                {
                    colorCount[pixel] = 1;
                }

                if (colorCount[pixel] > maxCount)
                {
                    maxCount = colorCount[pixel];
                    prevalentColor = pixel;
                }
            }

            return prevalentColor;
        }

        private static void MakeTextureReadable(Texture2D texture)
        {
#if UNITY_EDITOR
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Texture '{texture.name}' is now readable.");
            }
#endif
        }
    }
}