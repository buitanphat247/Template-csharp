using System.Collections.Generic;
using FoodStore.Models;
using FoodStore.Services;

namespace FoodStore.Views
{
    /// <summary>
    /// Quản lý dữ liệu mẫu
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Thêm dữ liệu sản phẩm mẫu
        /// </summary>
        public static void SeedProducts(ProductService productService)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Gạo ST25 5kg",
                    Category = "Gạo",
                    Price = 180000,
                    Stock = 50,
                    Status = "Active",
                },
                new Product
                {
                    Id = 2,
                    Name = "Gạo Jasmine 10kg",
                    Category = "Gạo",
                    Price = 320000,
                    Stock = 25,
                    Status = "Active",
                },
                new Product
                {
                    Id = 3,
                    Name = "Gạo Nàng Thơm 5kg",
                    Category = "Gạo",
                    Price = 150000,
                    Stock = 30,
                    Status = "Active",
                },
                new Product
                {
                    Id = 4,
                    Name = "Gạo Tám Hải Hậu 5kg",
                    Category = "Gạo",
                    Price = 200000,
                    Stock = 20,
                    Status = "Active",
                },
                new Product
                {
                    Id = 5,
                    Name = "Gạo Nếp Cái Hoa Vàng 2kg",
                    Category = "Gạo Nếp",
                    Price = 80000,
                    Stock = 15,
                    Status = "Active",
                },
            };

            foreach (var product in products)
            {
                productService.AddProduct(product);
            }
        }

        /// <summary>
        /// Thêm dữ liệu khách hàng mẫu
        /// </summary>
        public static void SeedCustomers(CustomerService customerService)
        {
            // Tạo khách hàng với điểm phù hợp với rank
            var customer1 = customerService.RegisterCustomer("Nguyễn Văn A", "0901234567", 1); // Thường
            customer1.Points = 0; // Đảm bảo điểm = 0 để rank = Thường
            customer1.TierId = GetTierIdByPoints(customer1.Points); // Cập nhật TierId theo điểm thực tế

            var customer2 = customerService.RegisterCustomer("Trần Thị B", "0901234568", 1); // Thường
            customer2.Points = 150; // Đủ điểm để rank = Bạc
            customer2.TierId = GetTierIdByPoints(customer2.Points); // Cập nhật TierId theo điểm thực tế

            var customer3 = customerService.RegisterCustomer("Lê Văn C", "0901234569", 1); // Thường
            customer3.Points = 0; // Đảm bảo điểm = 0 để rank = Thường
            customer3.TierId = GetTierIdByPoints(customer3.Points); // Cập nhật TierId theo điểm thực tế
        }

        /// <summary>
        /// Lấy rank ID dựa trên điểm tích lũy (đồng bộ với logic trong CustomerService)
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
