using System.Collections.Generic;
using System.Linq;
using CRMApp.Models;

namespace CRMApp.Services
{
    public class NotificationStore
    {
        public List<AppNotification> Notifications { get; set; } = new();
        public int NextId { get; set; } = 1;
    }

    public class NotificationService
    {
        private NotificationStore _store;

        public NotificationService()
        {
            _store = JsonStorage.Load<NotificationStore>(JsonStorage.NotificationsPath);
        }

        public List<AppNotification> GetAll()
        {
            _store = JsonStorage.Load<NotificationStore>(JsonStorage.NotificationsPath);
            return _store.Notifications.OrderByDescending(n => n.CreatedAt).ToList();
        }

        public int UnreadCount => GetAll().Count(n => !n.IsRead);

        public void Add(AppNotification notification)
        {
            _store = JsonStorage.Load<NotificationStore>(JsonStorage.NotificationsPath);
            notification.Id = _store.NextId++;
            _store.Notifications.Insert(0, notification);
            // Оставляем не более 200 уведомлений
            if (_store.Notifications.Count > 200)
                _store.Notifications.RemoveAt(_store.Notifications.Count - 1);
            JsonStorage.Save(JsonStorage.NotificationsPath, _store);
        }

        public void MarkAllRead()
        {
            _store = JsonStorage.Load<NotificationStore>(JsonStorage.NotificationsPath);
            foreach (var n in _store.Notifications)
                n.IsRead = true;
            JsonStorage.Save(JsonStorage.NotificationsPath, _store);
        }
    }
}
