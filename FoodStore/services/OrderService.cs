using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ liên quan đến đơn hàng
    /// Bao gồm: tạo đơn hàng, quản lý chi tiết, tính toán giá và giảm giá
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// Danh sách đơn hàng trong hệ thống
        /// </summary>
        private List<Order> _orders = new List<Order>();

        /// <summary>
        /// ID tiếp theo cho đơn hàng mới (auto-increment)
        /// </summary>
        private int _nextOrderId = 1;

        /// <summary>
        /// ID tiếp theo cho chi tiết đơn hàng mới (auto-increment)
        /// </summary>
        private int _nextDetailId = 1;

        /// <summary>
        /// Tạo đơn hàng mới với khách hàng và nhân viên xử lý
        /// </summary>
        /// <param name="customer">Khách hàng đặt hàng</param>
        /// <param name="employee">Nhân viên xử lý đơn hàng</param>
        /// <returns>Đối tượng Order đã được tạo</returns>
        public Order CreateOrder(Customer customer, Employee employee)
        {
            var order = new Order
            {
                Id = _nextOrderId++, // Tự động tăng ID
                CustomerId = customer.Id,
                EmployeeId = employee.Id,
                Customer = customer, // Navigation property
                Employee = employee, // Navigation property
                Status = "Pending", // Trạng thái ban đầu
                CreatedAt = DateTime.Now, // Thời gian tạo
                TotalAmount = 0, // Tổng tiền ban đầu = 0
            };

            _orders.Add(order);
            return order;
        }

        /// <summary>
        /// Thêm sản phẩm vào đơn hàng với số lượng cụ thể
        /// </summary>
        /// <param name="order">Đơn hàng cần thêm sản phẩm</param>
        /// <param name="product">Sản phẩm cần thêm</param>
        /// <param name="quantity">Số lượng sản phẩm</param>
        public void AddOrderDetail(Order order, Product product, int quantity)
        {
            var orderDetail = new OrderDetail
            {
                Id = _nextDetailId++, // Tự động tăng ID
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = product.Price, // Giá tại thời điểm đặt hàng
                DiscountPercent = 0, // Chưa áp dụng giảm giá
                Total = product.Price * quantity, // Tổng tiền chi tiết
                Product = product, // Navigation property
            };

            order.OrderDetails.Add(orderDetail);
        }

        /// <summary>
        /// Tính tổng tiền đơn hàng với giảm giá theo cấp độ thành viên
        /// Sử dụng điểm tích lũy thực tế để xác định cấp độ
        /// </summary>
        /// <param name="order">Đơn hàng cần tính tổng tiền</param>
        /// <param name="customerService">Service để lấy thông tin cấp độ thành viên</param>
        public void CalculateTotal(Order order, CustomerService customerService)
        {
            // Tính tổng tiền trước giảm giá
            var subtotal = order.OrderDetails.Sum(od => od.Total);

            // Lấy thông tin cấp độ thành viên của khách hàng dựa trên điểm thực tế
            var customer = order.Customer;
            if (customer != null)
            {
                var currentTierId = GetTierIdByPoints(customer.Points);
                var tier = customerService.GetTier(currentTierId);
                var discountPercent = tier?.DiscountPercent ?? 0;

                if (discountPercent > 0)
                {
                    // Áp dụng giảm giá theo cấp độ thành viên
                    var discountAmount = subtotal * (decimal)(discountPercent / 100f);
                    order.TotalAmount = subtotal - discountAmount;
                }
                else
                {
                    // Không có giảm giá
                    order.TotalAmount = subtotal;
                }
            }
            else
            {
                // Không có thông tin khách hàng, không áp dụng giảm giá
                order.TotalAmount = subtotal;
            }
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
        /// Logic này phải đồng bộ với CustomerService và UIManager
        /// </summary>
        /// <param name="points">Số điểm tích lũy</param>
        /// <returns>ID cấp độ thành viên tương ứng</returns>
        private int GetTierIdByPoints(int points)
        {
            if (points >= 1000)
                return 4; // Kim Cương (10% giảm giá)
            else if (points >= 500)
                return 3; // Vàng (5% giảm giá)
            else if (points >= 100)
                return 2; // Bạc (3% giảm giá)
            else
                return 1; // Thường (0% giảm giá)
        }

        /// <summary>
        /// Lấy danh sách tất cả đơn hàng trong hệ thống
        /// </summary>
        /// <returns>Danh sách đơn hàng</returns>
        public List<Order> GetAllOrders()
        {
            return _orders.ToList();
        }

        /// <summary>
        /// Tìm đơn hàng theo ID
        /// </summary>
        /// <param name="id">ID đơn hàng cần tìm</param>
        /// <returns>Đối tượng Order nếu tìm thấy, null nếu không tồn tại</returns>
        public Order? GetOrderById(int id)
        {
            return _orders.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        /// <param name="orderId">ID đơn hàng cần cập nhật</param>
        /// <param name="status">Trạng thái mới (Pending, Processing, Completed, Cancelled)</param>
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
