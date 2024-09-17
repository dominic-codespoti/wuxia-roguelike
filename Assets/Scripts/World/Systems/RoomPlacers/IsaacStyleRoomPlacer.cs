using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace World.Systems.RoomPlacers
{
    public class IsaacStyleRoomPlacer : IRoomPlacer
    {
        private readonly int _gridWidth;
        private readonly int _gridHeight;
        private readonly bool[,] _occupiedCells;
        private readonly List<Room> _placedRooms;
        private readonly Vector2Int _startingCell;
        private readonly Vector2Int[] _directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        public IsaacStyleRoomPlacer(int width, int height)
        {
            _gridWidth = width;
            _gridHeight = height;
            _occupiedCells = new bool[width, height];
            _placedRooms = new List<Room>();
            _startingCell = new Vector2Int(width / 2, height / 2);
        }

        public List<Room> PlaceRooms(int roomCount, int roomWidth, int roomHeight, System.Func<Vector2, RoomType, GameObject> roomGenerator)
        {
            Queue<Vector2Int> cellsToExpand = new Queue<Vector2Int>();
            cellsToExpand.Enqueue(_startingCell);

            int i = 0;
            while (_placedRooms.Count < roomCount && cellsToExpand.Count > 0)
            {
                Vector2Int currentCell = cellsToExpand.Dequeue();

                if (!_occupiedCells[currentCell.x, currentCell.y])
                {
                    PlaceRoom(currentCell, roomWidth, roomHeight, roomCount, i, roomGenerator);
                    i++;

                    // Add adjacent cells to the queue
                    foreach (Vector2Int direction in _directions)
                    {
                        Vector2Int adjacentCell = currentCell + direction;
                        if (IsValidCell(adjacentCell))
                        {
                            cellsToExpand.Enqueue(adjacentCell);
                        }
                    }
                }
            }

            return _placedRooms;
        }

        private void PlaceRoom(Vector2Int cell, int roomWidth, int roomHeight, int maxRooms, int index, System.Func<Vector2, RoomType, GameObject> roomGenerator)
        {
            var roomType = index switch {
                0 => RoomType.Spawn,
                _ when index == maxRooms - 1 => RoomType.Boss,
                _ => RoomType.Normal
            };

            Vector2 position = new Vector2(cell.x * roomWidth, cell.y * roomHeight);
            GameObject roomObj = roomGenerator(position, roomType);
            Room room = roomObj.GetComponent<Room>();

            if (room != null)
            {
                _occupiedCells[cell.x, cell.y] = true;
                _placedRooms.Add(room);
            }
        }

        private bool IsValidCell(Vector2Int cell)
        {
            if (cell.x < 0 || cell.x >= _gridWidth || cell.y < 0 || cell.y >= _gridHeight || _occupiedCells[cell.x, cell.y])
            {
                return false;
            }

            // Check if the cell is adjacent to an existing room
            foreach (Vector2Int direction in _directions)
            {
                Vector2Int adjacentCell = cell + direction;
                if (adjacentCell.x >= 0 && adjacentCell.x < _gridWidth && 
                    adjacentCell.y >= 0 && adjacentCell.y < _gridHeight && 
                    _occupiedCells[adjacentCell.x, adjacentCell.y])
                {
                    return true;
                }
            }

            return false;
        }
    }
}