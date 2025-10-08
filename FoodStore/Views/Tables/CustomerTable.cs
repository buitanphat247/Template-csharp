using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách khách hàng
    /// </summary>
    public static class CustomerTable
    {
        /// <summary>
        /// Hiển thị bảng khách hàng cho nhân viên
        /// </summary>
        public static void ShowCustomerTable(CustomerService customerService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH KHÁCH HÀNG ===");
            var customers = customerService.GetAllCustomers();

            var customerTable = new Table();
            customerTable.Border(TableBorder.Square);
            customerTable.AddColumn("ID");
            customerTable.AddColumn("Họ tên");
            customerTable.AddColumn("Số điện thoại");
            customerTable.AddColumn("Điểm tích lũy");
            customerTable.AddColumn("Rank hiện tại");

            foreach (var customer in customers)
            {
                var tier = customerService.GetTier(customer.TierId);
                customerTable.AddRow(
                    customer.Id.ToString(),
                    customer.Name,
                    customer.Phone,
                    customer.Points.ToString(),
                    tier?.Name ?? "N/A"
                );
            }

            AnsiConsole.Write(customerTable);

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng profile khách hàng
        /// </summary>
        public static void ShowProfileTable(Customer customer, CustomerService customerService, OrderService orderService)
        {
            // Lấy thông tin cấp độ thành viên
            var tier = customerService.GetTier(customer.TierId);

            // Tạo bảng thông tin cá nhân
            var profileTable = new Table();
            profileTable.Border(TableBorder.Square);
            profileTable.AddColumn("Thông tin");
            profileTable.AddColumn("Giá trị");

            profileTable.AddRow("Họ tên", customer.Name);
            profileTable.AddRow("Số điện thoại", customer.Phone);
            profileTable.AddRow("Cấp độ thành viên", tier?.Name ?? "Không xác định");
            profileTable.AddRow("Chiết khấu", $"{tier?.DiscountPercent ?? 0}%");
            profileTable.AddRow("Điểm tích lũy", customer.Points.ToString());

            // Tính toán thông tin bổ sung (chỉ đơn hàng đã thanh toán)
            var paidOrders = orderService
                .GetAllOrders()
                .Where(o => o.CustomerId == customer.Id && o.Status == "Paid");

            var totalOrders = paidOrders.Count();
            var totalSpent = paidOrders.Sum(o => o.TotalAmount);

            profileTable.AddRow("Tổng đơn hàng", totalOrders.ToString());
            profileTable.AddRow("Tổng chi tiêu", DisplayHelper.FormatCurrency(totalSpent));

            AnsiConsole.Write(profileTable);
        }

        /// <summary>
        /// Hiển thị bảng lịch sử đơn hàng
        /// </summary>
        public static void ShowOrderHistoryTable(OrderService orderService, Customer customer)
        {
            var paidOrders = orderService
                .GetAllOrders()
                .Where(o => o.CustomerId == customer.Id && o.Status == "Paid");

            Console.WriteLine("\n=== LỊCH SỬ ĐƠN HÀNG ĐÃ THANH TOÁN ===");
            var recentOrders = paidOrders.OrderBy(o => o.CreatedAt).Take(5);

            if (recentOrders.Any())
            {
                var orderHistoryTable = new Table();
                orderHistoryTable.Border(TableBorder.Square);
                orderHistoryTable.AddColumn("Đơn hàng");
                orderHistoryTable.AddColumn("Ngày tạo");
                orderHistoryTable.AddColumn("Tổng tiền");
                orderHistoryTable.AddColumn("Trạng thái");

                foreach (var order in recentOrders)
                {
                    orderHistoryTable.AddRow(
                        $"#{order.Id}",
                        order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                        DisplayHelper.FormatCurrency(order.TotalAmount),
                        order.Status
                    );
                }
                AnsiConsole.Write(orderHistoryTable);
            }
            else
            {
                Console.WriteLine("Chưa có đơn hàng nào đã thanh toán");
            }
        }
    }
}
