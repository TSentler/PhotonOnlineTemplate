using Network;
using UnityEngine;

namespace UI.Network
{
    [RequireComponent(typeof(ConnectToServer))]
    public class ConnectToServerView : MonoBehaviour
    {
        [SerializeField] private GameObject _waitPanel;
        
        private ConnectToServer _connectToServer;

        private void Awake()
        {
            _connectToServer = GetComponent<ConnectToServer>();
        }

        private void OnEnable()
        {
            _connectToServer.ConnectStarted += WaitShow;
            _connectToServer.ConnectEnded += WaitHide;
        }

        private void OnDisable()
        {
            _connectToServer.ConnectStarted -= WaitShow;
            _connectToServer.ConnectEnded -= WaitHide;
        }

        private void WaitShow()
        {
            _waitPanel.SetActive(true);
        }

        private void WaitHide()
        {
            _waitPanel.SetActive(false);
        }
    }
}
