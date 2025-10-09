using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;

namespace FoodStore.Utils
{
    /// <summary>
    /// Formatter chuyên dụng để tính toán và format hóa đơn
    /// Bao gồm: tính chiết khấu, VAT, tổng tiền và trọng lượng sản phẩm
    /// </summary>
    public static class ReceiptFormatter
    {
        /// <summary>
        /// Tính toán chiết khấu dành cho khách hàng dựa trên cấp độ thành viên
        /// Sử dụng điểm tích lũy thực tế để xác định cấp độ và mức giảm giá
        /// </summary>
        /// <param name="order">Đơn hàng cần tính chiết khấu</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        /// <returns>Số tiền được giảm giá (VNĐ)</returns>
        public static decimal CalculateCustomerDiscount(
            Order order,
            CustomerService customerService
        )
        {
            if (order.Customer != null)
            {
                // Xác định cấp độ thành viên dựa trên điểm tích lũy thực tế
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = customerService.GetTier(currentTierId);
                if (tier != null)
                {
                    // Tính tổng tiền trước giảm giá
                    var subtotal = order.OrderDetails.Sum(od => od.Total);
                    // Tính số tiền được giảm giá
                    return subtotal * (decimal)(tier.DiscountPercent / 100f);
                }
            }
            return 0; // Không có khách hàng hoặc không đủ điều kiện giảm giá
        }

        /// <summary>
        /// Tính tổng tiền trước thuế (chưa áp dụng giảm giá và VAT)
        /// </summary>
        /// <param name="order">Đơn hàng cần tính</param>
        /// <returns>Tổng tiền trước thuế (VNĐ)</returns>
        public static decimal CalculateSubtotal(Order order)
        {
            return order.OrderDetails.Sum(od => od.Total); // Tổng của tất cả chi tiết đơn hàng
        }

        /// <summary>
        /// Tính thuế VAT 8% dựa trên số tiền sau khi trừ các khoản giảm giá
        /// </summary>
        /// <param name="subtotal">Tổng tiền trước thuế</param>
        /// <param name="customerDiscount">Chiết khấu khách hàng</param>
        /// <param name="orderDiscount">Chiết khấu đơn hàng (mặc định 0)</param>
        /// <returns>Số tiền VAT (VNĐ)</returns>
        public static decimal CalculateVAT(
            decimal subtotal,
            decimal customerDiscount,
            decimal orderDiscount = 0
        )
        {
            var vatRate = 0.08m; // VAT 8% theo quy định Việt Nam
            // VAT được tính trên số tiền sau khi trừ tất cả chiết khấu
            return (subtotal - customerDiscount - orderDiscount) * vatRate;
        }

        /// <summary>
        /// Tính tổng tiền cuối cùng khách hàng phải trả
        /// Công thức: Subtotal - Customer Discount - Order Discount + VAT
        /// </summary>
        /// <param name="order">Đơn hàng cần tính</param>
        /// <param name="customerService">Service để lấy thông tin chiết khấu</param>
        /// <returns>Tổng tiền cuối cùng (VNĐ)</returns>
        public static decimal CalculateFinalTotal(Order order, CustomerService customerService)
        {
            var subtotal = CalculateSubtotal(order); // Tổng tiền trước thuế
            var customerDiscount = CalculateCustomerDiscount(order, customerService); // Chiết khấu khách hàng
            var orderDiscount = 0m; // Chiết khấu đơn hàng (có thể mở rộng trong tương lai)
            var vatAmount = CalculateVAT(subtotal, customerDiscount, orderDiscount); // Thuế VAT

            // Tổng cuối cùng = Tổng trước thuế - Chiết khấu + VAT
            return subtotal - customerDiscount - orderDiscount + vatAmount;
        }

        /// <summary>
        /// Tính tổng khối lượng của đơn hàng (kg)
        /// Dựa trên tên sản phẩm để xác định trọng lượng mỗi đơn vị
        /// </summary>
        /// <param name="order">Đơn hàng cần tính khối lượng</param>
        /// <returns>Tổng khối lượng (kg)</returns>
        public static decimal CalculateTotalWeight(Order order)
        {
            decimal totalWeight = 0;
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Product != null)
                {
                    // Lấy trọng lượng mỗi đơn vị sản phẩm
                    var weightPerUnit = GetProductWeight(detail.Product.Name);
                    // Tổng khối lượng = trọng lượng đơn vị × số lượng
                    totalWeight += weightPerUnit * detail.Quantity;
                }
            }
            return totalWeight;
        }

        /// <summary>
        /// Xác định trọng lượng sản phẩm dựa trên tên sản phẩm
        /// Phân tích tên sản phẩm để tìm thông tin trọng lượng
        /// </summary>
        /// <param name="productName">Tên sản phẩm</param>
        /// <returns>Trọng lượng mỗi đơn vị (kg)</returns>
        private static decimal GetProductWeight(string productName)
        {
            // Phân tích tên sản phẩm để tìm thông tin trọng lượng
            if (productName.Contains("5kg"))
                return 5; // Sản phẩm 5kg
            if (productName.Contains("10kg"))
                return 10; // Sản phẩm 10kg
            if (productName.Contains("2kg"))
                return 2; // Sản phẩm 2kg
            return 1; // Mặc định 1kg cho các sản phẩm khác
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
        /// Logic này phải đồng bộ với CustomerService và OrderService
        /// </summary>
        /// <param name="points">Số điểm tích lũy</param>
        /// <returns>ID cấp độ thành viên tương ứng</returns>
        private static int GetTierIdByPoints(int points)
        {
            if (points >= 1000)
                return 4; // Kim Cương (10% giảm giá)
            else if (points >= 500)
                return 3; // Vàng (5% giảm giá)
            else if (points >= 100)
                return 2; // Bạc (3% giảm giá)
            else
                return 1; // Thường (0% giảm giá)
        }
    }
}
