using System.Collections;
using System.Collections.Generic;
using Project._Scripts.Common;
using Project._Scripts.Common.Eventing;
using Project._Scripts.Entities.Player;
using UnityEngine;

namespace Project._Scripts.World.Systems
{
    public class GameState : Singleton<GameState>
    {
        public Room CurrentRoom { get; private set; }
        public List<Room> AllRooms { get; private set; }
        public List<Room> VisitedRooms { get; private set; }
        public Player Player { get; private set; }
        public bool CanUseDoor => !_isDoorCooldownActive && !_isInBossRoomWhileAlive;

        private float _doorCooldownDuration = 1f;
        private bool _isDoorCooldownActive;
        private bool _isInBossRoomWhileAlive;

        protected override void Awake()
        {
            base.Awake();

            Player = FindObjectOfType<Player>();
            VisitedRooms = new List<Room>();
            AllRooms = new List<Room>();

            EventBus.Subscribe<Events.PlayerEnteredRoom>(evt => OnPlayerEnteredRoom(evt));
            EventBus.Subscribe<Events.LevelGenerated>(evt => OnLevelGenerated(evt));
            EventBus.Subscribe<Events.BossSpawned>(evt => _isInBossRoomWhileAlive = true);
            EventBus.Subscribe<Events.BossDied>(evt => _isInBossRoomWhileAlive = false);
        }
        
        private void OnPlayerEnteredRoom(Events.PlayerEnteredRoom e)
        {
            _isDoorCooldownActive = true;
            StartCoroutine(DoorCooldownRoutine());
            CurrentRoom = e.RoomEntered;
            VisitedRooms.Add(e.RoomEntered);
        }
        
        private void OnLevelGenerated(Events.LevelGenerated e)
        {
            AllRooms = e.Rooms;
        }

        private IEnumerator DoorCooldownRoutine()
        {
            yield return new WaitForSeconds(_doorCooldownDuration);
            _isDoorCooldownActive = false;
        }
    }
}