using System;
using System.Collections.Generic;
using Project._Scripts.World;
using UnityEngine;

namespace Project._Scripts.Common.Interfaces
{
    public interface IRoomPlacer
    {
        List<Room> PlaceRooms(int roomCount, int roomMaxWidth, int roomMaxHeight, Func<Vector2, RoomType, GameObject> roomGenerator);
    }
}