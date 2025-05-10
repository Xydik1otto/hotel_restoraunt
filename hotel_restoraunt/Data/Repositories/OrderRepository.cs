// hotel_restoraunt/Data/Repositories/OrderRepository.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using MySql.Data.MySqlClient;

namespace hotel_restoraunt.Data.Repositories
{
    public class OrderRepository : DatabaseService, IOrderRepository
    {
        private readonly ILoggerService _logger;
        
        public OrderRepository(ILoggerService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                _logger.LogDebug("Отримання всіх замовлень");
                using (var connection = CreateConnection())
                {
                    var orders = await connection.QueryAsync<Order>("SELECT * FROM Orders ORDER BY OrderDateTime DESC");
                    
                    foreach (var order in orders)
                    {
                        // Завантажуємо елементи замовлення для кожного замовлення
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                    }
                    
                    _logger.LogInfo($"Отримано {orders.Count()} замовлень");
                    return orders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при отриманні всіх замовлень", ex);
                throw;
            }
        }
        
        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                _logger.LogDebug($"Отримання замовлення за ID: {id}");
                using (var connection = CreateConnection())
                {
                    var order = await connection.QueryFirstOrDefaultAsync<Order>(
                        "SELECT * FROM Orders WHERE OrderId = @Id", new { Id = id });
                    
                    if (order != null)
                    {
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                        _logger.LogInfo($"Замовлення з ID: {id} знайдено");
                    }
                    else
                    {
                        _logger.LogWarning($"Замовлення з ID: {id} не знайдено");
                    }
                    
                    return order;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні замовлення за ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByStatus(string status)
        {
            try
            {
                _logger.LogDebug($"Отримання замовлень за статусом: {status}");
                using (var connection = CreateConnection())
                {
                    var orders = await connection.QueryAsync<Order>(
                        "SELECT * FROM Orders WHERE Status = @Status ORDER BY OrderDateTime DESC", 
                        new { Status = status });
                    
                    foreach (var order in orders)
                    {
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                    }
                    
                    _logger.LogInfo($"Отримано {orders.Count()} замовлень зі статусом: {status}");
                    return orders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні замовлень за статусом: {status}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByDate(DateTime date)
        {
            try
            {
                _logger.LogDebug($"Отримання замовлень за датою: {date.ToShortDateString()}");
                using (var connection = CreateConnection())
                {
                    var orders = await connection.QueryAsync<Order>(
                        "SELECT * FROM Orders WHERE DATE(OrderDateTime) = @Date ORDER BY OrderDateTime DESC", 
                        new { Date = date.Date });
                    
                    foreach (var order in orders)
                    {
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                    }
                    
                    _logger.LogInfo($"Отримано {orders.Count()} замовлень за датою: {date.ToShortDateString()}");
                    return orders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні замовлень за датою: {date.ToShortDateString()}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByTableId(int tableId)
        {
            try
            {
                _logger.LogDebug($"Отримання замовлень за ID столика: {tableId}");
                using (var connection = CreateConnection())
                {
                    var orders = await connection.QueryAsync<Order>(
                        "SELECT * FROM Orders WHERE TableId = @TableId ORDER BY OrderDateTime DESC", 
                        new { TableId = tableId });
                    
                    foreach (var order in orders)
                    {
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                    }
                    
                    _logger.LogInfo($"Отримано {orders.Count()} замовлень за ID столика: {tableId}");
                    return orders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні замовлень за ID столика: {tableId}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<Order>> GetOrdersByGuestId(int guestId)
        {
            try
            {
                _logger.LogDebug($"Отримання замовлень за ID гостя: {guestId}");
                using (var connection = CreateConnection())
                {
                    var orders = await connection.QueryAsync<Order>(
                        "SELECT * FROM Orders WHERE GuestId = @GuestId ORDER BY OrderDateTime DESC", 
                        new { GuestId = guestId });
                    
                    foreach (var order in orders)
                    {
                        var items = await GetOrderItems(order.OrderId);
                        order.Items = items.ToList();
                    }
                    
                    _logger.LogInfo($"Отримано {orders.Count()} замовлень за ID гостя: {guestId}");
                    return orders;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні замовлень за ID гостя: {guestId}", ex);
                throw;
            }
        }
        
        public async Task<int> CreateOrder(Order order, IEnumerable<OrderItem> orderItems)
        {
            try
            {
                _logger.LogInfo($"Створення нового замовлення для столика: {order.TableId}, гостя: {order.GuestId}");
                using (var connection = CreateConnection() as MySqlConnection)
                {
                    // Відкриваємо транзакцію, щоб гарантувати цілісність даних
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Створюємо замовлення
                            var orderSql = @"
                                INSERT INTO Orders (TableId, GuestId, Status, TotalAmount, PaymentStatus, PaymentMethod, Notes)
                                VALUES (@TableId, @GuestId, @Status, @TotalAmount, @PaymentStatus, @PaymentMethod, @Notes);
                                SELECT LAST_INSERT_ID();";
                            
                            var orderId = await connection.ExecuteScalarAsync<int>(orderSql, order, transaction);
                            _logger.LogDebug($"Створено замовлення з ID: {orderId}");
                            
                            // Додаємо елементи замовлення
                            var itemSql = @"
                                INSERT INTO OrderItems (OrderId, MenuItemId, Quantity, UnitPrice, SubTotal, Notes)
                                VALUES (@OrderId, @MenuItemId, @Quantity, @UnitPrice, @SubTotal, @Notes);";
                            
                            foreach (var item in orderItems)
                            {
                                item.OrderId = orderId;
                                await connection.ExecuteAsync(itemSql, item, transaction);
                                _logger.LogDebug($"Додано елемент замовлення: {item.MenuItemId}, кількість: {item.Quantity}");
                            }
                            
                            // Підтверджуємо транзакцію
                            transaction.Commit();
                            _logger.LogInfo($"Замовлення успішно створено з ID: {orderId}");
                            
                            return orderId;
                        }
                        catch (Exception ex)
                        {
                            // Відкат транзакції у випадку помилки
                            transaction.Rollback();
                            _logger.LogError("Помилка при створенні замовлення, транзакцію відкачено", ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при створенні замовлення", ex);
                throw;
            }
        }
        
        public async Task<bool> UpdateOrderStatus(int id, string status)
        {
            try
            {
                _logger.LogInfo($"Оновлення статусу замовлення з ID: {id} на: {status}");
                using (var connection = CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(
                        "UPDATE Orders SET Status = @Status WHERE OrderId = @Id", 
                        new { Id = id, Status = status });
                    
                    if (rowsAffected > 0)
                    {
                        _logger.LogInfo($"Статус замовлення успішно оновлено, ID: {id}, новий статус: {status}");
                    }
                    else
                    {
                        _logger.LogWarning($"Не вдалося оновити статус замовлення з ID: {id}");
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні статусу замовлення з ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<bool> UpdateOrderPaymentStatus(int id, string paymentStatus)
        {
            try
            {
                _logger.LogInfo($"Оновлення статусу оплати замовлення з ID: {id} на: {paymentStatus}");
                using (var connection = CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(
                        "UPDATE Orders SET PaymentStatus = @PaymentStatus WHERE OrderId = @Id", 
                        new { Id = id, PaymentStatus = paymentStatus });
                    
                    if (rowsAffected > 0)
                    {
                        _logger.LogInfo($"Статус оплати замовлення успішно оновлено, ID: {id}, новий статус: {paymentStatus}");
                    }
                    else
                    {
                        _logger.LogWarning($"Не вдалося оновити статус оплати замовлення з ID: {id}");
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні статусу оплати замовлення з ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<bool> DeleteOrder(int id)
        {
            try
            {
                _logger.LogInfo($"Видалення замовлення з ID: {id}");
                using (var connection = CreateConnection() as MySqlConnection)
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Спочатку видаляємо пов'язані елементи замовлення
                            await connection.ExecuteAsync(
                                "DELETE FROM OrderItems WHERE OrderId = @Id", 
                                new { Id = id }, transaction);
                            _logger.LogDebug($"Видалено елементи замовлення для замовлення з ID: {id}");
                            
                            // Потім видаляємо саме замовлення
                            int rowsAffected = await connection.ExecuteAsync(
                                "DELETE FROM Orders WHERE OrderId = @Id", 
                                new { Id = id }, transaction);
                            
                            transaction.Commit();
                            
                            if (rowsAffected > 0)
                            {
                                _logger.LogInfo($"Замовлення успішно видалено, ID: {id}");
                            }
                            else
                            {
                                _logger.LogWarning($"Не вдалося видалити замовлення з ID: {id}");
                            }
                            
                            return rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"Помилка при видаленні замовлення з ID: {id}, транзакцію відкачено", ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при видаленні замовлення з ID: {id}", ex);
                throw;
            }
        }
        
        public async Task<IEnumerable<OrderItem>> GetOrderItems(int orderId)
        {
            try
            {
                _logger.LogDebug($"Отримання елементів замовлення для замовлення з ID: {orderId}");
                using (var connection = CreateConnection())
                {
                    var sql = @"
                        SELECT oi.*, mi.Name as ItemName
                        FROM OrderItems oi
                        JOIN MenuItems mi ON oi.MenuItemId = mi.MenuItemId
                        WHERE oi.OrderId = @OrderId";
                    
                    var items = await connection.QueryAsync<OrderItem>(sql, new { OrderId = orderId });
                    _logger.LogDebug($"Отримано {items.Count()} елементів для замовлення з ID: {orderId}");
                    return items;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при отриманні елементів замовлення для замовлення з ID: {orderId}", ex);
                throw;
            }
        }
        
        public async Task<bool> AddOrderItem(OrderItem item)
        {
            try
            {
                _logger.LogInfo($"Додавання елемента до замовлення з ID: {item.OrderId}, пункт меню: {item.MenuItemId}");
                using (var connection = CreateConnection())
                {
                    var sql = @"
                        INSERT INTO OrderItems (OrderId, MenuItemId, Quantity, UnitPrice, SubTotal, Notes)
                        VALUES (@OrderId, @MenuItemId, @Quantity, @UnitPrice, @SubTotal, @Notes);";
                    
                    int rowsAffected = await connection.ExecuteAsync(sql, item);
                    
                    if (rowsAffected > 0)
                    {
                        // Оновлюємо загальну суму замовлення
                        await UpdateOrderTotal(item.OrderId);
                        _logger.LogInfo($"Елемент успішно додано до замовлення з ID: {item.OrderId}");
                    }
                    else
                    {
                        _logger.LogWarning($"Не вдалося додати елемент до замовлення з ID: {item.OrderId}");
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при додаванні елемента до замовлення з ID: {item.OrderId}", ex);
                throw;
            }
        }
        
        public async Task<bool> UpdateOrderItem(OrderItem item)
        {
            try
            {
                _logger.LogInfo($"Оновлення елемента замовлення з ID: {item.OrderItemId}, замовлення: {item.OrderId}");
                using (var connection = CreateConnection())
                {
                    var sql = @"
                        UPDATE OrderItems 
                        SET Quantity = @Quantity, 
                            UnitPrice = @UnitPrice, 
                            SubTotal = @SubTotal, 
                            Notes = @Notes
                        WHERE OrderItemId = @OrderItemId";
                    
                    int rowsAffected = await connection.ExecuteAsync(sql, item);
                    
                    if (rowsAffected > 0)
                    {
                        // Оновлюємо загальну суму замовлення
                        await UpdateOrderTotal(item.OrderId);
                        _logger.LogInfo($"Елемент замовлення успішно оновлено, ID: {item.OrderItemId}");
                    }
                    else
                    {
                        _logger.LogWarning($"Не вдалося оновити елемент замовлення з ID: {item.OrderItemId}");
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні елемента замовлення з ID: {item.OrderItemId}", ex);
                throw;
            }
        }
        
        public async Task<bool> DeleteOrderItem(int itemId)
        {
            try
            {
                _logger.LogInfo($"Видалення елемента замовлення з ID: {itemId}");
                using (var connection = CreateConnection())
                {
                    // Спочатку отримуємо OrderId для подальшого оновлення загальної суми
                    var orderItem = await connection.QueryFirstOrDefaultAsync<OrderItem>(
                        "SELECT OrderId FROM OrderItems WHERE OrderItemId = @Id", new { Id = itemId });
                    
                    if (orderItem == null)
                    {
                        _logger.LogWarning($"Елемент замовлення з ID: {itemId} не знайдено");
                        return false;
                    }
                    
                    int rowsAffected = await connection.ExecuteAsync(
                        "DELETE FROM OrderItems WHERE OrderItemId = @Id", new { Id = itemId });
                    
                    if (rowsAffected > 0)
                    {
                        // Оновлюємо загальну суму замовлення
                        await UpdateOrderTotal(orderItem.OrderId);
                        _logger.LogInfo($"Елемент замовлення успішно видалено, ID: {itemId}");
                    }
                    else
                    {
                        _logger.LogWarning($"Не вдалося видалити елемент замовлення з ID: {itemId}");
                    }
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при видаленні елемента замовлення з ID: {itemId}", ex);
                throw;
            }
        }
        
        private async Task UpdateOrderTotal(int orderId)
        {
            try
            {
                _logger.LogDebug($"Оновлення загальної суми замовлення з ID: {orderId}");
                using (var connection = CreateConnection())
                {
                    // Обчислюємо нову загальну суму замовлення
                    var total = await connection.ExecuteScalarAsync<decimal>(
                        "SELECT SUM(SubTotal) FROM OrderItems WHERE OrderId = @OrderId", 
                        new { OrderId = orderId });
                    
                    // Оновлюємо загальну суму в замовленні
                    await connection.ExecuteAsync(
                        "UPDATE Orders SET TotalAmount = @Total WHERE OrderId = @OrderId", 
                        new { OrderId = orderId, Total = total });
                    
                    _logger.LogDebug($"Загальна сума замовлення з ID: {orderId} оновлена до: {total}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при оновленні загальної суми замовлення з ID: {orderId}", ex);
                throw;
            }
        }
    }
}