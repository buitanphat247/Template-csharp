using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị hóa đơn bán hàng đẹp mắt
    /// Sử dụng Spectre.Console để tạo hóa đơn chuyên nghiệp với đầy đủ thông tin
    /// </summary>
    public static class ReceiptTable
    {
        /// <summary>
        /// Hiển thị hóa đơn bán hàng đẹp mắt với đầy đủ thông tin
        /// Bao gồm: thông tin đơn hàng, khách hàng, sản phẩm, chiết khấu và VAT
        /// </summary>
        /// <param name="order">Đơn hàng cần tạo hóa đơn</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        public static void ShowReceiptTable(Order order, CustomerService customerService)
        {
            Console.Clear();

            // Header hóa đơn với màu vàng nổi bật
            var header = new Panel("=== HÓA ĐƠN BÁN GẠO ===")
                .Border(BoxBorder.Double)
                .BorderColor(Color.Yellow);
            AnsiConsole.Write(header);

            // Bảng thông tin đơn hàng và khách hàng
            var orderInfo = new Table();
            orderInfo.Border(TableBorder.Square);
            orderInfo.AddColumn("Thông tin");
            orderInfo.AddColumn("Giá trị");

            // Thông tin cơ bản của đơn hàng
            orderInfo.AddRow("Số đơn", $"#{order.Id}");
            orderInfo.AddRow("Ngày tạo", order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"));

            // Thông tin khách hàng (nếu có)
            if (order.Customer != null)
            {
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = customerService.GetTier(currentTierId);

                orderInfo.AddRow("Khách hàng", order.Customer.Name);
                orderInfo.AddRow("Số điện thoại", order.Customer.Phone);
                orderInfo.AddRow(
                    "Rank hiện tại",
                    $"{tier?.Name} ({tier?.DiscountPercent}% giảm giá)"
                );
                orderInfo.AddRow("Điểm tích lũy", order.Customer.Points.ToString());
            }

            AnsiConsole.Write(orderInfo);

            // Bảng chi tiết sản phẩm
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
                subtotal += detail.Total; // Tính tổng tiền trước thuế
            }

            AnsiConsole.Write(productTable);

            // Bảng tóm tắt thanh toán
            var customerDiscount = CalculateCustomerDiscount(order, subtotal, customerService);
            var orderDiscount = 0m; // Chiết khấu đơn hàng (có thể mở rộng)
            var vatRate = 0.08m; // VAT 8% theo quy định Việt Nam
            var vatAmount = (subtotal - customerDiscount - orderDiscount) * vatRate;
            var finalTotal = subtotal - customerDiscount - orderDiscount + vatAmount;

            var summaryTable = new Table();
            summaryTable.Border(TableBorder.Square);
            summaryTable.AddColumn("Mô tả");
            summaryTable.AddColumn("Số tiền");

            // Thêm các khoản vào bảng tóm tắt
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

            // Footer hóa đơn với lời cảm ơn
            var footer = new Panel("Cảm ơn quý khách!")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue);
            AnsiConsole.Write(footer);

            // Chờ người dùng xem hóa đơn trước khi tiếp tục
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Tính chiết khấu dành cho khách hàng dựa trên cấp độ thành viên
        /// Sử dụng điểm tích lũy thực tế để xác định cấp độ và mức giảm giá
        /// </summary>
        /// <param name="order">Đơn hàng cần tính chiết khấu</param>
        /// <param name="subtotal">Tổng tiền trước thuế</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        /// <returns>Số tiền được giảm giá (VNĐ)</returns>
        private static decimal CalculateCustomerDiscount(
            Order order,
            decimal subtotal,
            CustomerService customerService
        )
        {
            if (order.Customer != null)
            {
                var currentTierId = GetTierIdByPoints(order.Customer.Points);
                var tier = customerService.GetTier(currentTierId);
                if (tier != null)
                {
                    return subtotal * (decimal)(tier.DiscountPercent / 100f);
                }
            }
            return 0; // Không có khách hàng hoặc không đủ điều kiện giảm giá
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
