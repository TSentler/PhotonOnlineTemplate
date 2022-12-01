using System;
using System.Collections.Generic;
using System.Linq;
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using Packer = Tools.Packer;

namespace Chat
{
    [RequireComponent(typeof(PhotonView))]
    public class ChatMessageSender : MonoBehaviour
    {
        private readonly int _countLastMessages = 50;
        
        [SerializeField] private List<ChatMessage> _messages = new();
        
        private InRoomCallbackCatcher _inRoomCallback;
        private PhotonView _photonView;

        public event UnityAction<ChatMessage> Received;
        public event UnityAction<List<ChatMessage>> Synchronized;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _inRoomCallback = FindObjectOfType<InRoomCallbackCatcher>();
        }

        private void OnEnable()
        {
            _inRoomCallback.PlayerEntered += SyncLastMessages;
        }

        private void OnDisable()
        {
            _inRoomCallback.PlayerEntered -= SyncLastMessages;
        }

        public void SendChatMessage(string text)
        {
            _photonView.RPC(nameof(SendMessageRPC), RpcTarget.All,
                PhotonNetwork.NickName, text);
        }
        
        public void SyncLastMessages(Player player)
        {
            if (PhotonNetwork.IsMasterClient == false)
                return;

            var lastMessages = GetLastMessages();
            byte[] container = Packer.ObjectToByteArray((object)lastMessages);
            
            _photonView.RPC(nameof(SyncMessagesRPC), player, (object)container);
        }

        [PunRPC]
        private void SyncMessagesRPC(object container)
        {
            var messages = (List<ChatMessage>)Packer.
                ByteArrayToObject((byte[])container);
            
            AddSyncMessages(messages);

            Synchronized?.Invoke(messages);
        }

        [PunRPC]
        private void SendMessageRPC(string nick, string text)
        {
            var message = new ChatMessage(nick, text);
            _messages.Add(message);
            Received?.Invoke(message);
        }
        
        private void AddSyncMessages(List<ChatMessage> messages)
        {
            List<ChatMessage> temp = new();
            temp.AddRange(_messages);
            _messages.Clear();
            _messages.AddRange(messages);
            _messages.AddRange(temp);
        }

        private List<ChatMessage> GetLastMessages()
        {
            return _messages.
                Skip(Math.Max(0, _messages.Count - _countLastMessages)).
                ToList();
        }
    }
}
