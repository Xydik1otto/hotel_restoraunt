using System.ComponentModel.DataAnnotations;

namespace hotel_restoraunt.Models
{
    public class Guest
    {
        public int GuestId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string PassportNumber { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}