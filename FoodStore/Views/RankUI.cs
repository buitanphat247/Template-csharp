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
    /// RankUI - Giao diện hiển thị hệ thống cấp độ thành viên
    /// Quản lý việc hiển thị thông tin rank hiện tại và rank tiếp theo của khách hàng
    /// Sử dụng Dependency Injection để nhận CustomerService
    /// </summary>
    public class RankUI
    {
        /// <summary>
        /// Service quản lý khách hàng - được inject từ CustomerUI
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận CustomerService từ CustomerUI
        /// </summary>
        /// <param name="customerService">Service quản lý khách hàng</param>
        public RankUI(CustomerService customerService)
        {
            // Lưu trữ service được inject
            _customerService = customerService;
        }

        /// <summary>
        /// Hiển thị thông tin cấp độ thành viên cho khách hàng
        /// Bao gồm rank hiện tại, rank tiếp theo và số điểm cần thiết để lên cấp
        /// Sử dụng RankTable để hiển thị dữ liệu dưới dạng bảng đẹp mắt
        /// </summary>
        /// <param name="customer">Thông tin khách hàng cần xem rank</param>
        public void ShowCustomerRank(Customer customer)
        {
            // Sử dụng RankTable để hiển thị bảng rank với thông tin chi tiết
            RankTable.ShowCustomerRankTable(customer, _customerService);
        }
    }
}
