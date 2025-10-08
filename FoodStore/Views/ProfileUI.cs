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
    /// Giao diện profile khách hàng
    /// </summary>
    public class ProfileUI
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;

        public ProfileUI(OrderService orderService, CustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        /// <summary>
        /// Hiển thị profile khách hàng
        /// </summary>
        public void ShowCustomerProfile(Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== PROFILE CÁ NHÂN ===");
                Console.WriteLine(DisplayHelper.CreateSeparator(50));

                // Hiển thị bảng profile
                CustomerTable.ShowProfileTable(customer, _customerService, _orderService);

                // Hiển thị lịch sử đơn hàng
                CustomerTable.ShowOrderHistoryTable(_orderService, customer);

                Console.WriteLine(DisplayHelper.CreateSeparator(50));
                Console.WriteLine("Nhấn phím bất kỳ để quay lại...");
                Console.ReadKey();
                return;
            }
        }
    }
}
