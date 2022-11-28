using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class LobbyCallbackListener : ILobbyCallbacks
    {
        public event UnityAction JoinedLobby, LeftLobby;
        public event UnityAction<List<RoomInfo>> RoomListUpdated;
        public event UnityAction<List<TypedLobbyInfo>> StatsUpdated;

        public void OnJoinedLobby()
        {
            JoinedLobby?.Invoke();
        }

        public void OnLeftLobby()
        {
            LeftLobby?.Invoke();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            RoomListUpdated?.Invoke(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            StatsUpdated?.Invoke(lobbyStatistics);
        }
    }
}
