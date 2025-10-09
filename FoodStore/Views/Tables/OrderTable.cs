using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách đơn hàng và giỏ hàng
    /// Sử dụng Spectre.Console để tạo bảng đẹp và thống kê chi tiết
    /// </summary>
    public static class OrderTable
    {
        /// <summary>
        /// Hiển thị bảng danh sách đơn hàng đã thanh toán cho nhân viên
        /// Bao gồm thông tin đơn hàng và thống kê tổng thu
        /// </summary>
        /// <param name="orderService">Service để lấy dữ liệu đơn hàng</param>
        public static void ShowOrderTable(OrderService orderService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH ĐƠN HÀNG ĐÃ THANH TOÁN ===");

            // Lấy tất cả đơn hàng đã thanh toán, sắp xếp theo ngày tạo
            var orders = orderService
                .GetAllOrders()
                .Where(o => o.Status == "Paid")
                .OrderBy(o => o.CreatedAt);

            if (orders.Any())
            {
                // Tạo bảng danh sách đơn hàng
                var orderTable = new Table();
                orderTable.Border(TableBorder.Square);
                orderTable.AddColumn("Đơn hàng");
                orderTable.AddColumn("Khách hàng");
                orderTable.AddColumn("Ngày tạo");
                orderTable.AddColumn("Tổng tiền");
                orderTable.AddColumn("Trạng thái");

                // Thêm từng đơn hàng vào bảng
                foreach (var order in orders)
                {
                    orderTable.AddRow(
                        $"#{order.Id}", // Format ID đơn hàng
                        order.Customer?.Name ?? "N/A", // Tên khách hàng hoặc N/A
                        order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"), // Format ngày tháng VN
                        DisplayHelper.FormatCurrency(order.TotalAmount), // Format tiền tệ
                        order.Status
                    );
                }

                AnsiConsole.Write(orderTable);

                // Tính toán thống kê tổng thu
                var totalRevenue = orders.Sum(o => o.TotalAmount);
                var totalOrders = orders.Count();
                var averageOrder = totalOrders > 0 ? totalRevenue / totalOrders : 0;

                Console.WriteLine("\n=== THỐNG KÊ TỔNG THU ===");
                var statsTable = new Table();
                statsTable.Border(TableBorder.Square);
                statsTable.AddColumn("Thống kê");
                statsTable.AddColumn("Giá trị");

                // Thêm các thống kê vào bảng
                statsTable.AddRow("Tổng số đơn hàng", totalOrders.ToString());
                statsTable.AddRow("Tổng thu", DisplayHelper.FormatCurrency(totalRevenue));
                statsTable.AddRow(
                    "Đơn hàng trung bình",
                    DisplayHelper.FormatCurrency(averageOrder)
                );

                AnsiConsole.Write(statsTable);
            }
            else
            {
                Console.WriteLine("Chưa có đơn hàng nào đã thanh toán");
            }

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng giỏ hàng hiện tại của khách hàng
        /// Chỉ hiển thị khi có sản phẩm trong giỏ hàng
        /// </summary>
        /// <param name="currentOrder">Đơn hàng hiện tại (giỏ hàng)</param>
        public static void ShowCartTable(Order currentOrder)
        {
            if (currentOrder.OrderDetails.Any())
            {
                Console.WriteLine("\nGiỏ hàng của bạn:");

                // Tạo bảng giỏ hàng với thông tin sản phẩm
                var cartTable = new Table();
                cartTable.Border(TableBorder.Square);
                cartTable.AddColumn("Sản phẩm");
                cartTable.AddColumn("Số lượng");
                cartTable.AddColumn("Đơn giá");
                cartTable.AddColumn("Thành tiền");

                // Thêm từng sản phẩm trong giỏ hàng vào bảng
                foreach (var detail in currentOrder.OrderDetails)
                {
                    cartTable.AddRow(
                        detail.Product?.Name ?? "N/A", // Tên sản phẩm hoặc N/A
                        detail.Quantity.ToString(), // Số lượng
                        DisplayHelper.FormatCurrency(detail.UnitPrice), // Đơn giá đã format
                        DisplayHelper.FormatCurrency(detail.Total) // Thành tiền đã format
                    );
                }
                AnsiConsole.Write(cartTable);
            }
        }
    }
}
