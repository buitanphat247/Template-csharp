using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho cấp độ thành viên
    /// </summary>
    public class MemberTier
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public float DiscountPercent { get; set; }
    }
}
