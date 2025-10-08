using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách đơn hàng
    /// </summary>
    public static class OrderTable
    {
        /// <summary>
        /// Hiển thị bảng đơn hàng cho nhân viên
        /// </summary>
        public static void ShowOrderTable(OrderService orderService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH ĐƠN HÀNG ĐÃ THANH TOÁN ===");
            var orders = orderService
                .GetAllOrders()
                .Where(o => o.Status == "Paid")
                .OrderBy(o => o.CreatedAt);

            if (orders.Any())
            {
                var orderTable = new Table();
                orderTable.Border(TableBorder.Square);
                orderTable.AddColumn("Đơn hàng");
                orderTable.AddColumn("Khách hàng");
                orderTable.AddColumn("Ngày tạo");
                orderTable.AddColumn("Tổng tiền");
                orderTable.AddColumn("Trạng thái");

                foreach (var order in orders)
                {
                    orderTable.AddRow(
                        $"#{order.Id}",
                        order.Customer?.Name ?? "N/A",
                        order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                        DisplayHelper.FormatCurrency(order.TotalAmount),
                        order.Status
                    );
                }

                AnsiConsole.Write(orderTable);

                // Thống kê tổng thu
                var totalRevenue = orders.Sum(o => o.TotalAmount);
                var totalOrders = orders.Count();
                var averageOrder = totalOrders > 0 ? totalRevenue / totalOrders : 0;

                Console.WriteLine("\n=== THỐNG KÊ TỔNG THU ===");
                var statsTable = new Table();
                statsTable.Border(TableBorder.Square);
                statsTable.AddColumn("Thống kê");
                statsTable.AddColumn("Giá trị");

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
        /// Hiển thị bảng giỏ hàng cho khách hàng
        /// </summary>
        public static void ShowCartTable(Order currentOrder)
        {
            if (currentOrder.OrderDetails.Any())
            {
                Console.WriteLine("\nGiỏ hàng của bạn:");

                var cartTable = new Table();
                cartTable.Border(TableBorder.Square);
                cartTable.AddColumn("Sản phẩm");
                cartTable.AddColumn("Số lượng");
                cartTable.AddColumn("Đơn giá");
                cartTable.AddColumn("Thành tiền");

                foreach (var detail in currentOrder.OrderDetails)
                {
                    cartTable.AddRow(
                        detail.Product?.Name ?? "N/A",
                        detail.Quantity.ToString(),
                        DisplayHelper.FormatCurrency(detail.UnitPrice),
                        DisplayHelper.FormatCurrency(detail.Total)
                    );
                }
                AnsiConsole.Write(cartTable);
            }
        }
    }
}
