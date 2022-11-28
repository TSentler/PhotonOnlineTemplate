using System;
using System.Collections.Generic;
using Network;
using Photon.Realtime;
using UnityEngine;

namespace LevelLoaders
{
    public class RoomsInfo : MonoBehaviour
    {
        private List<RoomInfo> _roomInfos = new List<RoomInfo>();
        private LobbyCallbackCatcher _lobbyCalback;

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

        public string FindRoomName(string levelName)
        {
            if (_roomInfos.Count == 0)
                return null;
            
            for (int i = 0; i < _roomInfos.Count; i++)
            {
                var roomInfo = _roomInfos[i];
                if (roomInfo.CustomProperties.ContainsKey(LevelRoom.SceneNameKey) == false
                    && levelName == roomInfo.CustomProperties[LevelRoom.SceneNameKey].ToString())
                {
                    if (roomInfo.PlayerCount < roomInfo.MaxPlayers)
                    {
                        return roomInfo.Name;
                    }

                    return null;
                }
            }

            return null;
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