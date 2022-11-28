using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class MatchmakingCallbacksListener : IMatchmakingCallbacks
    {
        public event UnityAction<List<FriendInfo>> FriendsUpdated;
        public event UnityAction CreatedRoom, JoinedRoom, LeftRoom;
        public event UnityAction<short, string> CreateRoomFailed,
            JoinRoomFailed, JoinRandomFailed;

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            FriendsUpdated?.Invoke(friendList);
        }

        public void OnCreatedRoom()
        {
            CreatedRoom?.Invoke();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            CreateRoomFailed?.Invoke(returnCode, message);
        }

        public void OnJoinedRoom()
        {
            JoinedRoom?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            JoinRoomFailed?.Invoke(returnCode, message);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            JoinRandomFailed?.Invoke(returnCode, message);
        }

        public void OnLeftRoom()
        {
            LeftRoom?.Invoke();
        }
    }
}
