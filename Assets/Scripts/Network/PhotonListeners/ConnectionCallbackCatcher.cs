using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectionCallbackCatcher : MonoBehaviour
    {
        private ConnectionCallbackListener _connectionCallbacks = new();
        
        public event UnityAction Connected
        {
            add => _connectionCallbacks.Connected += value;
            remove => _connectionCallbacks.Connected -= value;
        }
        public event UnityAction ConnectedToMaster
        {
            add => _connectionCallbacks.ConnectedToMaster += value;
            remove => _connectionCallbacks.ConnectedToMaster -= value;
        }
        public event UnityAction<DisconnectCause> Disconnnected
        { 
            add => _connectionCallbacks.Disconnnected += value;
            remove => _connectionCallbacks.Disconnnected -= value;
        }
        public event UnityAction<Dictionary<string, object>> AuthResponsed
        {
            add => _connectionCallbacks.AuthResponsed += value;
            remove => _connectionCallbacks.AuthResponsed -= value;
        }
        public event UnityAction<string> AuthFailed
        {
            add => _connectionCallbacks.AuthFailed += value;
            remove => _connectionCallbacks.AuthFailed -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_connectionCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_connectionCallbacks);
        }
    }
}
