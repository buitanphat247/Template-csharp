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
    /// EmployeeUI - Giao diện người dùng dành cho nhân viên
    /// Quản lý tất cả các chức năng quản trị: xem sản phẩm, đơn hàng, khách hàng, rank
    /// Sử dụng Dependency Injection để nhận các service cần thiết
    /// </summary>
    public class EmployeeUI
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
        public EmployeeUI(
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
        /// Hiển thị giao diện đăng nhập cho nhân viên
        /// Hiển thị danh sách nhân viên có sẵn và yêu cầu nhập tên để xác thực
        /// Sử dụng EmployeeService để xác thực thông tin đăng nhập
        /// </summary>
        public void ShowEmployeeLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP NHÂN VIÊN ===");
            Console.WriteLine("Nhân viên có sẵn:");

            // Lấy danh sách tất cả nhân viên từ EmployeeService
            var employees = _employeeService.GetAllEmployees();
            foreach (var emp in employees)
            {
                Console.WriteLine($"- {emp.Name}");
            }
            Console.Write("\nNhập tên nhân viên: ");
            var name = Console.ReadLine();

            // Sử dụng EmployeeService để xác thực đăng nhập
            var employee = _employeeService.LoginEmployee(name ?? "");
            if (employee != null)
            {
                // Đăng nhập thành công - chuyển đến giao diện quản trị
                DisplayHelper.DisplaySuccess($"Chào mừng {employee.Name}!");
                Console.ReadKey();
                ShowEmployeeInterface(employee);
            }
            else
            {
                // Đăng nhập thất bại - hiển thị lỗi
                DisplayHelper.DisplayError("Không tìm thấy nhân viên!");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Hiển thị giao diện chính dành cho nhân viên đã đăng nhập
        /// Hiển thị menu quản trị với các chức năng: xem sản phẩm, đơn hàng, khách hàng, rank
        /// Sử dụng các Table class để hiển thị dữ liệu dưới dạng bảng đẹp mắt
        /// </summary>
        /// <param name="employee">Thông tin nhân viên đã đăng nhập</param>
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
                        // Hiển thị bảng sản phẩm đầy đủ cho nhân viên quản lý
                        ProductTable.ShowProductTable(_productService);
                        break;
                    case "2":
                        // Hiển thị bảng đơn hàng với thống kê tổng thu
                        OrderTable.ShowOrderTable(_orderService);
                        break;
                    case "3":
                        // Hiển thị bảng khách hàng với thông tin cấp độ thành viên
                        CustomerTable.ShowCustomerTable(_customerService);
                        break;
                    case "4":
                        // Hiển thị bảng quản lý rank với thống kê số lượng khách hàng
                        RankTable.ShowEmployeeRankTable(_customerService);
                        break;
                    case "5":
                        // Đăng xuất - quay lại menu chính
                        return;
                    default:
                        // Xử lý lựa chọn không hợp lệ
                        Console.WriteLine("Lựa chọn không hợp lệ!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
