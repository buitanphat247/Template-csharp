// See https://aka.ms/new-console-template for more information
using System;
using System.Text;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using FoodStore.Views;

class Program
{
    static void Main(string[] args)
    {
        // config utf-8 for language vietnam
        Console.OutputEncoding = Encoding.UTF8;

        // Khởi tạo services
        var productService = new ProductService();
        var orderService = new OrderService();
        var customerService = new CustomerService();
        var employeeService = new EmployeeService();

        // Thêm dữ liệu mẫu
        DataSeeder.SeedProducts(productService);
        DataSeeder.SeedCustomers(customerService);

        // Đồng bộ hóa TierId của tất cả khách hàng
        customerService.SyncAllCustomerTiers();

        // Khởi tạo UI Manager
        var uiManager = new UIManager(
            productService,
            orderService,
            customerService,
            employeeService
        );

        // Hiển thị menu chính
        uiManager.ShowMainMenu();
    }
}
