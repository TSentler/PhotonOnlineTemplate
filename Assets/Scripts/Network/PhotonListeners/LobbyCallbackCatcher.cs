using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class LobbyCallbackCatcher : MonoBehaviour
    {
        private LobbyCallbackListener _lobbyCallbacks = new();
        
        public event UnityAction JoinedLobby
        {
            add => _lobbyCallbacks.JoinedLobby += value;
            remove => _lobbyCallbacks.JoinedLobby -= value;
        }
        public event UnityAction LeftLobby
        {
            add => _lobbyCallbacks.LeftLobby += value;
            remove => _lobbyCallbacks.LeftLobby -= value;
        }
        public event UnityAction<List<RoomInfo>> RoomListUpdated
        { 
            add => _lobbyCallbacks.RoomListUpdated += value;
            remove => _lobbyCallbacks.RoomListUpdated -= value;
        }
        public event UnityAction<List<TypedLobbyInfo>> StatsUpdated
        {
            add => _lobbyCallbacks.StatsUpdated += value;
            remove => _lobbyCallbacks.StatsUpdated -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_lobbyCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_lobbyCallbacks);
        }
    }
}
