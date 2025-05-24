// hotel_restoraunt/Services/Implementations/MenuItemService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Data.Repositories;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        
        public MenuItemService(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            try
            {
                _logger.LogDebug("Отримання всіх пунктів меню");
                return await _unitOfWork.MenuItems.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при отриманні пунктів меню", ex);
                throw;
            }
        }
        
        [Obsolete("Використовуйте GetAllMenuItemsAsync() замість цього методу")]
        public IEnumerable<MenuItem> GetAllMenuItems()
        {
            _logger.LogWarning("Використано застарілий метод GetAllMenuItems()");
            return GetAllMenuItemsAsync().GetAwaiter().GetResult();
        }
        
        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            try
            {
                _logger.LogDebug($"Отримання пункту меню за ID: {id}");
                return await _unitOfWork.MenuItems.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні пункту меню за ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(int categoryId)
        {
            try
            {
                _logger.LogDebug($"Отримання пунктів меню за категорією: {categoryId}");
                
                if (_unitOfWork.MenuItems is MenuItemRepository menuItemRepository)
                {
                    return await menuItemRepository.GetByCategoryAsync(categoryId);
                }
                
                var error = "Не вдалося отримати репозиторій з розширеними методами";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні пунктів меню за категорією: {categoryId}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<MenuItem>> GetAvailableMenuItemsAsync()
        {
            try
            {
                _logger.LogDebug("Отримання доступних пунктів меню");
                
                if (_unitOfWork.MenuItems is MenuItemRepository menuItemRepository)
                {
                    return await menuItemRepository.GetAvailableItemsAsync();
                }
                
                var error = "Не вдалося отримати репозиторій з розширеними методами";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при отриманні доступних пунктів меню", ex);
                throw;
            }
        }
        
        public async Task<int> AddMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                _logger.LogInfo($"Додавання нового пункту меню: {menuItem.Name}");
                
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.MenuItems.AddAsync(menuItem);
                _unitOfWork.Commit();
                
                _logger.LogInfo($"Пункт меню успішно додано з ID: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"Помилка при додаванні пункту меню: {menuItem.Name}", ex);
                throw;
            }
        }
        
        public async Task<bool> UpdateMenuItemAsync(MenuItem menuItem)
        {
            try
            {
                _logger.LogInfo($"Оновлення пункту меню з ID: {menuItem.Id}, назва: {menuItem.Name}");
                
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.MenuItems.UpdateAsync(menuItem);
                _unitOfWork.Commit();
                
                if (result)
                {
                    _logger.LogInfo($"Пункт меню успішно оновлено: {menuItem.Name}");
                }
                else
                {
                    _logger.LogWarning($"Не вдалося оновити пункт меню з ID: {menuItem.Id}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"Помилка при оновленні пункту меню: {menuItem.Name}", ex);
                throw;
            }
        }
        
        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            try
            {
                _logger.LogInfo($"Видалення пункту меню з ID: {id}");
                
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.MenuItems.DeleteAsync(id);
                _unitOfWork.Commit();
                
                if (result)
                {
                    _logger.LogInfo($"Пункт меню успішно видалено, ID: {id}");
                }
                else
                {
                    _logger.LogWarning($"Не вдалося видалити пункт меню з ID: {id}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"Помилка при видаленні пункту меню з ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<bool> SetMenuItemAvailabilityAsync(int id, bool isAvailable)
        {
            try
            {
                _logger.LogInfo($"Зміна доступності пункту меню з ID: {id} на: {(isAvailable ? "доступний" : "недоступний")}");
                
                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(id);
                if (menuItem == null)
                {
                    _logger.LogWarning($"Пункт меню з ID: {id} не знайдено");
                    return false;
                }
                
                menuItem.IsAvailable = isAvailable;
                _unitOfWork.BeginTransaction();
                var result = await _unitOfWork.MenuItems.UpdateAsync(menuItem);
                _unitOfWork.Commit();
                
                if (result)
                {
                    _logger.LogInfo($"Доступність пункту меню успішно змінено, ID: {id}, назва: {menuItem.Name}");
                }
                else
                {
                    _logger.LogWarning($"Не вдалося змінити доступність пункту меню з ID: {id}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"Помилка при зміні доступності пункту меню з ID: {id}", ex);
                throw;
            }
        }
    }
}