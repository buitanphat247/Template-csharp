using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho sản phẩm trong hệ thống
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Status { get; set; } = "Active";
    }
}
