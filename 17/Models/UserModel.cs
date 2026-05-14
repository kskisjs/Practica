using System.Text.Json.Serialization;

namespace CRMApp.Models
{
    public enum UserRole
    {
        Manager,
        Administrator
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Manager;

        [JsonIgnore]
        public bool IsAdmin => Role == UserRole.Administrator;
    }
}
