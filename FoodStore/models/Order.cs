using System;
using System.Collections.Generic;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho đơn hàng trong hệ thống
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Customer? Customer { get; set; }
        public Employee? Employee { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
