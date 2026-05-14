using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CRMApp.Models;
using CRMApp.Services;

namespace CRMApp.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;
        private readonly FileWatcherService _watcher;
        private readonly IAuthService _authService;

        public ObservableCollection<ChatMessage> Messages { get; } = new();

        private string _newMessage = string.Empty;
        public string NewMessage
        {
            get => _newMessage;
            set => SetField(ref _newMessage, value);
        }

        public ICommand SendCommand { get; }
        public ICommand RefreshCommand { get; }

        public ChatViewModel(ChatService chatService, FileWatcherService watcher, IAuthService authService)
        {
            _chatService = chatService;
            _watcher     = watcher;
            _authService = authService;

            SendCommand    = new RelayCommand(Send, () => !string.IsNullOrWhiteSpace(NewMessage));
            RefreshCommand = new RelayCommand(Reload);

            // FileSystemWatcher → обновляем чат при изменении chat.json
            _watcher.ChatDataChanged += () =>
                Application.Current?.Dispatcher.Invoke(Reload);

            Reload();
        }

        private void Reload()
        {
            var msgs = _chatService.GetMessages();
            Messages.Clear();
            foreach (var m in msgs)
                Messages.Add(m);
        }

        private void Send()
        {
            if (string.IsNullOrWhiteSpace(NewMessage)) return;
            var user = _authService.CurrentUser;
            if (user == null) return;

            var msg = new ChatMessage
            {
                SenderUsername = user.Username,
                SenderFullName = user.FullName,
                Text = NewMessage.Trim(),
                SentAt = DateTime.Now
            };

            _chatService.SendMessage(msg);
            NewMessage = string.Empty;

            // Уведомляем другие экземпляры через Named Pipe
            PipeMessenger.Send(new PipeMessage
            {
                Type    = "Chat",
                Sender  = user.Username,
                Payload = msg.Text
            });

            Reload();
        }
    }
}
