using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho chính sách giảm giá trong hệ thống
    /// Hỗ trợ nhiều loại giảm giá: theo sản phẩm, theo cấp độ thành viên, theo đơn hàng
    /// </summary>
    public class DiscountPolicy
    {
        /// <summary>
        /// ID duy nhất của chính sách giảm giá
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên của chính sách giảm giá
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Loại giảm giá: "Product" (theo sản phẩm), "Tier" (theo cấp độ), "Order" (theo đơn hàng)
        /// </summary>
        public string Type { get; set; } = ""; // Product, Tier, Order

        /// <summary>
        /// ID sản phẩm áp dụng giảm giá (chỉ dùng khi Type = "Product")
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// ID cấp độ thành viên áp dụng giảm giá (chỉ dùng khi Type = "Tier")
        /// </summary>
        public int? TierId { get; set; }

        /// <summary>
        /// Số tiền đơn hàng tối thiểu để áp dụng giảm giá
        /// </summary>
        public decimal MinOrderAmount { get; set; }

        /// <summary>
        /// Phần trăm giảm giá (0-100)
        /// </summary>
        public float DiscountPercent { get; set; }

        /// <summary>
        /// Ngày bắt đầu áp dụng chính sách
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc áp dụng chính sách
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của chính sách (true = đang áp dụng)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property - Thông tin sản phẩm (nếu áp dụng theo sản phẩm)
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Navigation property - Thông tin cấp độ thành viên (nếu áp dụng theo cấp độ)
        /// </summary>
        public MemberTier? Tier { get; set; }
    }
}
