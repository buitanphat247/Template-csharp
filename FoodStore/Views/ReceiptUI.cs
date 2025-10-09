using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views
{
    /// <summary>
    /// ReceiptUI - Giao diện hiển thị hóa đơn bán hàng
    /// Quản lý việc hiển thị hóa đơn đẹp mắt với thông tin đầy đủ về đơn hàng
    /// Sử dụng Spectre.Console để tạo layout chuyên nghiệp với màu sắc
    /// Sử dụng Dependency Injection để nhận CustomerService
    /// </summary>
    public class ReceiptUI
    {
        /// <summary>
        /// Service quản lý khách hàng - được inject từ ShoppingUI
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận CustomerService từ ShoppingUI
        /// </summary>
        /// <param name="customerService">Service quản lý khách hàng</param>
        public ReceiptUI(CustomerService customerService)
        {
            // Lưu trữ service được inject
            _customerService = customerService;
        }

        /// <summary>
        /// Hiển thị hóa đơn bán hàng đẹp mắt với layout chuyên nghiệp
        /// Bao gồm header, thông tin đơn hàng, khách hàng, sản phẩm và tóm tắt thanh toán
        /// Sử dụng Spectre.Console để tạo các Panel và Table với màu sắc
        /// </summary>
        /// <param name="order">Đơn hàng cần hiển thị hóa đơn</param>
        public void ShowReceipt(Order order)
        {
            Console.Clear();

            // Header với Panel màu vàng
            var header = new Panel("=== HÓA ĐƠN BÁN GẠO ===")
                .Border(BoxBorder.Double)
                .BorderColor(Color.Yellow);
            AnsiConsole.Write(header);

            // Bảng thông tin đơn hàng và khách hàng
            var orderInfo = new Table();
            orderInfo.Border(TableBorder.Square);
            orderInfo.AddColumn("Thông tin");
            orderInfo.AddColumn("Giá trị");

            orderInfo.AddRow("Số đơn", $"#{order.Id}");
            orderInfo.AddRow("Ngày tạo", order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"));

            // Thông tin khách hàng nếu có
            if (order.Customer != null)
            {
                // Lấy cấp độ thành viên dựa trên điểm tích lũy thực tế
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = _customerService.GetTier(currentTierId);

                orderInfo.AddRow("Khách hàng", order.Customer.Name);
                orderInfo.AddRow("Số điện thoại", order.Customer.Phone);
                orderInfo.AddRow(
                    "Rank hiện tại",
                    $"{tier?.Name} ({tier?.DiscountPercent}% giảm giá)"
                );
                orderInfo.AddRow("Điểm tích lũy", order.Customer.Points.ToString());
            }

            AnsiConsole.Write(orderInfo);

            // Bảng sản phẩm trong đơn hàng
            var productTable = new Table();
            productTable.Border(TableBorder.Square);
            productTable.AddColumn("Sản phẩm");
            productTable.AddColumn("SL");
            productTable.AddColumn("Đơn giá");
            productTable.AddColumn("Giảm%");
            productTable.AddColumn("Thành tiền");

            decimal subtotal = 0;
            foreach (var detail in order.OrderDetails)
            {
                var productName = detail.Product?.Name ?? "N/A";
                var quantity = detail.Quantity.ToString();
                var unitPrice = DisplayHelper.FormatCurrency(detail.UnitPrice);
                var discount = $"{detail.DiscountPercent:F0}%";
                var total = DisplayHelper.FormatCurrency(detail.Total);

                productTable.AddRow(productName, quantity, unitPrice, discount, total);
                subtotal += detail.Total;
            }

            AnsiConsole.Write(productTable);

            // Bảng tóm tắt thanh toán với chiết khấu và VAT
            var customerDiscount = CalculateCustomerDiscount(order, subtotal);
            var orderDiscount = 0m; // Có thể thêm logic chiết khấu đơn hàng trong tương lai
            var vatRate = 0.08m; // VAT 8% theo quy định Việt Nam
            var vatAmount = (subtotal - customerDiscount - orderDiscount) * vatRate;
            var finalTotal = subtotal - customerDiscount - orderDiscount + vatAmount;

            var summaryTable = new Table();
            summaryTable.Border(TableBorder.Square);
            summaryTable.AddColumn("Mô tả");
            summaryTable.AddColumn("Số tiền");

            summaryTable.AddRow("Tạm tính", DisplayHelper.FormatCurrency(subtotal));
            summaryTable.AddRow(
                "Chiết khấu khách hàng",
                $"-{DisplayHelper.FormatCurrency(customerDiscount)}"
            );
            summaryTable.AddRow(
                "Chiết khấu đơn hàng",
                $"-{DisplayHelper.FormatCurrency(orderDiscount)}"
            );
            summaryTable.AddRow("VAT 8%", DisplayHelper.FormatCurrency(vatAmount));

            // Tổng cộng với highlight đặc biệt
            var totalRow = new Table();
            totalRow.Border(TableBorder.Square);
            totalRow.AddColumn("TỔNG CỘNG");
            totalRow.AddColumn(DisplayHelper.FormatCurrency(finalTotal));

            AnsiConsole.Write(summaryTable);
            AnsiConsole.Write(totalRow);

            // Footer với Panel màu xanh
            var footer = new Panel("Cảm ơn quý khách!")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue);
            AnsiConsole.Write(footer);
        }

        /// <summary>
        /// Tính chiết khấu dành cho khách hàng dựa trên cấp độ thành viên
        /// Sử dụng điểm tích lũy thực tế để xác định cấp độ và mức giảm giá
        /// </summary>
        /// <param name="order">Đơn hàng cần tính chiết khấu</param>
        /// <param name="subtotal">Tổng tiền trước chiết khấu</param>
        /// <returns>Số tiền được giảm giá (VNĐ)</returns>
        private decimal CalculateCustomerDiscount(Order order, decimal subtotal)
        {
            if (order.Customer != null)
            {
                // Xác định cấp độ thành viên dựa trên điểm tích lũy thực tế
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = _customerService.GetTier(currentTierId);
                if (tier != null)
                {
                    // Tính số tiền được giảm giá
                    return subtotal * (decimal)(tier.DiscountPercent / 100f);
                }
            }
            return 0; // Không có khách hàng hoặc không đủ điều kiện giảm giá
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
        /// Logic này phải đồng bộ với CustomerService và các UI khác
        /// </summary>
        /// <param name="points">Số điểm tích lũy của khách hàng</param>
        /// <returns>ID cấp độ thành viên tương ứng</returns>
        private int GetTierIdByPoints(int points)
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
