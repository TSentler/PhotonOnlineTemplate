using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Chat
{
    public class ChatView : MonoBehaviour
    {
        [SerializeField] private Transform _rootMessages;
        [SerializeField] private GameObject _message;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Button _button;

        private ChatMessageSender _chatMessageSender;

        private bool IsSendReady => _input.text != string.Empty;

        private void OnValidate()
        {
            if (_message == null 
                || _message.TryGetComponent(out MessageText _text) == false)
            {
                Debug.Log(nameof(MessageText) + 
                          " was not found", this);
                _message = null;
            }
        }

        private void Awake()
        {
            _chatMessageSender = FindObjectOfType<ChatMessageSender>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnSended);
            _input.onSubmit.AddListener(OnSended);
            _chatMessageSender.Received += AddMessage;
            _chatMessageSender.Synchronized += OnSychronized;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnSended);
            _input.onSubmit.RemoveListener(OnSended);
            _chatMessageSender.Received -= AddMessage;
            _chatMessageSender.Synchronized -= OnSychronized;
        }

        private void OnSychronized(List<ChatMessage> chatMessages)
        {
            Clear();
            foreach (var message in chatMessages)
            {
                AddMessage(message);
            }
        }

        private void OnSended()
        {
            OnSended(_input.text);
        }
        
        private void OnSended(string text)
        {
            if (IsSendReady == false)
                return;
            
            _input.text = "";
            _chatMessageSender.SendChatMessage(text);
        }

        private void AddMessage(ChatMessage chatMessage)
        {
            var message = Instantiate(_message, _rootMessages);
            message.GetComponent<MessageText>().SetMessage(
                chatMessage.Nick, chatMessage.Text);
        }

        private void Clear()
        {
            foreach (Transform child in _rootMessages)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
