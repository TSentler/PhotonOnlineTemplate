using Network;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public enum LevelNames
    {
        Game
    }

    public class LevelRoom
    {
        private string _levelName;
        private RoomOptions _roomOptions;

        public string LevelName => _levelName;
        public RoomOptions RoomOptions => _roomOptions;
        
        private LevelRoom(string levelName, RoomOptions roomOptions)
        {
            _levelName = levelName;
            _roomOptions = roomOptions;
        }

        public static LevelRoom CreateChatLevelRoom()
        {
            var chatRoomOptions = new RoomOptions();
            chatRoomOptions.MaxPlayers = 5;
            chatRoomOptions.CleanupCacheOnLeave = true;
            chatRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            chatRoomOptions.CustomRoomPropertiesForLobby = new string[] { LevelLoader.SceneNameKey };
            chatRoomOptions.CustomRoomProperties.Add(LevelLoader.SceneNameKey, LevelNames.Game.ToString());
            return new LevelRoom(LevelNames.Game.ToString(), chatRoomOptions);
        }
        
        public string GenerateRoomName()
        {
           return "Room" + _levelName + PhotonNetwork.NickName 
                  + Random.Range(0, 1000).ToString();
        }
    }
    
    public class LevelLoader : MonoBehaviour
    {
        public const string SceneNameKey = "SceneName";

        private LevelRoom _currentLevelRoom;

        private MatchmakingCallbacksCatcher _matchCallback;
        private LobbyCallbackCatcher _lobbyCalback;
        private LevelRoom _chatLevelRoom;
        private List<RoomInfo> _roomInfos = new List<RoomInfo>();
        
        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _lobbyCalback = FindObjectOfType<LobbyCallbackCatcher>();
            _chatLevelRoom = LevelRoom.CreateChatLevelRoom();
        }

        private void OnEnable()
        {
            _matchCallback.CreatedRoom += OnCreatedRoom;
            _matchCallback.JoinRandomFailed += OnJoinRoomFailed;
            _matchCallback.JoinRoomFailed += OnJoinRoomFailed;
            _lobbyCalback.RoomListUpdated += OnRoomListUpdated;
        }

        private void OnDisable()
        {
            _matchCallback.CreatedRoom -= OnCreatedRoom;
            _matchCallback.JoinRandomFailed -= OnJoinRoomFailed;
            _matchCallback.JoinRoomFailed -= OnJoinRoomFailed;
            _lobbyCalback.RoomListUpdated -= OnRoomListUpdated;
        }

        private void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Faild join room");
            var name = _currentLevelRoom.GenerateRoomName();
            CreateRoom(name);
        }

        public void StartGame()
        {
            CreateOrJoinMap(_chatLevelRoom);
        }

        private void OnCreatedRoom()
        {
            PhotonNetwork.LoadLevel(_currentLevelRoom.LevelName);
        }

        public void CreateOrJoinRandom()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
        }

        private void CreateOrJoinMap(LevelRoom levelRoom)
        {
            _currentLevelRoom = levelRoom;
            var name = FindRoomName(levelRoom.LevelName, 
                levelRoom.RoomOptions.MaxPlayers);
            if (name == null)
            {
                name = _currentLevelRoom.GenerateRoomName();
            }
            PhotonNetwork.JoinOrCreateRoom(
                name, _currentLevelRoom.RoomOptions, TypedLobby.Default);
        }

        private string FindRoomName(string levelName, int maxPlayers)
        {
            if (_roomInfos.Count == 0)
                return null;
            
            for (int i = 0; i < _roomInfos.Count; i++)
            {
                var roomInfo = _roomInfos[i];
                if (roomInfo.CustomProperties.ContainsKey(SceneNameKey) == false
                    && levelName == roomInfo.CustomProperties[SceneNameKey].ToString())
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

        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name, _currentLevelRoom.RoomOptions);
        }

        public void OnRoomListUpdated(List<RoomInfo> roomInfos)
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
                if (roomInfo.CustomProperties.ContainsKey(SceneNameKey) == false)
                {
                    Debug.Log("Not contain key " + roomInfo.Name);
                    continue;
                }

                Debug.Log("Available room: " + roomInfo.Name);
            }
        }
    }
}
