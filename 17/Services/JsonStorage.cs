using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CRMApp.Models;

namespace CRMApp.Services
{
    /// <summary>
    /// Корневой объект JSON-хранилища crm_data.json
    /// </summary>
    public class CrmDataStore
    {
        public List<ClientModel> Clients { get; set; } = new();
        public List<OrderModel> Orders { get; set; } = new();
        public int NextClientId { get; set; } = 1;
        public int NextOrderId { get; set; } = 1;
    }

    /// <summary>
    /// Корневой объект JSON-хранилища users.json
    /// </summary>
    public class UsersDataStore
    {
        public List<UserModel> Users { get; set; } = new();
        public int NextUserId { get; set; } = 1;
    }

    /// <summary>
    /// Корневой объект JSON-хранилища chat.json
    /// </summary>
    public class ChatDataStore
    {
        public List<ChatMessage> Messages { get; set; } = new();
        public int NextMessageId { get; set; } = 1;
    }

    /// <summary>
    /// Вспомогательный класс для работы с JSON-файлами
    /// </summary>
    public static class JsonStorage
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static T Load<T>(string path) where T : new()
        {
            try
            {
                if (!File.Exists(path))
                    return new T();

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json, _options) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public static void Save<T>(string path, T data)
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(data, _options);

            // Атомарная запись: сначала во временный файл, потом rename
            var tmp = path + ".tmp";
            File.WriteAllText(tmp, json);
            File.Move(tmp, path, overwrite: true);
        }

        /// <summary>
        /// Путь к папке данных приложения (рядом с .exe)
        /// </summary>
        public static string DataFolder =>
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data");

        public static string CrmDataPath => Path.Combine(DataFolder, "crm_data.json");
        public static string UsersDataPath => Path.Combine(DataFolder, "users.json");
        public static string ChatDataPath => Path.Combine(DataFolder, "chat.json");
        public static string NotificationsPath => Path.Combine(DataFolder, "notifications.json");
    }
}
