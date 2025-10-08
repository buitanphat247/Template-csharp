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
    /// Giao diện rank thành viên
    /// </summary>
    public class RankUI
    {
        private readonly CustomerService _customerService;

        public RankUI(CustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Hiển thị rank cho khách hàng
        /// </summary>
        public void ShowCustomerRank(Customer customer)
        {
            RankTable.ShowCustomerRankTable(customer, _customerService);
        }

    }
}
