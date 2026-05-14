using System.Collections.Generic;
using System.Linq;
using CRMApp.Models;

namespace CRMApp.Services
{
    public class ChatService
    {
        private ChatDataStore _store;

        public ChatService()
        {
            _store = Load();
        }

        private static ChatDataStore Load()
            => JsonStorage.Load<ChatDataStore>(JsonStorage.ChatDataPath);

        public List<ChatMessage> GetMessages(int lastN = 100)
        {
            _store = Load();
            return _store.Messages
                .OrderBy(m => m.SentAt)
                .TakeLast(lastN)
                .ToList();
        }

        public void SendMessage(ChatMessage msg)
        {
            _store = Load(); // перечитываем перед записью
            msg.Id = _store.NextMessageId++;
            _store.Messages.Add(msg);
            JsonStorage.Save(JsonStorage.ChatDataPath, _store);
        }
    }
}
