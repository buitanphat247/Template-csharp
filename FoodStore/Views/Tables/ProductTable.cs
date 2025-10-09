using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách sản phẩm cho nhân viên và khách hàng
    /// Sử dụng Spectre.Console để tạo bảng đẹp và dễ đọc
    /// </summary>
    public static class ProductTable
    {
        /// <summary>
        /// Hiển thị bảng danh sách sản phẩm đầy đủ cho nhân viên
        /// Bao gồm thông tin trạng thái tồn kho và quản lý
        /// </summary>
        /// <param name="productService">Service để lấy dữ liệu sản phẩm</param>
        public static void ShowProductTable(ProductService productService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH SẢN PHẨM ===");
            var products = productService.GetAllProducts();

            // Tạo bảng sản phẩm với đầy đủ thông tin
            var productTable = new Table();
            productTable.Border(TableBorder.Square);
            productTable.AddColumn("ID");
            productTable.AddColumn("Tên sản phẩm");
            productTable.AddColumn("Giá");
            productTable.AddColumn("Tồn kho");
            productTable.AddColumn("Trạng thái");

            // Thêm từng sản phẩm vào bảng
            foreach (var product in products)
            {
                // Xác định trạng thái dựa trên tồn kho
                var status = product.Stock > 0 ? "Còn hàng" : "Hết hàng";
                productTable.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    DisplayHelper.FormatCurrency(product.Price), // Format tiền tệ
                    product.Stock.ToString(),
                    status
                );
            }

            AnsiConsole.Write(productTable);

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng sản phẩm đơn giản cho khách hàng mua sắm
        /// Chỉ hiển thị thông tin cần thiết để khách hàng lựa chọn
        /// </summary>
        /// <param name="productService">Service để lấy dữ liệu sản phẩm</param>
        public static void ShowShoppingProductTable(ProductService productService)
        {
            var products = productService.GetAllProducts();
            Console.WriteLine("Sản phẩm có sẵn:");

            // Tạo bảng sản phẩm đơn giản cho khách hàng
            var productTable = new Table();
            productTable.Border(TableBorder.Square);
            productTable.AddColumn("ID");
            productTable.AddColumn("Tên sản phẩm");
            productTable.AddColumn("Giá");
            productTable.AddColumn("Tồn kho");

            // Thêm từng sản phẩm vào bảng
            foreach (var product in products)
            {
                productTable.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    DisplayHelper.FormatCurrency(product.Price), // Format tiền tệ
                    product.Stock.ToString()
                );
            }
            AnsiConsole.Write(productTable);
        }
    }
}
