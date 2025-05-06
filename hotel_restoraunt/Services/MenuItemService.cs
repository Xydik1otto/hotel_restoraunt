// Services/Implementations/MenuItemService.cs
using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItems()
        {
            var sql = """
            SELECT m.*, c.category_name 
            FROM restoraunt_menu_items m
            JOIN restoraunt_menu_categories c ON m.category_id = c.category_id
            """;
            
            return await _unitOfWork.Connection.QueryAsync<MenuItem, MenuCategory, MenuItem>(sql,
                (item, category) =>
                {
                    item.Category = category;
                    return item;
                },
                splitOn: "category_name");
        }

        public async Task<MenuItem?> GetMenuItemById(int id)
        {
            var sql = """
            SELECT m.*, c.category_name 
            FROM restoraunt_menu_items m
            JOIN restoraunt_menu_categories c ON m.category_id = c.category_id
            WHERE m.item_id = @Id
            """;
            
            var result = await _unitOfWork.Connection.QueryAsync<MenuItem, MenuCategory, MenuItem>(sql,
                (item, category) =>
                {
                    item.Category = category;
                    return item;
                },
                new { Id = id },
                splitOn: "category_name");
                
            return result.FirstOrDefault();
        }

        public async Task CreateMenuItem(MenuItem item)
        {
            var sql = """
            INSERT INTO restoraunt_menu_items 
            (category_id, item_name, description, price, is_available) 
            VALUES 
            (@CategoryId, @ItemName, @Description, @Price, @IsAvailable)
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, item, _unitOfWork.Transaction);
        }

        public async Task UpdateMenuItem(MenuItem item)
        {
            var sql = """
            UPDATE restoraunt_menu_items SET 
            category_id = @CategoryId,
            item_name = @ItemName,
            description = @Description,
            price = @Price,
            is_available = @IsAvailable
            WHERE item_id = @ItemId
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, item, _unitOfWork.Transaction);
        }

        public async Task ToggleMenuItemAvailability(int id)
        {
            var sql = """
            UPDATE restoraunt_menu_items 
            SET is_available = NOT is_available 
            WHERE item_id = @Id
            """;
            
            await _unitOfWork.Connection.ExecuteAsync(sql, new { Id = id }, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<MenuItem>> GetAvailableMenuItems()
        {
            var sql = """
            SELECT * FROM restoraunt_menu_items 
            WHERE is_available = TRUE
            ORDER BY item_name
            """;
            
            return await _unitOfWork.Connection.QueryAsync<MenuItem>(sql);
        }
    }
}