using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho vai trò của nhân viên trong hệ thống cửa hàng thực phẩm
    /// Định nghĩa các chức vụ và quyền hạn của nhân viên
    /// </summary>
    public class Role
    {
        /// <summary>
        /// ID duy nhất của vai trò
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên vai trò (VD: Manager, Cashier, Stock Keeper, Customer Service)
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Mô tả chi tiết về vai trò và quyền hạn
        /// </summary>
        public string Description { get; set; } = "";
    }
}
