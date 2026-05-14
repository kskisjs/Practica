using System;
using System.Linq;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IAuthService
    {
        UserModel? CurrentUser { get; }
        bool IsAuthenticated { get; }

        bool Register(string username, string password, string fullName, UserRole role = UserRole.Manager);
        bool Login(string username, string password);
        void Logout();
    }

    public class AuthService : IAuthService
    {
        private UsersDataStore _store;

        public UserModel? CurrentUser { get; private set; }
        public bool IsAuthenticated => CurrentUser != null;

        public AuthService()
        {
            _store = JsonStorage.Load<UsersDataStore>(JsonStorage.UsersDataPath);

            // Создаём дефолтного администратора, если базы нет
            if (_store.Users.Count == 0)
            {
                SeedDefaultAdmin();
            }
        }

        private void SeedDefaultAdmin()
        {
            var admin = new UserModel
            {
                Id = _store.NextUserId++,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "Администратор системы",
                Role = UserRole.Administrator
            };
            _store.Users.Add(admin);

            var manager = new UserModel
            {
                Id = _store.NextUserId++,
                Username = "manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                FullName = "Менеджер по продажам",
                Role = UserRole.Manager
            };
            _store.Users.Add(manager);

            JsonStorage.Save(JsonStorage.UsersDataPath, _store);
        }

        public bool Register(string username, string password, string fullName, UserRole role = UserRole.Manager)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            if (_store.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            var user = new UserModel
            {
                Id = _store.NextUserId++,
                Username = username.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FullName = fullName.Trim(),
                Role = role
            };

            _store.Users.Add(user);
            JsonStorage.Save(JsonStorage.UsersDataPath, _store);
            return true;
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            var user = _store.Users.FirstOrDefault(
                u => u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return false;

            CurrentUser = user;
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
