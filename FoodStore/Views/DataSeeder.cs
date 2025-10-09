using System.Collections.Generic;
using FoodStore.Models;
using FoodStore.Services;

namespace FoodStore.Views
{
    /// <summary>
    /// DataSeeder - Quản lý dữ liệu mẫu cho hệ thống
    /// Cung cấp các phương thức static để khởi tạo dữ liệu test
    /// Bao gồm: sản phẩm mẫu, khách hàng mẫu với các cấp độ thành viên khác nhau
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Thêm dữ liệu sản phẩm mẫu vào hệ thống
        /// Tạo danh sách các loại gạo phổ biến với giá cả và tồn kho thực tế
        /// Sử dụng ProductService để thêm sản phẩm vào hệ thống
        /// </summary>
        /// <param name="productService">Service quản lý sản phẩm</param>
        public static void SeedProducts(ProductService productService)
        {
            // Danh sách sản phẩm gạo mẫu với thông tin đầy đủ
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

            // Thêm từng sản phẩm vào hệ thống thông qua ProductService
            foreach (var product in products)
            {
                productService.AddProduct(product);
                // Có thể sử dụng cách khác để tạo sản phẩm không cần ID:
                // productService.AddProduct(new Product
                // {
                //     Name = product.Name,
                //     Category = product.Category,
                //     Price = product.Price,
                //     Stock = product.Stock,
                //     Status = product.Status,
                // });
            }
        }

        /// <summary>
        /// Thêm dữ liệu khách hàng mẫu với các cấp độ thành viên khác nhau
        /// Tạo khách hàng với điểm tích lũy phù hợp để test hệ thống rank
        /// Đồng bộ hóa TierId với điểm tích lũy thực tế
        /// </summary>
        /// <param name="customerService">Service quản lý khách hàng</param>
        public static void SeedCustomers(CustomerService customerService)
        {
            // Tạo khách hàng cấp độ Thường (0 điểm)
            var customer1 = customerService.RegisterCustomer("Nguyễn Văn A", "0901234567", 1); // Thường
            customer1.Points = 0; // Đảm bảo điểm = 0 để rank = Thường
            customer1.TierId = GetTierIdByPoints(customer1.Points); // Cập nhật TierId theo điểm thực tế

            // Tạo khách hàng cấp độ Bạc (150 điểm)
            var customer2 = customerService.RegisterCustomer("Trần Thị B", "0901234568", 1); // Thường
            customer2.Points = 150; // Đủ điểm để rank = Bạc (>= 100 điểm)
            customer2.TierId = GetTierIdByPoints(customer2.Points); // Cập nhật TierId theo điểm thực tế

            // Tạo khách hàng cấp độ Thường (0 điểm)
            var customer3 = customerService.RegisterCustomer("Lê Văn C", "0901234569", 1); // Thường
            customer3.Points = 0; // Đảm bảo điểm = 0 để rank = Thường
            customer3.TierId = GetTierIdByPoints(customer3.Points); // Cập nhật TierId theo điểm thực tế
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
        /// Logic này phải đồng bộ với CustomerService và các UI khác
        /// </summary>
        /// <param name="points">Số điểm tích lũy của khách hàng</param>
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
