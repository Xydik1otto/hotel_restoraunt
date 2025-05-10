// hotel_restoraunt/Data/Repositories/MenuItemRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Repositories
{
    public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(DatabaseContext context) 
            : base(context, "MenuItems", "MenuItemId")
        {
        }
        
        public override async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT mi.*, mc.Name as CategoryName
                    FROM MenuItems mi
                    JOIN MenuCategories mc ON mi.CategoryId = mc.CategoryId
                    ORDER BY mi.CategoryId, mi.Name";
                
                var menuItems = await connection.QueryAsync<MenuItem>(sql);
                return menuItems;
            }
        }
        
        public override async Task<MenuItem> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT mi.*, mc.Name as CategoryName
                    FROM MenuItems mi
                    JOIN MenuCategories mc ON mi.CategoryId = mc.CategoryId
                    WHERE mi.MenuItemId = @Id";
                
                return await connection.QueryFirstOrDefaultAsync<MenuItem>(sql, new { Id = id });
            }
        }
        
        public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(int categoryId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT mi.*, mc.Name as CategoryName
                    FROM MenuItems mi
                    JOIN MenuCategories mc ON mi.CategoryId = mc.CategoryId
                    WHERE mi.CategoryId = @CategoryId
                    ORDER BY mi.Name";
                
                return await connection.QueryAsync<MenuItem>(sql, new { CategoryId = categoryId });
            }
        }
        
        public async Task<IEnumerable<MenuItem>> GetAvailableItemsAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT mi.*, mc.Name as CategoryName
                    FROM MenuItems mi
                    JOIN MenuCategories mc ON mi.CategoryId = mc.CategoryId
                    WHERE mi.IsAvailable = 1
                    ORDER BY mi.CategoryId, mi.Name";
                
                return await connection.QueryAsync<MenuItem>(sql);
            }
        }
    }
}