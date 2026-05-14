using System;

namespace CRMApp.Models
{
    public enum NotificationType
    {
        NewOrder,
        OrderStatusChanged,
        NewClient,
        ChatMessage
    }

    public class AppNotification
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
