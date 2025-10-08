using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho khách hàng trong hệ thống cửa hàng thực phẩm
    /// Lưu trữ thông tin cá nhân, cấp độ thành viên và điểm tích lũy
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// ID duy nhất của khách hàng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ của khách hàng
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Số điện thoại liên hệ của khách hàng
        /// </summary>
        public string Phone { get; set; } = "";

        /// <summary>
        /// ID của cấp độ thành viên (tham chiếu đến MemberTier)
        /// </summary>
        public int TierId { get; set; }

        /// <summary>
        /// Điểm tích lũy của khách hàng từ các giao dịch
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Navigation property - Thông tin chi tiết về cấp độ thành viên
        /// </summary>
        public MemberTier? Tier { get; set; }
    }
}
