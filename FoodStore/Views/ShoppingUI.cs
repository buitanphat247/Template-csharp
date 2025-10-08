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
    /// Giao diện mua sắm
    /// </summary>
    public class ShoppingUI
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly EmployeeService _employeeService;
        private readonly ReceiptUI _receiptUI;

        public ShoppingUI(
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
            _receiptUI = new ReceiptUI(customerService);
        }

        /// <summary>
        /// Hiển thị giao diện mua sắm
        /// </summary>
        public void ShowShoppingInterface(Customer customer)
        {
            var employee = new Employee
            {
                Id = 1,
                Name = "Nhân viên",
                RoleId = 1,
            };
            var currentOrder = _orderService.CreateOrder(customer, employee);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== MUA SẮM - {customer.Name} ===");
                Console.WriteLine($"Điểm tích lũy: {customer.Points}");
                Console.WriteLine();

                // Hiển thị sản phẩm có sẵn
                ProductTable.ShowShoppingProductTable(_productService);

                // Hiển thị giỏ hàng hiện tại
                OrderTable.ShowCartTable(currentOrder);
                
                if (currentOrder.OrderDetails.Any())
                {
                    _orderService.CalculateTotal(currentOrder, _customerService);
                    Console.WriteLine(
                        $"Tổng: {DisplayHelper.FormatCurrency(currentOrder.TotalAmount)}"
                    );
                }

                Console.WriteLine("\nChọn sản phẩm (ID) để thêm vào giỏ hàng:");
                Console.WriteLine("Nhập 0 để thanh toán");
                Console.WriteLine("Nhập -1 để quay lại menu chính");
                Console.Write("Lựa chọn: ");

                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    if (choice == 0)
                    {
                        // Thanh toán
                        if (currentOrder.OrderDetails.Any())
                        {
                            ProcessPayment(currentOrder, customer);
                            return; // Quay lại menu chính
                        }
                        else
                        {
                            Console.WriteLine("Giỏ hàng trống!");
                            Console.ReadKey();
                        }
                    }
                    else if (choice == -1)
                    {
                        return;
                    }
                    else
                    {
                        // Thêm sản phẩm vào giỏ hàng
                        var product = _productService.GetProductById(choice);
                        if (product != null)
                        {
                            Console.Write($"Nhập số lượng {product.Name}: ");
                            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                            {
                                if (_productService.CheckStock(product.Id, quantity))
                                {
                                    _orderService.AddOrderDetail(currentOrder, product, quantity);
                                    _productService.UpdateStock(product.Id, quantity);
                                    DisplayHelper.DisplaySuccess(
                                        $"Đã thêm {product.Name} x{quantity} vào giỏ hàng!"
                                    );
                                }
                                else
                                {
                                    DisplayHelper.DisplayError("Không đủ hàng trong kho!");
                                }
                            }
                            else
                            {
                                DisplayHelper.DisplayError("Số lượng không hợp lệ!");
                            }
                        }
                        else
                        {
                            DisplayHelper.DisplayError("Không tìm thấy sản phẩm!");
                        }
                        Console.ReadKey();
                    }
                }
                else
                {
                    DisplayHelper.DisplayError("Lựa chọn không hợp lệ!");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Xử lý thanh toán
        /// </summary>
        private void ProcessPayment(Order order, Customer customer)
        {
            Console.Clear();

            // Lấy thông tin rank dựa trên điểm thực tế (đồng bộ với menu rank)
            var currentTierId = GetTierIdByPoints(customer.Points);
            var tier = _customerService.GetTier(currentTierId);
            var discountPercent = tier?.DiscountPercent ?? 0;
            var subtotal = order.OrderDetails.Sum(od => od.Total);
            var discountAmount = subtotal * (decimal)(discountPercent / 100f);

            // Tính tổng tiền với discount
            order.TotalAmount = subtotal - discountAmount;

            // Cập nhật trạng thái đơn hàng
            _orderService.UpdateOrderStatus(order.Id, "Confirmed");

            // Tính điểm tích lũy (1 điểm = 1000 VNĐ) - SAU khi thanh toán
            var points = (int)(order.TotalAmount / 1000);
            _customerService.UpdatePoints(order.CustomerId, points);

            // Cập nhật trạng thái thanh toán
            _orderService.UpdateOrderStatus(order.Id, "Paid");

            // Hiển thị hóa đơn đẹp
            ReceiptTable.ShowReceiptTable(order, _customerService);
        }

        /// <summary>
        /// Lấy rank ID dựa trên điểm tích lũy
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
    }
}
