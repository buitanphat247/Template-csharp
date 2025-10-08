using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho chi tiết đơn hàng
    /// Lưu trữ thông tin từng sản phẩm trong đơn hàng với số lượng, giá và giảm giá
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// ID duy nhất của chi tiết đơn hàng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID của đơn hàng chứa chi tiết này (tham chiếu đến Order)
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// ID của sản phẩm trong chi tiết (tham chiếu đến Product)
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Số lượng sản phẩm được đặt
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Giá đơn vị của sản phẩm tại thời điểm đặt hàng
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Phần trăm giảm giá áp dụng cho sản phẩm này (0-100)
        /// </summary>
        public float DiscountPercent { get; set; }

        /// <summary>
        /// Tổng tiền của chi tiết này (Quantity * UnitPrice * (1 - DiscountPercent/100))
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Navigation property - Thông tin đơn hàng chứa chi tiết này
        /// </summary>
        public Order? Order { get; set; }

        /// <summary>
        /// Navigation property - Thông tin sản phẩm trong chi tiết
        /// </summary>
        public Product? Product { get; set; }
    }
}
