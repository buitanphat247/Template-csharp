using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho chính sách giảm giá
    /// </summary>
    public class DiscountPolicy
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // Product, Tier, Order
        public int? ProductId { get; set; }
        public int? TierId { get; set; }
        public decimal MinOrderAmount { get; set; }
        public float DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Product? Product { get; set; }
        public MemberTier? Tier { get; set; }
    }
}
