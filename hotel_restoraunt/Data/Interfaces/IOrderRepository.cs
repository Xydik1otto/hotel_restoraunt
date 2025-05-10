// hotel_restoraunt/Data/Interfaces/IOrderRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hotel_restoraunt.Models;

namespace hotel_restoraunt.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetOrdersByStatus(string status);
        Task<IEnumerable<Order>> GetOrdersByDate(DateTime date);
        Task<IEnumerable<Order>> GetOrdersByTableId(int tableId);
        Task<IEnumerable<Order>> GetOrdersByGuestId(int guestId);
        Task<int> CreateOrder(Order order, IEnumerable<OrderItem> orderItems);
        Task<bool> UpdateOrderStatus(int id, string status);
        Task<bool> UpdateOrderPaymentStatus(int id, string paymentStatus);
        Task<bool> DeleteOrder(int id);
        Task<IEnumerable<OrderItem>> GetOrderItems(int orderId);
        Task<bool> AddOrderItem(OrderItem item);
        Task<bool> UpdateOrderItem(OrderItem item);
        Task<bool> DeleteOrderItem(int itemId);
    }
}