using System;
using System.Collections.Generic;
using System.Linq;
using Network;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace LevelLoaders
{
    public class RoomsInfo : MonoBehaviour
    {
        private List<RoomInfo> _roomInfos = new List<RoomInfo>();
        private LobbyCallbackCatcher _lobbyCalback;

        public event UnityAction<string[]> RoomListUpdated; 

        private void Awake()
        {
            _lobbyCalback = FindObjectOfType<LobbyCallbackCatcher>();
        }

        private void OnEnable()
        {
            _lobbyCalback.RoomListUpdated += OnRoomListUpdated;
        }

        private void OnDisable()
        {
            _lobbyCalback.RoomListUpdated -= OnRoomListUpdated;
        }

        public string FindRoomByLevelName(string levelName)
        {
            if (_roomInfos.Count == 0)
                return null;
            
            for (int i = 0; i < _roomInfos.Count; i++)
            {
                var roomInfo = _roomInfos[i];
                if (roomInfo.CustomProperties.ContainsKey(LevelRoom.SceneNameKey) == false
                    && levelName == roomInfo.CustomProperties[LevelRoom.SceneNameKey].ToString())
                {
                    if (CheckRoomAccess(roomInfo))
                    {
                        return roomInfo.Name;
                    }

                    return null;
                }
            }

            return null;
        }

        private string[] GetRoomNames()
        {
            return _roomInfos.Select(item => item.Name).ToArray();
        }
        
        private bool CheckRoomAccess(RoomInfo roomInfo)
        {
            return roomInfo.IsOpen && roomInfo.IsVisible &&
                   roomInfo.PlayerCount < roomInfo.MaxPlayers;
        }
        
        private void OnRoomListUpdated(List<RoomInfo> roomInfos)
        {
            Debug.Log("Room list update ");
            for (int i = 0; i < roomInfos.Count; i++)
            {
                var roomInfo = roomInfos[i];
                var roomIndex = _roomInfos.FindIndex(
                    room => room.Name == roomInfo.Name);

                if(roomInfo.RemovedFromList || roomInfo.IsOpen == false)
                {
                    if (roomIndex != -1)
                    {
                        _roomInfos.RemoveAt(roomIndex);
                    }
                    continue;
                }
                
                if (roomIndex != -1)
                {
                    _roomInfos.RemoveAt(roomIndex);
                }
                _roomInfos.Add(roomInfo);
            }
            
            //DebugRoomInfos();
            RoomListUpdated?.Invoke(GetRoomNames());
        }

        private void DebugRoomInfos()
        {
            foreach (var roomInfo in _roomInfos)
            {
                if (roomInfo.CustomProperties.
                        ContainsKey(LevelRoom.SceneNameKey) == false)
                {
                    Debug.Log("Not contain key " + roomInfo.Name);
                    continue;
                }

                Debug.Log("Available room: " + roomInfo.Name);
            }
        }
    }
}