using System;
using System.IO;

namespace CRMApp.Services
{
    /// <summary>
    /// Отслеживает изменения файлов данных через FileSystemWatcher.
    /// При изменении crm_data.json или chat.json поднимает события.
    /// </summary>
    public class FileWatcherService : IDisposable
    {
        private FileSystemWatcher? _crmWatcher;
        private FileSystemWatcher? _chatWatcher;

        public event Action? CrmDataChanged;
        public event Action? ChatDataChanged;

        public void Start()
        {
            var folder = JsonStorage.DataFolder;
            Directory.CreateDirectory(folder);

            _crmWatcher = CreateWatcher(folder, "crm_data.json", () => CrmDataChanged?.Invoke());
            _chatWatcher = CreateWatcher(folder, "chat.json",     () => ChatDataChanged?.Invoke());
        }

        private static FileSystemWatcher CreateWatcher(string folder, string filter, Action handler)
        {
            var watcher = new FileSystemWatcher(folder, filter)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            // Debounce: избегаем двойных срабатываний при атомарной записи
            DateTime lastFired = DateTime.MinValue;

            watcher.Changed += (_, _) =>
            {
                var now = DateTime.Now;
                if ((now - lastFired).TotalMilliseconds < 400) return;
                lastFired = now;
                handler();
            };

            return watcher;
        }

        public void Dispose()
        {
            _crmWatcher?.Dispose();
            _chatWatcher?.Dispose();
        }
    }
}
