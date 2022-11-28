using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class ConnectionCallbackListener : IConnectionCallbacks
    {
        public event UnityAction Connected, ConnectedToMaster;
        public event UnityAction<DisconnectCause> Disconnnected;
        public event UnityAction<RegionHandler> RegionsReceived;
        public event UnityAction<Dictionary<string, object>> AuthResponsed;
        public event UnityAction<string> AuthFailed;

        public void OnConnected()
        {
            Connected?.Invoke();
        }

        public void OnConnectedToMaster()
        {
            ConnectedToMaster?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Disconnnected?.Invoke(cause);
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            RegionsReceived?.Invoke(regionHandler);
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            AuthResponsed?.Invoke(data);
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            AuthFailed?.Invoke(debugMessage);
        }
    }
}
