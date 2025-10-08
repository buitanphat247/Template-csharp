using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho nhân viên trong hệ thống cửa hàng thực phẩm
    /// Lưu trữ thông tin nhân viên và vai trò của họ trong hệ thống
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID duy nhất của nhân viên
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ của nhân viên
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// ID của vai trò nhân viên (tham chiếu đến Role)
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Navigation property - Thông tin chi tiết về vai trò của nhân viên
        /// </summary>
        public Role? Role { get; set; }
    }
}
