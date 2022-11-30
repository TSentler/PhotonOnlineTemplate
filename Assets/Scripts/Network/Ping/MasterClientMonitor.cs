//original script https://pastebin.com/QxavvqRt
//https://www.youtube.com/watch?v=yrB7Hyh2BE4&t=381s

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(PingSender), 
        typeof(PhotonView))]
    public class MasterClientMonitor : MonoBehaviour
    {
        private const int _minimumPingDifference = 50;
        private const float _pingCheckInterval = 5f;
        private const float _takeoverRequestTimeout = 3f;
        
        private float _nextCheckChangeMaster = 0f;
        private float _takeoverRequestTime = -1f;
        private int _consequtiveHighPingCount = 0;
        private bool _isPendingMasterChange = false;
        private PlayersPingList _playersPings;
        private PingSender _pingSender;
        private InRoomCallbackCatcher _enteredCallbackCatcher;
        private PhotonView _photonView;
        
        private void Awake()
        {
            _enteredCallbackCatcher = FindObjectOfType<InRoomCallbackCatcher>();
            _pingSender = GetComponent<PingSender>();
            _playersPings = new PlayersPingList(_pingSender.SendPingInterval);
        }

        public void OnEnable()
        {
            _enteredCallbackCatcher.PlayerLeft += PlayerLeftRoomHandler;
            _enteredCallbackCatcher.MasterSwitched += MasterClientSwitchedHandler;
            _pingSender.PingReceived += _playersPings.PingReceived;
        }

        public void OnDisable()
        {
            _enteredCallbackCatcher.PlayerLeft -= PlayerLeftRoomHandler;
            _enteredCallbackCatcher.MasterSwitched -= MasterClientSwitchedHandler;
            _pingSender.PingReceived -= _playersPings.PingReceived;
        }

        private void Update()
        {
            if (PhotonNetwork.InRoom == false)
                return;
            
            CheckChangeMaster();
            CheckTakeoverTimeout();
        }

        public void LocallyHandOffMasterClient()
        {
            if (!PhotonNetwork.IsConnected 
                || !PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient
                || PhotonNetwork.PlayerList.Length <= 1)
                return;
 
            _playersPings.RemoveNullPlayers();
            _playersPings.CalculateLowestAveragePing(out var lowestPlayer, 
                out var lowestPing, false);

            if (lowestPlayer == null)
                lowestPlayer = _playersPings.GetFirstAnother();
            
            if (lowestPlayer != null)
                SetNewMaster(lowestPlayer);
        }
        
        private void PlayerLeftRoomHandler(Player otherPlayer)
        {
            if (otherPlayer == PhotonNetwork.LocalPlayer)
                return;
            
            _playersPings.Remove(otherPlayer);
        }
 
        private void MasterClientSwitchedHandler(Player newMasterClient)
        {
            _isPendingMasterChange = false;
            _takeoverRequestTime = -1f;
            _consequtiveHighPingCount = 0;
        }
 
        private void CheckTakeoverTimeout()
        {
            if (_takeoverRequestTime == -1f)
                return;

            var takeoverRequestTimePassed =
                Time.unscaledTime - _takeoverRequestTime;
            if (takeoverRequestTimePassed > _takeoverRequestTimeout)
            {
                _takeoverRequestTime = -1f;
                SetNewMaster(PhotonNetwork.LocalPlayer);
            }
        }
 
        private void SetNewMaster(Player newMaster, bool resetHighPingCount = true)
        {
            if (resetHighPingCount)
                _consequtiveHighPingCount = 0;
            
            PhotonNetwork.SetMasterClient(newMaster);
        }
 
        private void CheckChangeMaster()
        {
            if (!PhotonNetwork.IsConnected 
                || !PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient
                || _takeoverRequestTime != -1f
                || Time.time < _nextCheckChangeMaster)
                return;
            
            _nextCheckChangeMaster = Time.time + _pingCheckInterval;
            _playersPings.RemoveNullPlayers();
 
            if (PhotonNetwork.PlayerList.Length <= 1 
                || _playersPings.CheckMyPingIntervalValid() == false)
                return;

            _playersPings.CalculateLowestAveragePing(
                out var lowestAveragePlayer, out var lowestAveragePing);

            if (lowestAveragePlayer == null)
                return;

            var masterPing = _playersPings.GetMasterPing();
            
            if (masterPing == -1
                || lowestAveragePlayer.Equals(PhotonNetwork.LocalPlayer) == false)
                return;
 
            float masterPingDifference = masterPing - lowestAveragePing;
            if (masterPingDifference > _minimumPingDifference)
                _consequtiveHighPingCount++;
            else
                _consequtiveHighPingCount = 0;
 
            if (_consequtiveHighPingCount >= 3)
            {
                _takeoverRequestTime = Time.unscaledTime;
                _photonView.RPC(nameof(RequestMasterClientRPC),
                    RpcTarget.MasterClient, lowestAveragePlayer);
            }
        }


        [PunRPC]
        private void RequestMasterClientRPC(Player requestor)
        {
            if (_isPendingMasterChange 
                || PhotonNetwork.IsMasterClient == false)
                return;
 
            _isPendingMasterChange = true;
            _photonView.RPC(nameof(MasterClientGrantedRPC), requestor);
        }
 
        [PunRPC]
        private void MasterClientGrantedRPC()
        {
            SetNewMaster(PhotonNetwork.LocalPlayer);
        }
    }
}
