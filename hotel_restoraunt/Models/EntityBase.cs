namespace hotel_restoraunt.Models;

   public abstract class EntityBase
   {
       public int Id { get; set; }
       public DateTime CreatedAt { get; set; } = DateTime.Now;
       public DateTime? UpdatedAt { get; set; }
       public bool IsDeleted { get; set; } = false;
   }