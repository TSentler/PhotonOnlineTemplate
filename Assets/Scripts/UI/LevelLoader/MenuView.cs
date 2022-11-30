using System;
using System.Linq;
using LevelLoaders;
using TMPro;
using UnityEngine;

namespace UI.Network
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _joinDropdown;
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private RoomsInfo _roomsInfo;

        public string JoinRoomName =>
            _joinDropdown.options.Count == 0
                ? String.Empty
                : _joinDropdown.options[_joinDropdown.value].text;

        private void Awake()
        {
            _joinDropdown.ClearOptions();
        }

        private void OnEnable()
        {
            _roomsInfo.RoomListUpdated += RoomUpdate;
        }

        private void OnDisable()
        {
            _roomsInfo.RoomListUpdated -= RoomUpdate;
        }

        public void OnJoinClick()
        {
            _levelLoader.JoinRoom(JoinRoomName);
        }
        
        private void RoomUpdate(string[] roomNames)
        {
            _joinDropdown.ClearOptions();
            if (roomNames.Length == 0)
            {
                Debug.Log("rooms empty");
            }
            else
            {
                _joinDropdown.AddOptions(roomNames.ToList());
            }
        }
    }
}
