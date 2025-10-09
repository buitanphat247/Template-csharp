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
    /// ShoppingUI - Giao diện mua sắm dành cho khách hàng
    /// Quản lý quá trình mua sắm: xem sản phẩm, thêm vào giỏ hàng, thanh toán
    /// Sử dụng Dependency Injection để nhận các service cần thiết
    /// Tự động tạo ReceiptUI để hiển thị hóa đơn
    /// </summary>
    public class ShoppingUI
    {
        /// <summary>
        /// Service quản lý sản phẩm - được inject từ CustomerUI
        /// </summary>
        private readonly ProductService _productService;

        /// <summary>
        /// Service quản lý đơn hàng - được inject từ CustomerUI
        /// </summary>
        private readonly OrderService _orderService;

        /// <summary>
        /// Service quản lý khách hàng - được inject từ CustomerUI
        /// </summary>
        private readonly CustomerService _customerService;

        /// <summary>
        /// Service quản lý nhân viên - được inject từ CustomerUI
        /// </summary>
        private readonly EmployeeService _employeeService;

        /// <summary>
        /// UI component hiển thị hóa đơn - được khởi tạo trong constructor
        /// </summary>
        private readonly ReceiptUI _receiptUI;

        /// <summary>
        /// Constructor với Dependency Injection
        /// Nhận tất cả các service cần thiết từ CustomerUI
        /// Tự động khởi tạo ReceiptUI với CustomerService
        /// </summary>
        /// <param name="productService">Service quản lý sản phẩm</param>
        /// <param name="orderService">Service quản lý đơn hàng</param>
        /// <param name="customerService">Service quản lý khách hàng</param>
        /// <param name="employeeService">Service quản lý nhân viên</param>
        public ShoppingUI(
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

            // Khởi tạo ReceiptUI với CustomerService
            _receiptUI = new ReceiptUI(customerService);
        }

        /// <summary>
        /// Hiển thị giao diện mua sắm chính cho khách hàng
        /// Tạo đơn hàng mới, hiển thị sản phẩm, giỏ hàng và xử lý thanh toán
        /// Sử dụng các Table class để hiển thị dữ liệu đẹp mắt
        /// </summary>
        /// <param name="customer">Thông tin khách hàng đang mua sắm</param>
        public void ShowShoppingInterface(Customer customer)
        {
            // Tạo nhân viên mặc định cho đơn hàng (có thể mở rộng trong tương lai)
            var employee = new Employee
            {
                Id = 1,
                Name = "Nhân viên",
                RoleId = 1,
            };

            // Tạo đơn hàng mới cho khách hàng
            var currentOrder = _orderService.CreateOrder(customer, employee);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== MUA SẮM - {customer.Name} ===");
                Console.WriteLine($"Điểm tích lũy: {customer.Points}");
                Console.WriteLine();

                // Hiển thị bảng sản phẩm đơn giản cho khách hàng
                ProductTable.ShowShoppingProductTable(_productService);

                // Hiển thị giỏ hàng hiện tại
                OrderTable.ShowCartTable(currentOrder);

                // Tính và hiển thị tổng tiền nếu có sản phẩm trong giỏ hàng
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
                        // Xử lý thanh toán
                        if (currentOrder.OrderDetails.Any())
                        {
                            ProcessPayment(currentOrder, customer);
                            return; // Quay lại menu chính sau khi thanh toán
                        }
                        else
                        {
                            Console.WriteLine("Giỏ hàng trống!");
                            Console.ReadKey();
                        }
                    }
                    else if (choice == -1)
                    {
                        // Quay lại menu chính mà không thanh toán
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
                                // Kiểm tra tồn kho trước khi thêm vào giỏ hàng
                                if (_productService.CheckStock(product.Id, quantity))
                                {
                                    // Thêm sản phẩm vào đơn hàng
                                    _orderService.AddOrderDetail(currentOrder, product, quantity);
                                    // Cập nhật tồn kho
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
        /// Xử lý quá trình thanh toán cho đơn hàng
        /// Tính chiết khấu dựa trên cấp độ thành viên, cập nhật điểm tích lũy
        /// Hiển thị hóa đơn đẹp mắt sau khi thanh toán thành công
        /// </summary>
        /// <param name="order">Đơn hàng cần thanh toán</param>
        /// <param name="customer">Khách hàng thực hiện thanh toán</param>
        private void ProcessPayment(Order order, Customer customer)
        {
            Console.Clear();

            // Lấy thông tin cấp độ thành viên dựa trên điểm tích lũy thực tế
            var currentTierId = GetTierIdByPoints(customer.Points);
            var tier = _customerService.GetTier(currentTierId);
            var discountPercent = tier?.DiscountPercent ?? 0;
            var subtotal = order.OrderDetails.Sum(od => od.Total);
            var discountAmount = subtotal * (decimal)(discountPercent / 100f);

            // Tính tổng tiền cuối cùng sau khi áp dụng chiết khấu
            order.TotalAmount = subtotal - discountAmount;

            // Cập nhật trạng thái đơn hàng thành "Confirmed"
            _orderService.UpdateOrderStatus(order.Id, "Confirmed");

            // Tính điểm tích lũy (1 điểm = 1000 VNĐ) - chỉ tính SAU khi thanh toán
            var points = (int)(order.TotalAmount / 1000);
            _customerService.UpdatePoints(order.CustomerId, points);

            // Cập nhật trạng thái đơn hàng thành "Paid"
            _orderService.UpdateOrderStatus(order.Id, "Paid");

            // Hiển thị hóa đơn đẹp mắt với ReceiptTable
            ReceiptTable.ShowReceiptTable(order, _customerService);
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
        /// Logic này phải đồng bộ với CustomerService và các UI khác
        /// </summary>
        /// <param name="points">Số điểm tích lũy của khách hàng</param>
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
    }
}
