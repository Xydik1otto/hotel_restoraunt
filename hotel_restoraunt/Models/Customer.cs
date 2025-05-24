using System.ComponentModel.DataAnnotations;

namespace hotel_restoraunt.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int VisitCount { get; set; } = 0;
    }
}