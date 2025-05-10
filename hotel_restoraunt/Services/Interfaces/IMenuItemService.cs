// hotel_restoraunt/Services/Interfaces/IMenuItemService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Services.Interfaces
{
    public interface IMenuItemService
    {
        [Obsolete("Використовуйте GetAllMenuItemsAsync() замість цього методу")]
        IEnumerable<MenuItem> GetAllMenuItems();
        
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(int categoryId);
        Task<IEnumerable<MenuItem>> GetAvailableMenuItemsAsync();
        Task<int> AddMenuItemAsync(MenuItem menuItem);
        Task<bool> UpdateMenuItemAsync(MenuItem menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<bool> SetMenuItemAvailabilityAsync(int id, bool isAvailable);
    }
}