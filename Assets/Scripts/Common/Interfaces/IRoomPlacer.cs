using System;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace Common.Interfaces
{
    public interface IRoomPlacer
    {
        List<Room> PlaceRooms(int roomCount, int roomMaxWidth, int roomMaxHeight, Func<Vector2, RoomType, GameObject> roomGenerator);
    }
}