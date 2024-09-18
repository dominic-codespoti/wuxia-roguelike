using System.Collections.Generic;
using Project._Scripts.World;
using Project._Scripts.World.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project._Scripts.UI.Elements
{
    public class Minimap : MonoBehaviour
    {
        public float scaleModifier = 0.8f;

        private VisualElement _minimapContainer;
        private VisualElement _mapContent;
        private Dictionary<Room, VisualElement> _roomElements = new();
        private VisualElement _playerMarker;
        private Vector2 _minimapCenter;
        private bool _isInitialized = false;

        public void Initialize(VisualElement root)
        {
            _minimapContainer = root.Q<VisualElement>("minimap-container");
            _playerMarker = _minimapContainer.Q<VisualElement>("player-marker");

            _mapContent = new VisualElement { name = "map-content" };
            _mapContent.style.position = Position.Absolute;
            _minimapContainer.Add(_mapContent);

            _minimapContainer.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                _minimapCenter = new Vector2(_minimapContainer.layout.width / 2, _minimapContainer.layout.height / 2);
                GenerateMinimapRooms();
                UpdateMap();
                _isInitialized = true;
            });
        }

        public void Update()
        {
            if (_isInitialized)
            {
                UpdateMap();
            }
        }
        
        public void Hide()
        {
            _minimapContainer.style.display = DisplayStyle.None;
        }
        
        public void Show()
        {
            _minimapContainer.style.display = DisplayStyle.Flex;
        }

        private void GenerateMinimapRooms()
        {
            var rooms = GameState.Instance.AllRooms;
            if (rooms.Count == 0) return;

            // Find the bounds of all rooms
            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            foreach (var room in rooms)
            {
                minX = Mathf.Min(minX, room.Position.x);
                minY = Mathf.Min(minY, room.Position.y);
                maxX = Mathf.Max(maxX, room.Position.x + room.width);
                maxY = Mathf.Max(maxY, room.Position.y + room.height);
            }

            float totalWidth = maxX - minX;
            float totalHeight = maxY - minY;
            float scale = scaleModifier * Mathf.Min(_minimapContainer.layout.width / totalWidth, _minimapContainer.layout.height / totalHeight) * 0.9f; // 90% of the container size

            foreach (var room in rooms)
            {
                var roomElement = new VisualElement
                {
                    name = $"room-{room.Id}",
                    style = {
                        position = Position.Absolute,
                        width = room.width * scale,
                        height = room.height * scale,
                        left = (room.Position.x - minX) * scale,
                        top = (_minimapContainer.layout.height - (room.Position.y - minY + room.height) * scale), // Invert Y-axis
                        backgroundColor = room.type == RoomType.Boss ? Color.red : new Color(0.5f, 0.5f, 0.5f, 0.5f),
                        borderLeftWidth = 1,
                        borderRightWidth = 1,
                        borderTopWidth = 1,
                        borderBottomWidth = 1,
                    }
                };

                _mapContent.Add(roomElement);
                _roomElements[room] = roomElement;
            }
        }

        private void UpdateMap()
{
    if (GameState.Instance?.Player == null || GameState.Instance.CurrentRoom == null)
    {
        return;
    }

    var currentRoom = GameState.Instance.CurrentRoom;
    var playerPos = GameState.Instance.Player.transform.position;

    // Calculate player's position relative to the current room (0 to 1 range)
    var relativePlayerPos = new Vector2(
        Mathf.Clamp01((playerPos.x - currentRoom.Position.x) / Mathf.Max(currentRoom.width, 1f)),
        Mathf.Clamp01((playerPos.y - currentRoom.Position.y) / Mathf.Max(currentRoom.height, 1f))
    );

    // Get room element for the current room
    if (!_roomElements.TryGetValue(currentRoom, out var currentRoomElement))
    {
        return;
    }

    // Calculate the center of the current room on the minimap
    float roomCenterX = currentRoomElement.resolvedStyle.left + (currentRoomElement.resolvedStyle.width / 2);
    float roomCenterY = currentRoomElement.resolvedStyle.top + (currentRoomElement.resolvedStyle.height / 2);

    // Calculate the offset to center the current room in the minimap container
    float offsetX = _minimapCenter.x - roomCenterX;
    float offsetY = _minimapCenter.y - roomCenterY;

    // Apply the offset to _mapContent to center the current room
    _mapContent.style.left = offsetX;
    _mapContent.style.top = offsetY;

    // Calculate the player's position on the minimap relative to the room-centered minimap
    float markerLeft = relativePlayerPos.x * currentRoomElement.resolvedStyle.width;
    float markerTop = (1 - relativePlayerPos.y) * currentRoomElement.resolvedStyle.height;

    // Adjust the player's marker position on the minimap with the offset applied
    _playerMarker.style.left = markerLeft + _minimapCenter.x - currentRoomElement.resolvedStyle.width / 2;
    _playerMarker.style.top = markerTop + _minimapCenter.y - currentRoomElement.resolvedStyle.height / 2;

    // Update visibility of rooms
    foreach (var roomPair in _roomElements)
    {
        if (roomPair.Key == null) continue;

        bool isVisible = GameState.Instance.VisitedRooms.Contains(roomPair.Key) || roomPair.Key == currentRoom;
        roomPair.Value.style.opacity = isVisible ? 1 : 0.2f;

        // Highlight current room
        if (roomPair.Key == currentRoom)
        {
            roomPair.Value.style.backgroundColor = new Color(0.7f, 0.7f, 1f, 0.8f);
        }
        else
        {
            roomPair.Value.style.backgroundColor = roomPair.Key.type == RoomType.Boss ? Color.red : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }
}

    }
}