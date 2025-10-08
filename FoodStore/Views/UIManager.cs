using System;
using FoodStore.Services;
using FoodStore.Utils;

namespace FoodStore.Views
{
    /// <summary>
    /// Quản lý giao diện người dùng chính
    /// </summary>
    public class UIManager
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        private readonly CustomerUI _customerUI;
        private readonly EmployeeUI _employeeUI;

        public UIManager(
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

            _customerUI = new CustomerUI(
                productService,
                orderService,
                customerService,
                employeeService
            );
            _employeeUI = new EmployeeUI(
                productService,
                orderService,
                customerService,
                employeeService
            );
        }

        /// <summary>
        /// Hiển thị menu chính
        /// </summary>
        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== HỆ THỐNG QUẢN LÝ CỬA HÀNG GẠO ===");
                Console.WriteLine("1. Đăng nhập khách hàng");
                Console.WriteLine("2. Đăng ký khách hàng mới");
                Console.WriteLine("3. Đăng nhập nhân viên");
                Console.WriteLine("4. Thoát");
                Console.Write("\nChọn chức năng (1-4): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _customerUI.ShowCustomerLogin();
                        break;
                    case "2":
                        _customerUI.ShowCustomerRegister();
                        break;
                    case "3":
                        _employeeUI.ShowEmployeeLogin();
                        break;
                    case "4":
                        Console.WriteLine("Cảm ơn bạn đã sử dụng hệ thống!");
                        return;
                    default:
                        DisplayHelper.DisplayError("Lựa chọn không hợp lệ!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
