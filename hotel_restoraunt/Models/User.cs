// hotel_restoraunt/Models/User.cs
using System;

namespace hotel_restoraunt.Models
{
    public enum UserType
    {
        Admin,
        Manager,
        Staff,
        Guest
    }
    
    public enum UserStatus
    {
        Active,
        Inactive,
        Blocked
    }
    
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType UserType { get; set; } = UserType.Guest;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }
        
        // Властивості для відображення
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string UserTypeDisplay => GetUserTypeDisplay();
        public string StatusDisplay => GetStatusDisplay();
        
        private string GetUserTypeDisplay()
        {
            return UserType switch
            {
                UserType.Admin => "Адміністратор",
                UserType.Manager => "Менеджер",
                UserType.Staff => "Персонал",
                UserType.Guest => "Гість",
                _ => UserType.ToString()
            };
        }
        
        private string GetStatusDisplay()
        {
            return Status switch
            {
                UserStatus.Active => "Активний",
                UserStatus.Inactive => "Неактивний",
                UserStatus.Blocked => "Заблоковано",
                _ => Status.ToString()
            };
        }
    }
}