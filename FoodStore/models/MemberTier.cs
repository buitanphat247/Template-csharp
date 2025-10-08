using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho cấp độ thành viên trong hệ thống
    /// Định nghĩa các cấp độ khách hàng với mức giảm giá tương ứng
    /// </summary>
    public class MemberTier
    {
        /// <summary>
        /// ID duy nhất của cấp độ thành viên
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên cấp độ thành viên (VD: Bronze, Silver, Gold, Platinum)
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Phần trăm giảm giá dành cho cấp độ thành viên này (0-100)
        /// </summary>
        public float DiscountPercent { get; set; }
    }
}
