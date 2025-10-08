using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho khách hàng trong hệ thống
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public int TierId { get; set; }
        public int Points { get; set; }

        // Navigation property
        public MemberTier? Tier { get; set; }
    }
}
