using System;
using System.Collections.Generic;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho đơn hàng trong hệ thống cửa hàng thực phẩm
    /// Lưu trữ thông tin đơn hàng, khách hàng, nhân viên xử lý và chi tiết sản phẩm
    /// </summary>
    public class Order
    {
        /// <summary>
        /// ID duy nhất của đơn hàng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID của khách hàng đặt hàng (tham chiếu đến Customer)
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// ID của nhân viên xử lý đơn hàng (tham chiếu đến Employee)
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Tổng số tiền của đơn hàng (đã bao gồm giảm giá nếu có)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Trạng thái đơn hàng: "Pending", "Processing", "Completed", "Cancelled"
        /// </summary>
        public string Status { get; set; } = "Pending";

        /// <summary>
        /// Thời gian tạo đơn hàng
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Navigation property - Thông tin khách hàng đặt hàng
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Navigation property - Thông tin nhân viên xử lý đơn hàng
        /// </summary>
        public Employee? Employee { get; set; }

        /// <summary>
        /// Navigation property - Danh sách chi tiết sản phẩm trong đơn hàng
        /// </summary>
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
