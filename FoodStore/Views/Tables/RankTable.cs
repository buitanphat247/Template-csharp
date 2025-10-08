using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị rank thành viên
    /// </summary>
    public static class RankTable
    {
        /// <summary>
        /// Hiển thị bảng rank cho khách hàng
        /// </summary>
        public static void ShowCustomerRankTable(Customer customer, CustomerService customerService)
        {
            Console.Clear();
            Console.WriteLine("=== BẢNG XẾP HẠNG THÀNH VIÊN ===");
            Console.WriteLine(DisplayHelper.CreateSeparator(60));

            // Lấy thông tin rank hiện tại (dựa trên điểm thực tế)
            var currentTierId = GetTierIdByPoints(customer.Points);
            var currentTier = customerService.GetTier(currentTierId);
            Console.WriteLine(
                $"Rank hiện tại: {currentTier?.Name ?? "Không xác định"} ({currentTier?.DiscountPercent ?? 0}% giảm giá)"
            );
            Console.WriteLine($"Điểm hiện tại: {customer.Points}");

            // Tính điểm cần thiết để lên rank tiếp theo
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

            var table = new Table();
            table.Border(TableBorder.Square);
            table.AddColumn("Rank");
            table.AddColumn("Điểm cần");
            table.AddColumn("Giảm giá");
            table.AddColumn("Trạng thái");

            var tiers = new[]
            {
                (1, "Thường", 0, 0),
                (2, "Bạc", 100, 3),
                (3, "Vàng", 500, 5),
                (4, "Kim Cương", 1000, 10),
            };

            foreach (var (id, name, points, discount) in tiers)
            {
                var tier = customerService.GetTier(id);
                var status = currentTierId == id ? "← Hiện tại" : "";
                table.AddRow(name, points.ToString(), $"{discount}%", status);
            }

            AnsiConsole.Write(table);

            Console.WriteLine(DisplayHelper.CreateSeparator(60));
            Console.WriteLine("Nhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng quản lý rank cho nhân viên
        /// </summary>
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

            foreach (var group in tierGroups)
            {
                var tier = customerService.GetTier(group.Key);
                Console.WriteLine($"\n{tier?.Name} ({tier?.DiscountPercent}% giảm giá):");

                var customerTable = new Table();
                customerTable.Border(TableBorder.Square);
                customerTable.AddColumn("Khách hàng");
                customerTable.AddColumn("Điểm tích lũy");

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
        /// Lấy rank tiếp theo
        /// </summary>
        private static MemberTier? GetNextTier(int currentTierId, CustomerService customerService)
        {
            return customerService.GetTier(currentTierId + 1);
        }

        /// <summary>
        /// Lấy điểm cần thiết cho rank
        /// </summary>
        private static int GetPointsNeededForTier(int tierId)
        {
            return tierId switch
            {
                1 => 0, // Thường
                2 => 100, // Bạc
                3 => 500, // Vàng
                4 => 1000, // Kim Cương
                _ => 0,
            };
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
