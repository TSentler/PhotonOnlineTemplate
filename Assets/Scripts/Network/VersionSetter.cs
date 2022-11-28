using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class VersionSetter : MonoBehaviour
    {
        [SerializeField] private string _version;

        public string Version => _version;
        
        private void Awake()
        {
            PhotonNetwork.GameVersion = _version;
        }
    }
}
