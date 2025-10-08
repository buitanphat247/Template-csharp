using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views
{
    /// <summary>
    /// Giao diện khách hàng
    /// </summary>
    public class CustomerUI
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;

        public CustomerUI(
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
        /// Hiển thị đăng nhập khách hàng
        /// </summary>
        public void ShowCustomerLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP KHÁCH HÀNG ===");
            Console.Write("Nhập số điện thoại: ");
            var phone = Console.ReadLine();

            var customer = _customerService.LoginCustomer(phone ?? "");
            if (customer != null)
            {
                DisplayHelper.DisplaySuccess($"Chào mừng {customer.Name}!");
                Console.ReadKey();
                ShowCustomerInterface(customer);
            }
            else
            {
                DisplayHelper.DisplayError("Không tìm thấy khách hàng!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị đăng ký khách hàng
        /// </summary>
        public void ShowCustomerRegister()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG KÝ KHÁCH HÀNG MỚI ===");
            Console.Write("Nhập họ tên: ");
            var name = Console.ReadLine();
            Console.Write("Nhập số điện thoại: ");
            var phone = Console.ReadLine();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
            {
                var customer = _customerService.RegisterCustomer(name, phone);
                DisplayHelper.DisplaySuccess($"Đăng ký thành công! Chào mừng {customer.Name}!");
                Console.ReadKey();
                ShowCustomerInterface(customer);
            }
            else
            {
                DisplayHelper.DisplayError("Thông tin không hợp lệ!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị giao diện khách hàng
        /// </summary>
        public void ShowCustomerInterface(Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== CỬA HÀNG GẠO - {customer.Name} ===");
                Console.WriteLine($"Điểm tích lũy: {customer.Points}");
                Console.WriteLine();
                Console.WriteLine("1. Mua sắm");
                Console.WriteLine("2. Xem profile");
                Console.WriteLine("3. Xem rank");
                Console.WriteLine("4. Đăng xuất");
                Console.Write("\nChọn chức năng (1-4): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var shoppingUI = new ShoppingUI(
                            _productService,
                            _orderService,
                            _customerService,
                            _employeeService
                        );
                        shoppingUI.ShowShoppingInterface(customer);
                        break;
                    case "2":
                        var profileUI = new ProfileUI(_orderService, _customerService);
                        profileUI.ShowCustomerProfile(customer);
                        break;
                    case "3":
                        var rankUI = new RankUI(_customerService);
                        rankUI.ShowCustomerRank(customer);
                        break;
                    case "4":
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
