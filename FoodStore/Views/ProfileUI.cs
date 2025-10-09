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
    /// ProfileUI - Giao diện hiển thị profile cá nhân của khách hàng
    /// Quản lý việc hiển thị thông tin cá nhân, cấp độ thành viên và lịch sử đơn hàng
    /// Sử dụng Dependency Injection để nhận các service cần thiết
    /// </summary>
    public class ProfileUI
    {
        /// <summary>
        /// Service quản lý đơn hàng - được inject từ CustomerUI
        /// </summary>
        private readonly OrderService _orderService;

        /// <summary>
        /// Service quản lý khách hàng - được inject từ CustomerUI
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận OrderService và CustomerService từ CustomerUI
        /// </summary>
        /// <param name="orderService">Service quản lý đơn hàng</param>
        /// <param name="customerService">Service quản lý khách hàng</param>
        public ProfileUI(OrderService orderService, CustomerService customerService)
        {
            // Lưu trữ các service được inject
            _orderService = orderService;
            _customerService = customerService;
        }

        /// <summary>
        /// Hiển thị profile cá nhân đầy đủ của khách hàng
        /// Bao gồm thông tin cá nhân, cấp độ thành viên và lịch sử đơn hàng
        /// Sử dụng CustomerTable để hiển thị dữ liệu dưới dạng bảng đẹp mắt
        /// </summary>
        /// <param name="customer">Thông tin khách hàng cần hiển thị profile</param>
        public void ShowCustomerProfile(Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PROFILE CÁ NHÂN ===");
                Console.WriteLine(DisplayHelper.CreateSeparator(50));

                // Hiển thị bảng profile chi tiết với thông tin cá nhân và thống kê mua hàng
                CustomerTable.ShowProfileTable(customer, _customerService, _orderService);

                // Hiển thị lịch sử 5 đơn hàng gần nhất đã thanh toán
                CustomerTable.ShowOrderHistoryTable(_orderService, customer);

                Console.WriteLine(DisplayHelper.CreateSeparator(50));
                Console.WriteLine("Nhấn phím bất kỳ để quay lại...");
                Console.ReadKey();
                return;
            }
        }
    }
}
