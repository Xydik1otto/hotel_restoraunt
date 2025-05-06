namespace hotel_restoraunt.Models
{
    public class User
    {
        public int UserId { get; set; }  // Змінено з Id на UserId
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}