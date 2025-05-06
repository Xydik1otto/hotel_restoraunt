// Services/Interfaces/IMenuItemService.cs
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItems();
        Task<MenuItem?> GetMenuItemById(int id);
        Task CreateMenuItem(MenuItem item);
        Task UpdateMenuItem(MenuItem item);
        Task ToggleMenuItemAvailability(int id);
        Task<IEnumerable<MenuItem>> GetAvailableMenuItems();
    }
}