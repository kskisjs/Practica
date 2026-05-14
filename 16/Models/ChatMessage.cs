using System;

namespace CRMApp.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public string SenderFullName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
