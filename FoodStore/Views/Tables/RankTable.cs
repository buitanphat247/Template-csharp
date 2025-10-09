using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị hệ thống cấp độ thành viên và xếp hạng
    /// Bao gồm thông tin rank hiện tại, rank tiếp theo và thống kê
    /// </summary>
    public static class RankTable
    {
        /// <summary>
        /// Hiển thị bảng xếp hạng thành viên cho khách hàng
        /// Bao gồm rank hiện tại, rank tiếp theo và bảng cấp độ đầy đủ
        /// </summary>
        /// <param name="customer">Khách hàng cần xem rank</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        public static void ShowCustomerRankTable(Customer customer, CustomerService customerService)
        {
            Console.Clear();
            Console.WriteLine("=== BẢNG XẾP HẠNG THÀNH VIÊN ===");
            Console.WriteLine(DisplayHelper.CreateSeparator(60));

            // Lấy thông tin rank hiện tại dựa trên điểm tích lũy thực tế
            var currentTierId = GetTierIdByPoints(customer.Points);
            var currentTier = customerService.GetTier(currentTierId);
            Console.WriteLine(
                $"Rank hiện tại: {currentTier?.Name ?? "Không xác định"} ({currentTier?.DiscountPercent ?? 0}% giảm giá)"
            );
            Console.WriteLine($"Điểm hiện tại: {customer.Points}");

            // Tính toán thông tin rank tiếp theo
            var nextTier = GetNextTier(currentTierId, customerService);
            if (nextTier != null)
            {
                var pointsNeeded = GetPointsNeededForTier(nextTier.Id);
                var pointsToGo = pointsNeeded - customer.Points;
                Console.WriteLine(
                    $"Rank tiếp theo: {nextTier.Name} ({nextTier.DiscountPercent}% giảm giá)"
                );
                Console.WriteLine($"Cần thêm: {pointsToGo} điểm");
            }
            else
            {
                Console.WriteLine("Bạn đã đạt rank cao nhất!");
            }

            Console.WriteLine(DisplayHelper.CreateSeparator(60));
            Console.WriteLine("=== BẢNG RANK THÀNH VIÊN ===");

            // Tạo bảng hiển thị tất cả cấp độ thành viên
            var table = new Table();
            table.Border(TableBorder.Square);
            table.AddColumn("Rank");
            table.AddColumn("Điểm cần");
            table.AddColumn("Giảm giá");
            table.AddColumn("Trạng thái");

            // Định nghĩa các cấp độ thành viên
            var tiers = new[]
            {
                (1, "Thường", 0, 0),
                (2, "Bạc", 100, 3),
                (3, "Vàng", 500, 5),
                (4, "Kim Cương", 1000, 10),
            };

            // Thêm từng cấp độ vào bảng
            foreach (var (id, name, points, discount) in tiers)
            {
                var tier = customerService.GetTier(id);
                var status = currentTierId == id ? "← Hiện tại" : ""; // Đánh dấu rank hiện tại
                table.AddRow(name, points.ToString(), $"{discount}%", status);
            }

            AnsiConsole.Write(table);

            Console.WriteLine(DisplayHelper.CreateSeparator(60));
            Console.WriteLine("Nhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng quản lý rank thành viên cho nhân viên
        /// Bao gồm thống kê số lượng khách hàng theo từng cấp độ
        /// </summary>
        /// <param name="customerService">Service để lấy dữ liệu khách hàng và cấp độ</param>
        public static void ShowEmployeeRankTable(CustomerService customerService)
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ RANK THÀNH VIÊN ===");

            Console.WriteLine("=== BẢNG RANK HIỆN TẠI ===");
            var rankTable = new Table();
            rankTable.Border(TableBorder.Square);
            rankTable.AddColumn("Rank");
            rankTable.AddColumn("Điểm cần");
            rankTable.AddColumn("Giảm giá");
            rankTable.AddColumn("Số khách hàng");

            // Hiển thị thống kê cho từng cấp độ thành viên
            var tiers = new[] { 1, 2, 3, 4 };
            foreach (var tierId in tiers)
            {
                var tier = customerService.GetTier(tierId);
                var customers = customerService.GetAllCustomers().Count(c => c.TierId == tierId);
                var pointsNeeded = GetPointsNeededForTier(tierId);

                rankTable.AddRow(
                    tier?.Name ?? "N/A",
                    pointsNeeded.ToString(),
                    $"{tier?.DiscountPercent}%",
                    customers.ToString()
                );
            }

            AnsiConsole.Write(rankTable);

            Console.WriteLine("\n=== THỐNG KÊ KHÁCH HÀNG THEO RANK ===");
            var allCustomers = customerService.GetAllCustomers();
            var tierGroups = allCustomers.GroupBy(c => c.TierId).OrderBy(g => g.Key);

            // Hiển thị danh sách khách hàng theo từng cấp độ
            foreach (var group in tierGroups)
            {
                var tier = customerService.GetTier(group.Key);
                Console.WriteLine($"\n{tier?.Name} ({tier?.DiscountPercent}% giảm giá):");

                var customerTable = new Table();
                customerTable.Border(TableBorder.Square);
                customerTable.AddColumn("Khách hàng");
                customerTable.AddColumn("Điểm tích lũy");

                // Sắp xếp khách hàng theo điểm tích lũy giảm dần
                foreach (var customer in group.OrderByDescending(c => c.Points))
                {
                    customerTable.AddRow(customer.Name, customer.Points.ToString());
                }

                AnsiConsole.Write(customerTable);
            }

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Lấy thông tin cấp độ thành viên tiếp theo
        /// </summary>
        /// <param name="currentTierId">ID cấp độ hiện tại</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ</param>
        /// <returns>Thông tin cấp độ tiếp theo hoặc null nếu đã ở cấp cao nhất</returns>
        private static MemberTier? GetNextTier(int currentTierId, CustomerService customerService)
        {
            return customerService.GetTier(currentTierId + 1);
        }

        /// <summary>
        /// Lấy số điểm cần thiết để đạt được cấp độ thành viên
        /// </summary>
        /// <param name="tierId">ID cấp độ thành viên</param>
        /// <returns>Số điểm cần thiết</returns>
        private static int GetPointsNeededForTier(int tierId)
        {
            return tierId switch
            {
                1 => 0, // Thường - không cần điểm
                2 => 100, // Bạc - cần 100 điểm
                3 => 500, // Vàng - cần 500 điểm
                4 => 1000, // Kim Cương - cần 1000 điểm
                _ => 0, // Mặc định
            };
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
