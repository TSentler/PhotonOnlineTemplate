using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class InRoomCallbackListener : IInRoomCallbacks
    {
        public event UnityAction<Player> PlayerEntered, PlayerLeft, MasterSwitched;
        public event UnityAction<Player, Hashtable> PlayerPropsUpdated;
        public event UnityAction<Hashtable> RoomPropsUpdated;

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            PlayerEntered?.Invoke(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            PlayerLeft?.Invoke(otherPlayer);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            RoomPropsUpdated?.Invoke(propertiesThatChanged);
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, 
            Hashtable changedProps)
        {
            PlayerPropsUpdated?.Invoke(targetPlayer, changedProps);
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            MasterSwitched?.Invoke(newMasterClient);
        }
    }
}
