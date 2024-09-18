using System.Collections.Generic;
using System.Linq;
using Project._Scripts.Common;
using Project._Scripts.Common.Attributes;
using Project._Scripts.Common.Eventing;
using Project._Scripts.World.Factories;
using Project._Scripts.World.Systems.RoomPlacers;
using UnityEngine;

namespace Project._Scripts.World.Systems
{
    public class MapGenerator : Singleton<MapGenerator>
    {
        [InspectorButton(nameof(Randomize))]
        public bool randomize;
        [InspectorButton(nameof(Clear))]
        public bool clear;

        public int rooms = 10;
        public int width = 512;
        public int height = 512;
        public int roomMaxWidth = 32;
        public int roomMaxHeight = 32;

        public Sprite[] floorTiles;
        public Sprite verticalDoorTile;
        public Sprite horizontalDoorTile;
        public Sprite topWallTile;
        public Sprite bottomWallTile;
        public Sprite leftWallTile;
        public Sprite rightWallTile;
        public Transform parent;

        public void Start()
        {
            var createdRooms = parent.GetComponentsInChildren<Room>();
            GameObject player = GameObject.Find("Player");
            player.transform.position = createdRooms[0].Center;
            EventBus.Publish(new Events.LevelGenerated(createdRooms.ToList()));
            EventBus.Publish(new Events.PlayerEnteredRoom(createdRooms[0]), createdRooms[0].Id);
        }

        public void Randomize()
        {
            var roomPlacer = new IsaacStyleRoomPlacer(width / roomMaxWidth, height / roomMaxHeight);
            List<Room> placedRooms = roomPlacer.PlaceRooms(rooms, roomMaxWidth, roomMaxHeight, GenerateRoomAt);
            ConnectRooms(placedRooms);
        }
        
        public void Clear()
        {
            foreach (Transform child in parent)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        
        private void ConnectRooms(List<Room> rooms)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    if (AreRoomsAdjacent(rooms[i], rooms[j]))
                    {
                        Direction connectionDirection = DetermineConnectionDirection(rooms[i], rooms[j]);
                        var doorTile = connectionDirection is Direction.North or Direction.South
                            ? verticalDoorTile
                            : horizontalDoorTile;

                        rooms[i].ConnectTo(rooms[j], connectionDirection, doorTile, floorTiles[0]);
                    }
                }
            }
        }

        private bool AreRoomsAdjacent(Room room1, Room room2)
        {
            Vector2 difference = room2.Position - room1.Position;
            return (Mathf.Abs(difference.x) == roomMaxWidth && Mathf.Abs(difference.y) < roomMaxHeight) ||
                   (Mathf.Abs(difference.y) == roomMaxHeight && Mathf.Abs(difference.x) < roomMaxWidth);
        }

        private Direction DetermineConnectionDirection(Room room1, Room room2)
        {
            Vector2 difference = room2.Position - room1.Position;
            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                return difference.x > 0 ? Direction.East : Direction.West;
            }
            else
            {
                return difference.y > 0 ? Direction.North : Direction.South;
            }
        }

        private GameObject GenerateRoomAt(Vector2 position, RoomType type)
        {
            var obj = new GameObject("Room");
            var w = Random.Range(16, roomMaxWidth);
            var h = Random.Range(16, roomMaxHeight);
            
            var objChildren = new GameObject("Tiles");
            objChildren.transform.SetParent(obj.transform);

            // Place floor tiles (no colliders)
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    var randomTile = floorTiles[Random.Range(0, floorTiles.Length)];
                    PrefabFactory.NewSquare(randomTile, position + new Vector2(x, y), objChildren.transform, PrefabFactory.TileType.Floor);
                }
            }

            // Place walls around the room's borders
            for (int x = 0; x < w; x++)
            {
                PrefabFactory.NewSquare(topWallTile, position + new Vector2(x, 0), objChildren.transform, PrefabFactory.TileType.Wall);
                PrefabFactory.NewSquare(bottomWallTile, position + new Vector2(x, h - 1), objChildren.transform, PrefabFactory.TileType.Wall);
            }

            for (int y = 0; y < h; y++)
            {
                PrefabFactory.NewSquare(leftWallTile, position + new Vector2(0, y), objChildren.transform, PrefabFactory.TileType.Wall);
                PrefabFactory.NewSquare(rightWallTile, position + new Vector2(w - 1, y), objChildren.transform, PrefabFactory.TileType.Wall);
            }

            var room = obj.AddComponent<Room>();
            room.Configure(position, w, h, type);
            obj.transform.SetParent(parent);

            return obj;
        }
    }
}