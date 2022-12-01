using Network;
using Photon.Realtime;
using UnityEngine;

namespace Chat
{
    public class ChatSynchronizer : MonoBehaviour
    {
        private ChatMessageSender _sender;
        
        private void Awake()
        {
            _sender = GetComponent<ChatMessageSender>();
        }

    }
}
