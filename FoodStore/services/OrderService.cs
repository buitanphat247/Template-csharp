using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic đơn hàng
    /// </summary>
    public class OrderService
    {
        private List<Order> _orders = new List<Order>();
        private int _nextOrderId = 1;
        private int _nextDetailId = 1;

        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        public Order CreateOrder(Customer customer, Employee employee)
        {
            var order = new Order
            {
                Id = _nextOrderId++,
                CustomerId = customer.Id,
                EmployeeId = employee.Id,
                Customer = customer,
                Employee = employee,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                TotalAmount = 0,
            };

            _orders.Add(order);
            return order;
        }

        /// <summary>
        /// Thêm chi tiết đơn hàng
        /// </summary>
        public void AddOrderDetail(Order order, Product product, int quantity)
        {
            var orderDetail = new OrderDetail
            {
                Id = _nextDetailId++,
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = product.Price,
                DiscountPercent = 0,
                Total = product.Price * quantity,
                Product = product,
            };

            order.OrderDetails.Add(orderDetail);
        }

        /// <summary>
        /// Tính tổng tiền đơn hàng với discount theo rank (dựa trên điểm thực tế)
        /// </summary>
        public void CalculateTotal(Order order, CustomerService customerService)
        {
            var subtotal = order.OrderDetails.Sum(od => od.Total);

            // Lấy thông tin rank của khách hàng dựa trên điểm thực tế
            var customer = order.Customer;
            if (customer != null)
            {
                var currentTierId = GetTierIdByPoints(customer.Points);
                var tier = customerService.GetTier(currentTierId);
                var discountPercent = tier?.DiscountPercent ?? 0;

                if (discountPercent > 0)
                {
                    var discountAmount = subtotal * (decimal)(discountPercent / 100f);
                    order.TotalAmount = subtotal - discountAmount;
                }
                else
                {
                    order.TotalAmount = subtotal;
                }
            }
            else
            {
                order.TotalAmount = subtotal;
            }
        }

        /// <summary>
        /// Lấy rank ID dựa trên điểm tích lũy (đồng bộ với logic trong UIManager)
        /// </summary>
        private int GetTierIdByPoints(int points)
        {
            if (points >= 1000)
                return 4; // Kim Cương
            else if (points >= 500)
                return 3; // Vàng
            else if (points >= 100)
                return 2; // Bạc
            else
                return 1; // Thường
        }

        /// <summary>
        /// Lấy tất cả đơn hàng
        /// </summary>
        public List<Order> GetAllOrders()
        {
            return _orders.ToList();
        }

        /// <summary>
        /// Lấy đơn hàng theo ID
        /// </summary>
        public Order? GetOrderById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = status;
            }
        }
    }
}
