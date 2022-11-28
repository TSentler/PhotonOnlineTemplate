using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviour
    {
        private Coroutine _connectCoroutine;
        private ConnectionCallbackCatcher _connectCallback;
        
        public event UnityAction ConnectStarted, ConnectEnded, OnDisconnect;
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _connectCallback = FindObjectOfType<ConnectionCallbackCatcher>();
        }

        private void OnEnable()
        {
            _connectCallback.ConnectedToMaster += ConnectedToMasterHandler;
            _connectCallback.Disconnnected += DisconnectHandler;
        }

        private void OnDisable()
        {
            _connectCallback.ConnectedToMaster -= ConnectedToMasterHandler;
            _connectCallback.Disconnnected -= DisconnectHandler;
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnectedAndReady)
                return;

            Debug.Log("TryConnect");
            ConnectStarted?.Invoke();
            PhotonNetwork.ConnectUsingSettings();
        }

        private void ConnectedToMasterHandler()
        {
            PhotonNetwork.JoinLobby();
            ConnectEnded?.Invoke();
        }

        private void DisconnectHandler(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            OnDisconnect?.Invoke();
            Connect();
        }
    }
}
