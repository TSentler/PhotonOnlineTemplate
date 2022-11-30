using System.Collections;
using System.Net;
using Agava.WebUtility;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(MasterClientMonitor),
        typeof(PingSender))]
    public class BackgroundMasterSwitcher : MonoBehaviour
    {
        private readonly float _repeatRequestDelay = 3f;
        
        private MasterClientMonitor _monitor;
        private PingSender _pingSender;
        private Coroutine _coroutine;
        private InRoomCallbackCatcher _enteredCallbackCatcher;

        private void Awake()
        {
            _enteredCallbackCatcher = FindObjectOfType<InRoomCallbackCatcher>();
            _monitor = GetComponent<MasterClientMonitor>();
            _pingSender = GetComponent<PingSender>();
        }

        private void OnEnable()
        {
            _enteredCallbackCatcher.MasterSwitched += OnMasterSwitched;
            WebApplication.InBackgroundChangeEvent += OnBackgroundChanged;
        }

        private void OnDisable()
        {
            _enteredCallbackCatcher.MasterSwitched -= OnMasterSwitched;
            WebApplication.InBackgroundChangeEvent -= OnBackgroundChanged;
        }

        private void OnMasterSwitched(Player player)
        {
            SwitchBackgroundMaster();
        }

        private void OnBackgroundChanged(bool isBack)
        {
            SwitchBackgroundMaster();
        }

        private void SwitchBackgroundMaster()
        {
            if (WebApplication.InBackground
                && PhotonNetwork.IsMasterClient && _coroutine == null)
            {
                _coroutine = StartCoroutine(
                    SwitchBackgroundMasterCoroutine());
            }
        }

        private IEnumerator SwitchBackgroundMasterCoroutine()
        {
            while (WebApplication.InBackground && PhotonNetwork.IsMasterClient)
            {
                _pingSender.SendPingImmediate();
                _monitor.LocallyHandOffMasterClient();
                yield return new WaitForSeconds(_repeatRequestDelay);
            }
            _coroutine = null;
        }
    }
}
