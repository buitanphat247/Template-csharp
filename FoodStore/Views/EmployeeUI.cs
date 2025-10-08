using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using FoodStore.Views.Tables;
using Spectre.Console;

namespace FoodStore.Views
{
    /// <summary>
    /// Giao diện nhân viên
    /// </summary>
    public class EmployeeUI
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;

        public EmployeeUI(
            ProductService productService,
            OrderService orderService,
            CustomerService customerService,
            EmployeeService employeeService
        )
        {
            _productService = productService;
            _orderService = orderService;
            _customerService = customerService;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Hiển thị đăng nhập nhân viên
        /// </summary>
        public void ShowEmployeeLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP NHÂN VIÊN ===");
            Console.WriteLine("Nhân viên có sẵn:");
            var employees = _employeeService.GetAllEmployees();
            foreach (var emp in employees)
            {
                Console.WriteLine($"- {emp.Name}");
            }
            Console.Write("\nNhập tên nhân viên: ");
            var name = Console.ReadLine();

            var employee = _employeeService.LoginEmployee(name ?? "");
            if (employee != null)
            {
                DisplayHelper.DisplaySuccess($"Chào mừng {employee.Name}!");
                Console.ReadKey();
                ShowEmployeeInterface(employee);
            }
            else
            {
                DisplayHelper.DisplayError("Không tìm thấy nhân viên!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị giao diện nhân viên
        /// </summary>
        public void ShowEmployeeInterface(Employee employee)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== QUẢN LÝ CỬA HÀNG - {employee.Name} ===");
                Console.WriteLine("1. Xem danh sách sản phẩm");
                Console.WriteLine("2. Xem đơn hàng");
                Console.WriteLine("3. Quản lý khách hàng");
                Console.WriteLine("4. Quản lý rank");
                Console.WriteLine("5. Đăng xuất");
                Console.Write("\nChọn chức năng (1-5): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProductTable.ShowProductTable(_productService);
                        break;
                    case "2":
                        OrderTable.ShowOrderTable(_orderService);
                        break;
                    case "3":
                        CustomerTable.ShowCustomerTable(_customerService);
                        break;
                    case "4":
                        RankTable.ShowEmployeeRankTable(_customerService);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
