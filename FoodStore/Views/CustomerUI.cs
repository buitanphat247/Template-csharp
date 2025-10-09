using System;
using System.Linq;
using FoodStore.Models;
using FoodStore.Services;
using FoodStore.Utils;
using Spectre.Console;

namespace FoodStore.Views
{
    /// <summary>
    /// CustomerUI - Giao diện người dùng dành cho khách hàng
    /// Quản lý tất cả các chức năng liên quan đến khách hàng: đăng nhập, đăng ký, mua sắm
    /// Sử dụng Dependency Injection để nhận các service cần thiết
    /// </summary>
    public class CustomerUI
    {
        /// <summary>
        /// Service quản lý sản phẩm - được inject từ UIManager
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Service quản lý đơn hàng - được inject từ UIManager
        /// </summary>
        private readonly OrderService _orderService;

        /// <summary>
        /// Service quản lý khách hàng - được inject từ UIManager
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Service quản lý nhân viên - được inject từ UIManager
        /// </summary>
        private readonly EmployeeService _employeeService;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận tất cả các service cần thiết từ UIManager
        /// </summary>
        /// <param name="productService">Service quản lý sản phẩm</param>
        /// <param name="orderService">Service quản lý đơn hàng</param>
        /// <param name="customerService">Service quản lý khách hàng</param>
        /// <param name="employeeService">Service quản lý nhân viên</param>
        public CustomerUI(
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
        }

        /// <summary>
        /// Hiển thị giao diện đăng nhập cho khách hàng
        /// Yêu cầu nhập số điện thoại để xác thực danh tính
        /// Sử dụng CustomerService để xác thực thông tin đăng nhập
        /// </summary>
        public void ShowCustomerLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP KHÁCH HÀNG ===");
            Console.Write("Nhập số điện thoại: ");
            var phone = Console.ReadLine();

            // Sử dụng CustomerService để xác thực đăng nhập
            var customer = _customerService.LoginCustomer(phone ?? "");
            if (customer != null)
            {
                // Đăng nhập thành công - chuyển đến giao diện chính
                DisplayHelper.DisplaySuccess($"Chào mừng {customer.Name}!");
                Console.ReadKey();
                ShowCustomerInterface(customer);
            }
            else
            {
                // Đăng nhập thất bại - hiển thị lỗi
                DisplayHelper.DisplayError("Không tìm thấy khách hàng!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị giao diện đăng ký khách hàng mới
        /// Thu thập thông tin cơ bản: họ tên và số điện thoại
        /// Sử dụng CustomerService để tạo tài khoản mới
        /// </summary>
        public void ShowCustomerRegister()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG KÝ KHÁCH HÀNG MỚI ===");
            Console.Write("Nhập họ tên: ");
            var name = Console.ReadLine();
            Console.Write("Nhập số điện thoại: ");
            var phone = Console.ReadLine();

            // Kiểm tra thông tin đầu vào có hợp lệ không
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
            {
                // Sử dụng CustomerService để đăng ký khách hàng mới
                var customer = _customerService.RegisterCustomer(name, phone);
                DisplayHelper.DisplaySuccess($"Đăng ký thành công! Chào mừng {customer.Name}!");
                Console.ReadKey();
                // Chuyển đến giao diện chính sau khi đăng ký thành công
                ShowCustomerInterface(customer);
            }
            else
            {
                // Hiển thị lỗi nếu thông tin không hợp lệ
                DisplayHelper.DisplayError("Thông tin không hợp lệ!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị giao diện chính dành cho khách hàng đã đăng nhập
        /// Hiển thị menu với các chức năng: mua sắm, xem profile, xem rank, đăng xuất
        /// Sử dụng Dependency Injection để tạo các UI component con
        /// </summary>
        /// <param name="customer">Thông tin khách hàng đã đăng nhập</param>
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
                        // Tạo ShoppingUI với tất cả service dependencies
                        var shoppingUI = new ShoppingUI(
                            _productService,
                            _orderService,
                            _customerService,
                            _employeeService
                        );
                        shoppingUI.ShowShoppingInterface(customer);
                        break;
                    case "2":
                        // Tạo ProfileUI với OrderService và CustomerService
                        var profileUI = new ProfileUI(_orderService, _customerService);
                        profileUI.ShowCustomerProfile(customer);
                        break;
                    case "3":
                        // Tạo RankUI với CustomerService
                        var rankUI = new RankUI(_customerService);
                        rankUI.ShowCustomerRank(customer);
                        break;
                    case "4":
                        // Đăng xuất - quay lại menu chính
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
