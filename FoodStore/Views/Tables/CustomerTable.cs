using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách khách hàng và thông tin profile
    /// Sử dụng Spectre.Console để tạo bảng đẹp và dễ đọc
    /// </summary>
    public static class CustomerTable
    {
        /// <summary>
        /// Hiển thị bảng danh sách tất cả khách hàng cho nhân viên
        /// Bao gồm thông tin cơ bản và cấp độ thành viên
        /// </summary>
        /// <param name="customerService">Service để lấy dữ liệu khách hàng</param>
        public static void ShowCustomerTable(CustomerService customerService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH KHÁCH HÀNG ===");
            var customers = customerService.GetAllCustomers();

            // Tạo bảng với Spectre.Console
            var customerTable = new Table();
            customerTable.Border(TableBorder.Square); // Viền vuông đẹp mắt
            customerTable.AddColumn("ID");
            customerTable.AddColumn("Họ tên");
            customerTable.AddColumn("Số điện thoại");
            customerTable.AddColumn("Điểm tích lũy");
            customerTable.AddColumn("Rank hiện tại");

            // Thêm dữ liệu từng khách hàng vào bảng
            foreach (var customer in customers)
            {
                var tier = customerService.GetTier(customer.TierId);
                customerTable.AddRow(
                    customer.Id.ToString(),
                    customer.Name,
                    customer.Phone,
                    customer.Points.ToString(),
                    tier?.Name ?? "N/A" // Hiển thị "N/A" nếu không có thông tin cấp độ
                );
            }

            AnsiConsole.Write(customerTable);

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng profile chi tiết của khách hàng
        /// Bao gồm thông tin cá nhân, cấp độ thành viên và thống kê mua hàng
        /// </summary>
        /// <param name="customer">Khách hàng cần hiển thị profile</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        /// <param name="orderService">Service để lấy thông tin đơn hàng</param>
        public static void ShowProfileTable(
            Customer customer,
            CustomerService customerService,
            OrderService orderService
        )
        {
            // Lấy thông tin cấp độ thành viên hiện tại
            var tier = customerService.GetTier(customer.TierId);

            // Tạo bảng thông tin cá nhân với 2 cột
            var profileTable = new Table();
            profileTable.Border(TableBorder.Square);
            profileTable.AddColumn("Thông tin");
            profileTable.AddColumn("Giá trị");

            // Thêm thông tin cơ bản của khách hàng
            profileTable.AddRow("Họ tên", customer.Name);
            profileTable.AddRow("Số điện thoại", customer.Phone);
            profileTable.AddRow("Cấp độ thành viên", tier?.Name ?? "Không xác định");
            profileTable.AddRow("Chiết khấu", $"{tier?.DiscountPercent ?? 0}%");
            profileTable.AddRow("Điểm tích lũy", customer.Points.ToString());

            // Tính toán thông tin bổ sung từ lịch sử đơn hàng (chỉ đơn hàng đã thanh toán)
            var paidOrders = orderService
                .GetAllOrders()
                .Where(o => o.CustomerId == customer.Id && o.Status == "Paid");

            var totalOrders = paidOrders.Count();
            var totalSpent = paidOrders.Sum(o => o.TotalAmount);

            // Thêm thống kê mua hàng
            profileTable.AddRow("Tổng đơn hàng", totalOrders.ToString());
            profileTable.AddRow("Tổng chi tiêu", DisplayHelper.FormatCurrency(totalSpent));

            AnsiConsole.Write(profileTable);
        }

        /// <summary>
        /// Hiển thị bảng lịch sử đơn hàng đã thanh toán của khách hàng
        /// Chỉ hiển thị 5 đơn hàng gần nhất để tránh quá tải thông tin
        /// </summary>
        /// <param name="orderService">Service để lấy dữ liệu đơn hàng</param>
        /// <param name="customer">Khách hàng cần xem lịch sử</param>
        public static void ShowOrderHistoryTable(OrderService orderService, Customer customer)
        {
            // Lấy tất cả đơn hàng đã thanh toán của khách hàng
            var paidOrders = orderService
                .GetAllOrders()
                .Where(o => o.CustomerId == customer.Id && o.Status == "Paid");

            Console.WriteLine("\n=== LỊCH SỬ ĐƠN HÀNG ĐÃ THANH TOÁN ===");

            // Lấy 5 đơn hàng gần nhất (sắp xếp theo ngày tạo)
            var recentOrders = paidOrders.OrderBy(o => o.CreatedAt).Take(5);

            if (recentOrders.Any())
            {
                // Tạo bảng lịch sử đơn hàng
                var orderHistoryTable = new Table();
                orderHistoryTable.Border(TableBorder.Square);
                orderHistoryTable.AddColumn("Đơn hàng");
                orderHistoryTable.AddColumn("Ngày tạo");
                orderHistoryTable.AddColumn("Tổng tiền");
                orderHistoryTable.AddColumn("Trạng thái");

                // Thêm từng đơn hàng vào bảng
                foreach (var order in recentOrders)
                {
                    orderHistoryTable.AddRow(
                        $"#{order.Id}", // Format ID đơn hàng với dấu #
                        order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"), // Format ngày tháng VN
                        DisplayHelper.FormatCurrency(order.TotalAmount), // Format tiền tệ
                        order.Status
                    );
                }
                AnsiConsole.Write(orderHistoryTable);
            }
            else
            {
                // Hiển thị thông báo nếu chưa có đơn hàng nào
                Console.WriteLine("Chưa có đơn hàng nào đã thanh toán");
            }
        }
    }
}
