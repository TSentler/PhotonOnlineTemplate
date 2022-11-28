using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class InRoomCallbackCatcher : MonoBehaviour
    {
        private InRoomCallbackListener _inRoomCallbacks = new();
        
        public event UnityAction<Player> PlayerEntered
        {
            add => _inRoomCallbacks.PlayerEntered += value;
            remove => _inRoomCallbacks.PlayerEntered -= value;
        }
        public event UnityAction<Player> PlayerLeft
        {
            add => _inRoomCallbacks.PlayerLeft += value;
            remove => _inRoomCallbacks.PlayerLeft -= value;
        }
        public event UnityAction<Player> MasterSwitched
        { 
            add => _inRoomCallbacks.MasterSwitched += value;
            remove => _inRoomCallbacks.MasterSwitched -= value;
        }
        public event UnityAction<Player, Hashtable> PlayerPropsUpdated
        {
            add => _inRoomCallbacks.PlayerPropsUpdated += value;
            remove => _inRoomCallbacks.PlayerPropsUpdated -= value;
        }
        public event UnityAction<Hashtable> RoomPropsUpdated
        {
            add => _inRoomCallbacks.RoomPropsUpdated += value;
            remove => _inRoomCallbacks.RoomPropsUpdated -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_inRoomCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_inRoomCallbacks);
        }
    }
}
