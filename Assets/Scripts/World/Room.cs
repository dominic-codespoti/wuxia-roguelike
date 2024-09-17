using System;
using Common.Eventing;
using UnityEditor;
using UnityEngine;
using World.Factories;
using World.Systems;

namespace World
{
    /// <summary>
    /// A 2D room that contains enemies and connects to other rooms.
    /// </summary>
    public class Room : MonoBehaviour
    {
        public Vector2 position;
        public int width;
        public int height;
        public Transform tiles;
        public RoomType type;
        
        public Vector2 Position => position;
        public string Id => $"{position.x}_{position.y}";
        public Vector2 Center => new(position.x + width / 2, position.y + height / 2);
        
        public void Configure(Vector2 position, int width, int height, RoomType type)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.tiles = transform.Find("Tiles"); 
            this.type = type;
        }
        
        public void OnDrawGizmos()
        {
            var color = type switch
            {
                RoomType.Normal => Color.white,
                RoomType.Spawn => Color.green,
                RoomType.Boss => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Gizmos.color = color;
            Gizmos.DrawWireCube(new Vector3(position.x + width / 2, position.y + height / 2, 0), new Vector3(width, height, 0));
        }
        
        public void ConnectTo(Room connectedRoom, Direction direction, Sprite doorTile, Sprite floorTile, bool recurse = true)
        {
            // Place the door in the current room in the middle of the wall based on the direction
            Vector2 doorPosition = SnapToGrid(direction switch
            {
                Direction.North => new Vector2(position.x + (width / 2), position.y + height - 1),
                Direction.East  => new Vector2(position.x + width - 1, position.y + (height / 2)),
                Direction.South => new Vector2(position.x + (width / 2), position.y),
                Direction.West  => new Vector2(position.x, position.y + (height / 2)),
                _ => throw new ArgumentOutOfRangeException()
            });

            // Remove wall tile and place door in the current room
            Transform wallTile = tiles.Find($"Wall_{doorPosition.x}_{doorPosition.y}");
            if (wallTile != null)
            {
                DestroyImmediate(wallTile.gameObject);
                var floor = PrefabFactory.NewSquare(floorTile, doorPosition, tiles, PrefabFactory.TileType.Floor);
                floor.name = "Floor";
            }
            
            // Now calculate where to place the door in the connected room based on the current room's door position
            Vector2 connectedDoorPosition = SnapToGrid(direction switch
            {
                Direction.North => new Vector2(connectedRoom.position.x + (connectedRoom.width / 2), connectedRoom.position.y),
                Direction.East  => new Vector2(connectedRoom.position.x, connectedRoom.position.y + (connectedRoom.height / 2)),
                Direction.South => new Vector2(connectedRoom.position.x + (connectedRoom.width / 2), connectedRoom.position.y + connectedRoom.height - 1),
                Direction.West  => new Vector2(connectedRoom.position.x + connectedRoom.width - 1, connectedRoom.position.y + (connectedRoom.height / 2)),
                _ => throw new ArgumentOutOfRangeException()
            });

            // Remove wall tile and place door in the connected room
            wallTile = connectedRoom.tiles.Find($"Wall_{connectedDoorPosition.x}_{connectedDoorPosition.y}");
            if (wallTile != null)
            {
                DestroyImmediate(wallTile.gameObject);
                var floor = PrefabFactory.NewSquare(floorTile, doorPosition, tiles, PrefabFactory.TileType.Floor);
                floor.name = "Floor";
            }
            
            if (recurse)
            {
                connectedRoom.ConnectTo(this, direction.Opposite(), doorTile, floorTile, false);
            }
            
            // Set ejection spot within the room, one unit away from the door in the opposite direction
            Vector2 ejectionPosition = SnapToGrid(direction switch
            {
                Direction.North => new Vector2(connectedDoorPosition.x, connectedDoorPosition.y + 1),  // One step downward
                Direction.East  => new Vector2(connectedDoorPosition.x + 1, connectedDoorPosition.y),  // One step right
                Direction.South => new Vector2(connectedDoorPosition.x, connectedDoorPosition.y - 1),  // One step upward
                Direction.West  => new Vector2(connectedDoorPosition.x - 1, connectedDoorPosition.y),  // One step left
                _ => throw new ArgumentOutOfRangeException()
            });
            
            var door = PrefabFactory.NewSquare(doorTile, doorPosition, tiles, PrefabFactory.TileType.Door);
            door.name = "Door";
            var body = door.AddComponent<BoxCollider2D>();
            body.size = new Vector2(1, 1);
            var doorComponent = door.AddComponent<Door>();
            doorComponent.Configure(ejectionPosition, connectedRoom, this);
        }

        private static Vector2 SnapToGrid(Vector2 position)
        {
            return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        }
    }
}