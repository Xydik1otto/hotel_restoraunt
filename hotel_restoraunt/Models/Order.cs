// hotel_restoraunt/Models/Order.cs
using System;
using System.Collections.Generic;

namespace hotel_restoraunt.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public int? GuestId { get; set; }
        public DateTime OrderDateTime { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Новий";
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Не оплачено";
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        
        // Навігаційні властивості
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
        // Додаткові властивості для відображення
        public string FormattedOrderDate => OrderDateTime.ToString("dd.MM.yyyy HH:mm");
        public string StatusDisplay => GetStatusDisplay();
        public string PaymentStatusDisplay => GetPaymentStatusDisplay();
        
        private string GetStatusDisplay()
        {
            return Status switch
            {
                "New" => "Новий",
                "InProgress" => "В процесі",
                "Ready" => "Готовий",
                "Delivered" => "Доставлений",
                "Completed" => "Завершений",
                "Cancelled" => "Скасований",
                _ => Status
            };
        }
        
        private string GetPaymentStatusDisplay()
        {
            return PaymentStatus switch
            {
                "Unpaid" => "Не оплачено",
                "PartiallyPaid" => "Частково оплачено",
                "Paid" => "Оплачено",
                "Refunded" => "Повернуто",
                _ => PaymentStatus
            };
        }
    }
    
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string Notes { get; set; }
        
        // Властивість для відображення
        public string ItemName { get; set; }
    }
}