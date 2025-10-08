using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;

namespace FoodStore.Utils
{
    /// <summary>
    /// Formatter để tính toán hóa đơn
    /// </summary>
    public static class ReceiptFormatter
    {
        /// <summary>
        /// Tính toán chiết khấu khách hàng
        /// </summary>
        public static decimal CalculateCustomerDiscount(Order order, CustomerService customerService)
        {
            if (order.Customer != null)
            {
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = customerService.GetTier(currentTierId);
                if (tier != null)
                {
                    var subtotal = order.OrderDetails.Sum(od => od.Total);
                    return subtotal * (decimal)(tier.DiscountPercent / 100f);
                }
            }
            return 0;
        }

        /// <summary>
        /// Tính tổng tiền trước thuế
        /// </summary>
        public static decimal CalculateSubtotal(Order order)
        {
            return order.OrderDetails.Sum(od => od.Total);
        }

        /// <summary>
        /// Tính VAT
        /// </summary>
        public static decimal CalculateVAT(decimal subtotal, decimal customerDiscount, decimal orderDiscount = 0)
        {
            var vatRate = 0.08m; // VAT 8%
            return (subtotal - customerDiscount - orderDiscount) * vatRate;
        }

        /// <summary>
        /// Tính tổng tiền cuối cùng
        /// </summary>
        public static decimal CalculateFinalTotal(Order order, CustomerService customerService)
        {
            var subtotal = CalculateSubtotal(order);
            var customerDiscount = CalculateCustomerDiscount(order, customerService);
            var orderDiscount = 0m; // Có thể thêm logic chiết khấu đơn hàng
            var vatAmount = CalculateVAT(subtotal, customerDiscount, orderDiscount);
            
            return subtotal - customerDiscount - orderDiscount + vatAmount;
        }

        /// <summary>
        /// Tính tổng khối lượng (kg)
        /// </summary>
        public static decimal CalculateTotalWeight(Order order)
        {
            decimal totalWeight = 0;
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Product != null)
                {
                    // Giả sử mỗi sản phẩm có trọng lượng cố định
                    var weightPerUnit = GetProductWeight(detail.Product.Name);
                    totalWeight += weightPerUnit * detail.Quantity;
                }
            }
            return totalWeight;
        }

        /// <summary>
        /// Lấy trọng lượng sản phẩm (kg)
        /// </summary>
        private static decimal GetProductWeight(string productName)
        {
            if (productName.Contains("5kg"))
                return 5;
            if (productName.Contains("10kg"))
                return 10;
            if (productName.Contains("2kg"))
                return 2;
            return 1; // Mặc định
        }

        /// <summary>
        /// Lấy rank ID dựa trên điểm tích lũy
        /// </summary>
        private static int GetTierIdByPoints(int points)
        {
            if (points >= 1000)
                return 4; // Kim Cương
            else if (points >= 500)
                return 3; // Vàng
            else if (points >= 100)
                return 2; // Bạc
            else
                return 1; // Thường
        }
    }
}
