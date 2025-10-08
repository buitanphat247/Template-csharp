using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views.Tables
{
    /// <summary>
    /// Table hiển thị danh sách sản phẩm
    /// </summary>
    public static class ProductTable
    {
        /// <summary>
        /// Hiển thị bảng sản phẩm cho nhân viên
        /// </summary>
        public static void ShowProductTable(ProductService productService)
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH SẢN PHẨM ===");
            var products = productService.GetAllProducts();

            var productTable = new Table();
            productTable.Border(TableBorder.Square);
            productTable.AddColumn("ID");
            productTable.AddColumn("Tên sản phẩm");
            productTable.AddColumn("Giá");
            productTable.AddColumn("Tồn kho");
            productTable.AddColumn("Trạng thái");

            foreach (var product in products)
            {
                var status = product.Stock > 0 ? "Còn hàng" : "Hết hàng";
                productTable.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    DisplayHelper.FormatCurrency(product.Price),
                    product.Stock.ToString(),
                    status
                );
            }

            AnsiConsole.Write(productTable);

            Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
            Console.ReadKey();
        }

        /// <summary>
        /// Hiển thị bảng sản phẩm cho khách hàng mua sắm
        /// </summary>
        public static void ShowShoppingProductTable(ProductService productService)
        {
            var products = productService.GetAllProducts();
            Console.WriteLine("Sản phẩm có sẵn:");

            var productTable = new Table();
            productTable.Border(TableBorder.Square);
            productTable.AddColumn("ID");
            productTable.AddColumn("Tên sản phẩm");
            productTable.AddColumn("Giá");
            productTable.AddColumn("Tồn kho");

            foreach (var product in products)
            {
                productTable.AddRow(
                    product.Id.ToString(),
                    product.Name,
                    DisplayHelper.FormatCurrency(product.Price),
                    product.Stock.ToString()
                );
            }
            AnsiConsole.Write(productTable);
        }
    }
}
