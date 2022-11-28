using System;
using System.Collections.Generic;
using System.Linq;
using Network;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace UI.Network
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _create;
        [SerializeField] private TMP_Dropdown _join;
        
        private LobbyCallbackCatcher _lobbyCallbacks;

        public string CreateRoomName => _create.text;

        public string JoinRoomName =>
            _join.options.Count == 0
                ? String.Empty
                : _join.options[_join.value].text;

        private void Awake()
        {
            _lobbyCallbacks = FindObjectOfType<LobbyCallbackCatcher>();
            _create.text = "Room1";
            _join.ClearOptions();
        }

        private void OnEnable()
        {
            _lobbyCallbacks.RoomListUpdated += RoomUpdate;
        }

        private void OnDisable()
        {
            _lobbyCallbacks.RoomListUpdated -= RoomUpdate;
        }

        private void RoomUpdate(List<RoomInfo> roomInfos)
        {
            _join.ClearOptions();
            if (roomInfos.Count == 0)
            {
                Debug.Log("roomInfos empty");
            }
            else
            {
                var names = roomInfos.Select(info => info.Name).ToList();
                _join.AddOptions(names);
            }
        }
    }
}
