using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace LevelLoaders
{
    public class LevelRoom
    {
        private LevelRoom(string levelName, RoomOptions roomOptions)
        {
            _levelName = levelName;
            _roomOptions = roomOptions;
        }

        public const string SceneNameKey = "SceneName";

        private string _levelName;
        private RoomOptions _roomOptions;

        public string LevelName => _levelName;
        public RoomOptions RoomOptions => _roomOptions;
        
        public static LevelRoom CreateChatLevelRoom()
        {
            var chatRoomOptions = new RoomOptions();
            chatRoomOptions.MaxPlayers = 5;
            chatRoomOptions.CleanupCacheOnLeave = true;
            chatRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            chatRoomOptions.CustomRoomPropertiesForLobby = new string[] { SceneNameKey };
            chatRoomOptions.CustomRoomProperties.Add(SceneNameKey, LevelNames.Game.ToString());
            return new LevelRoom(LevelNames.Game.ToString(), chatRoomOptions);
        }
        
        public string GenerateRoomName()
        {
            return "Room" + _levelName + PhotonNetwork.NickName 
                   + Random.Range(0, 1000).ToString();
        }
    }
}