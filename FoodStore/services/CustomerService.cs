using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ liên quan đến khách hàng
    /// Bao gồm: đăng ký, đăng nhập, quản lý cấp độ thành viên và điểm tích lũy
    /// </summary>
    public class CustomerService
    {
        /// <summary>
        /// Danh sách khách hàng trong hệ thống
        /// </summary>
        private List<Customer> _customers = new List<Customer>();

        /// <summary>
        /// Danh sách cấp độ thành viên có sẵn
        /// </summary>
        private List<MemberTier> _tiers = new List<MemberTier>();

        /// <summary>
        /// ID tiếp theo cho khách hàng mới (auto-increment)
        /// </summary>
        private int _nextCustomerId = 1;

        /// <summary>
        /// Constructor - Khởi tạo service và dữ liệu mẫu
        /// </summary>
        public CustomerService()
        {
            // Khởi tạo các cấp độ thành viên với mức giảm giá tương ứng
            _tiers.Add(
                new MemberTier
                {
                    Id = 1,
                    Name = "Thường",
                    DiscountPercent = 0, // Không giảm giá
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 2,
                    Name = "Bạc",
                    DiscountPercent = 3, // 3% giảm giá
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 3,
                    Name = "Vàng",
                    DiscountPercent = 5, // 5% giảm giá
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 4,
                    Name = "Kim Cương",
                    DiscountPercent = 10, // 10% giảm giá
                }
            );
        }

        /// <summary>
        /// Đăng ký khách hàng mới vào hệ thống
        /// </summary>
        /// <param name="name">Tên khách hàng</param>
        /// <param name="phone">Số điện thoại (dùng để đăng nhập)</param>
        /// <param name="tierId">ID cấp độ thành viên (mặc định = 1 - Thường)</param>
        /// <returns>Đối tượng Customer đã được tạo</returns>
        public Customer RegisterCustomer(string name, string phone, int tierId = 1)
        {
            var customer = new Customer
            {
                Id = _nextCustomerId++, // Tự động tăng ID
                Name = name,
                Phone = phone,
                TierId = tierId, // Mặc định là cấp độ "Thường"
                Points = 0, // Điểm tích lũy ban đầu = 0
            };

            _customers.Add(customer);
            return customer;
        }

        /// <summary>
        /// Đăng nhập khách hàng bằng số điện thoại
        /// Tự động đồng bộ cấp độ thành viên dựa trên điểm tích lũy thực tế
        /// </summary>
        /// <param name="phone">Số điện thoại để đăng nhập</param>
        /// <returns>Đối tượng Customer nếu tìm thấy, null nếu không tồn tại</returns>
        public Customer? LoginCustomer(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            // Làm sạch số điện thoại: loại bỏ khoảng trắng, xuống dòng và ký tự đặc biệt
            var cleanedPhone = phone.Trim().Replace("\r", "").Replace("\n", "");
            var customer = _customers.FirstOrDefault(c => c.Phone.Trim() == cleanedPhone);

            // Đồng bộ hóa TierId khi đăng nhập dựa trên điểm tích lũy thực tế
            if (customer != null)
            {
                var correctTierId = GetTierIdByPoints(customer.Points);
                if (customer.TierId != correctTierId)
                {
                    customer.TierId = correctTierId; // Cập nhật cấp độ nếu có thay đổi
                }
            }

            return customer;
        }

        /// <summary>
        /// Lấy danh sách tất cả khách hàng trong hệ thống
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        public List<Customer> GetAllCustomers()
        {
            return _customers.ToList();
        }

        /// <summary>
        /// Lấy thông tin cấp độ thành viên theo ID
        /// </summary>
        /// <param name="tierId">ID cấp độ thành viên</param>
        /// <returns>Đối tượng MemberTier nếu tìm thấy, null nếu không tồn tại</returns>
        public MemberTier? GetTier(int tierId)
        {
            return _tiers.FirstOrDefault(t => t.Id == tierId);
        }

        /// <summary>
        /// Cập nhật điểm tích lũy cho khách hàng và tự động kiểm tra nâng cấp cấp độ
        /// </summary>
        /// <param name="customerId">ID khách hàng</param>
        /// <param name="points">Số điểm cần cộng thêm (có thể âm để trừ điểm)</param>
        public void UpdatePoints(int customerId, int points)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                customer.Points += points; // Cộng điểm vào tổng điểm hiện tại
                UpdateCustomerTier(customer); // Kiểm tra và cập nhật cấp độ
            }
        }

        /// <summary>
        /// Tự động cập nhật cấp độ thành viên dựa trên điểm tích lũy
        /// Chỉ nâng cấp khi điểm đủ điều kiện, không hạ cấp
        /// </summary>
        /// <param name="customer">Khách hàng cần kiểm tra cấp độ</param>
        private void UpdateCustomerTier(Customer customer)
        {
            var newTierId = GetTierIdByPoints(customer.Points);
            if (newTierId > customer.TierId) // Chỉ nâng cấp, không hạ cấp
            {
                customer.TierId = newTierId;
                var newTier = GetTier(newTierId);
                Console.WriteLine($"[RANK UP] {customer.Name} đã lên rank {newTier?.Name}!");
            }
        }

        /// <summary>
        /// Xác định cấp độ thành viên dựa trên điểm tích lũy
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
        /// Đồng bộ hóa cấp độ thành viên của tất cả khách hàng dựa trên điểm tích lũy thực tế
        /// Sử dụng khi cần đảm bảo tính nhất quán dữ liệu
        /// </summary>
        public void SyncAllCustomerTiers()
        {
            foreach (var customer in _customers)
            {
                var correctTierId = GetTierIdByPoints(customer.Points);
                if (customer.TierId != correctTierId)
                {
                    customer.TierId = correctTierId; // Cập nhật cấp độ cho đúng
                }
            }
        }
    }
}
