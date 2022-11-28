using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace LevelLoaders
{
    public enum LevelNames
    {
        Game
    }

    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private RoomsInfo _roomsInfo;
        
        private LevelRoom _currentLevelRoom;
        private MatchmakingCallbacksCatcher _matchCallback;
        
        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
        }

        private void OnEnable()
        {
            _matchCallback.CreatedRoom += OnCreatedRoom;
            _matchCallback.JoinRandomFailed += OnJoinRoomFailed;
            _matchCallback.JoinRoomFailed += OnJoinRoomFailed;
        }

        private void OnDisable()
        {
            _matchCallback.CreatedRoom -= OnCreatedRoom;
            _matchCallback.JoinRandomFailed -= OnJoinRoomFailed;
            _matchCallback.JoinRoomFailed -= OnJoinRoomFailed;
        }

        public void StartGame()
        {
            CreateOrJoinMap(LevelRoom.CreateChatLevelRoom());
        }

        public void CreateOrJoinRandom()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
        }

        private void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name, _currentLevelRoom.RoomOptions);
        }

        private void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Faild join room");
            var name = _currentLevelRoom.GenerateRoomName();
            CreateRoom(name);
        }

        private void OnCreatedRoom()
        {
            PhotonNetwork.LoadLevel(_currentLevelRoom.LevelName);
        }

        private void CreateOrJoinMap(LevelRoom levelRoom)
        {
            _currentLevelRoom = levelRoom;
            var name = _roomsInfo.FindRoomName(levelRoom.LevelName);
            if (name == null)
            {
                name = _currentLevelRoom.GenerateRoomName();
            }
            PhotonNetwork.JoinOrCreateRoom(
                name, _currentLevelRoom.RoomOptions, TypedLobby.Default);
        }
    }
}
