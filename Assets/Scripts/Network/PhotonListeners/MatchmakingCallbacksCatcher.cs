using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class MatchmakingCallbacksCatcher : MonoBehaviour
    {
        private MatchmakingCallbacksListener _matchCallbacks = new();
        
        public event UnityAction<List<FriendInfo>> FriendsUpdated
        {
            add => _matchCallbacks.FriendsUpdated += value;
            remove => _matchCallbacks.FriendsUpdated -= value;
        }
        public event UnityAction CreatedRoom
        {
            add => _matchCallbacks.CreatedRoom += value;
            remove => _matchCallbacks.CreatedRoom -= value;
        }
        public event UnityAction JoinedRoom
        {
            add => _matchCallbacks.JoinedRoom += value;
            remove => _matchCallbacks.JoinedRoom -= value;
        }
        public event UnityAction LeftRoom
        {
            add => _matchCallbacks.LeftRoom += value;
            remove => _matchCallbacks.LeftRoom -= value;
        }
        public event UnityAction<short, string> CreateRoomFailed
        { 
            add => _matchCallbacks.CreateRoomFailed += value;
            remove => _matchCallbacks.CreateRoomFailed -= value;
        }
        public event UnityAction<short, string> JoinRoomFailed
        { 
            add => _matchCallbacks.JoinRoomFailed += value;
            remove => _matchCallbacks.JoinRoomFailed -= value;
        }
        public event UnityAction<short, string> JoinRandomFailed
        { 
            add => _matchCallbacks.JoinRandomFailed += value;
            remove => _matchCallbacks.JoinRandomFailed -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_matchCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_matchCallbacks);
        }
    }
}
