using System;
using FoodStore.Services;
using FoodStore.Utils;

namespace FoodStore.Views
{
    /// <summary>
    /// UIManager - Quản lý giao diện người dùng chính của hệ thống
    /// Đây là entry point chính cho tất cả các giao diện trong ứng dụng
    /// Sử dụng Dependency Injection để quản lý các service dependencies
    /// </summary>
    public class UIManager
    {
        /// <summary>
        /// Service quản lý sản phẩm - được inject từ bên ngoài
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Service quản lý đơn hàng - được inject từ bên ngoài
        /// </summary>
        private readonly OrderService _orderService;

        /// <summary>
        /// Service quản lý khách hàng - được inject từ bên ngoài
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Service quản lý nhân viên - được inject từ bên ngoài
        /// </summary>
        private readonly EmployeeService _employeeService;

        /// <summary>
        /// UI component cho khách hàng - được khởi tạo trong constructor
        /// </summary>
        private readonly CustomerUI _customerUI;

        /// <summary>
        /// UI component cho nhân viên - được khởi tạo trong constructor
        /// </summary>
        private readonly EmployeeUI _employeeUI;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận tất cả các service cần thiết từ bên ngoài
        /// Khởi tạo các UI component với các service đã inject
        /// </summary>
        /// <param name="productService">Service quản lý sản phẩm</param>
        /// <param name="orderService">Service quản lý đơn hàng</param>
        /// <param name="customerService">Service quản lý khách hàng</param>
        /// <param name="employeeService">Service quản lý nhân viên</param>
        public UIManager(
            ProductService productService,
            OrderService orderService,
            CustomerService customerService,
            EmployeeService employeeService
        )
        {
            // Lưu trữ các service được inject
            _productService = productService;
            _orderService = orderService;
            _customerService = customerService;
            _employeeService = employeeService;

            // Khởi tạo CustomerUI với tất cả service dependencies
            _customerUI = new CustomerUI(
                productService,
                orderService,
                customerService,
                employeeService
            );

            // Khởi tạo EmployeeUI với tất cả service dependencies
            _employeeUI = new EmployeeUI(
                productService,
                orderService,
                customerService,
                employeeService
            );
        }

        /// <summary>
        /// Hiển thị menu chính của hệ thống
        /// Đây là entry point chính, hiển thị các tùy chọn đăng nhập/đăng ký
        /// Sử dụng vòng lặp while(true) để duy trì menu cho đến khi người dùng thoát
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
                        // Chuyển đến giao diện đăng nhập khách hàng
                        _customerUI.ShowCustomerLogin();
                        break;
                    case "2":
                        // Chuyển đến giao diện đăng ký khách hàng mới
                        _customerUI.ShowCustomerRegister();
                        break;
                    case "3":
                        // Chuyển đến giao diện đăng nhập nhân viên
                        _employeeUI.ShowEmployeeLogin();
                        break;
                    case "4":
                        // Thoát khỏi ứng dụng
                        Console.WriteLine("Cảm ơn bạn đã sử dụng hệ thống!");
                        return;
                    default:
                        // Xử lý lựa chọn không hợp lệ
                        DisplayHelper.DisplayError("Lựa chọn không hợp lệ!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
