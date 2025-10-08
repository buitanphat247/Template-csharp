using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho chi tiết đơn hàng
    /// </summary>
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public float DiscountPercent { get; set; }
        public decimal Total { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
