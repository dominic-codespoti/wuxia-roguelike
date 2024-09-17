using UnityEngine;

namespace World.Factories
{
    /// <summary>
    /// A factory for creating prefabs.
    /// </summary>
    public static class PrefabFactory
    {
        public enum TileType
        {
            Floor,
            Wall,
            Door
        }
        
        public static string GetTileName(Vector2 position, TileType type)
        {
            return $"{type}_{position.x}_{position.y}";
        }

        public static GameObject NewSquare(Sprite sprite, Vector2 position, Transform parent, TileType type)
        {
            // Create game object
            GameObject square = new GameObject(GetTileName(position, type));
            square.transform.position = position;
            square.transform.SetParent(parent);
            square.layer = LayerMask.NameToLayer("Ground");

            // Add sprite renderer
            var spriteRenderer = square.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = Index(type);

            // Configure specifics
            if (type == TileType.Wall)
            {
                ConfigureWall(square);
            }

            return square;
        }
        
        private static int Index(TileType type)
        {
            return type switch
            {
                TileType.Floor => 0,
                TileType.Wall => 1,
                TileType.Door => 2,
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
        
        private static void ConfigureWall(GameObject square)
        {
            var boxCollider = square.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(1, 1);

            square.AddComponent<Wall>();
        }
    }
}