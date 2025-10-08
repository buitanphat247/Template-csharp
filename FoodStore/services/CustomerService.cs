using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic khách hàng
    /// </summary>
    public class CustomerService
    {
        private List<Customer> _customers = new List<Customer>();
        private List<MemberTier> _tiers = new List<MemberTier>();
        private int _nextCustomerId = 1;

        public CustomerService()
        {
            // Khởi tạo cấp độ thành viên
            _tiers.Add(
                new MemberTier
                {
                    Id = 1,
                    Name = "Thường",
                    DiscountPercent = 0,
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 2,
                    Name = "Bạc",
                    DiscountPercent = 3,
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 3,
                    Name = "Vàng",
                    DiscountPercent = 5,
                }
            );
            _tiers.Add(
                new MemberTier
                {
                    Id = 4,
                    Name = "Kim Cương",
                    DiscountPercent = 10,
                }
            );
        }

        /// <summary>
        /// Đăng ký khách hàng mới
        /// </summary>
        public Customer RegisterCustomer(string name, string phone, int tierId = 1)
        {
            var customer = new Customer
            {
                Id = _nextCustomerId++,
                Name = name,
                Phone = phone,
                TierId = tierId,
                Points = 0,
            };

            _customers.Add(customer);
            return customer;
        }

        /// <summary>
        /// Đăng nhập khách hàng
        /// </summary>
        public Customer? LoginCustomer(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            // Loại bỏ khoảng trắng, xuống dòng và ký tự đặc biệt
            var cleanedPhone = phone.Trim().Replace("\r", "").Replace("\n", "");
            var customer = _customers.FirstOrDefault(c => c.Phone.Trim() == cleanedPhone);

            // Đồng bộ hóa TierId khi đăng nhập
            if (customer != null)
            {
                var correctTierId = GetTierIdByPoints(customer.Points);
                if (customer.TierId != correctTierId)
                {
                    customer.TierId = correctTierId;
                }
            }

            return customer;
        }

        /// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        public List<Customer> GetAllCustomers()
        {
            return _customers.ToList();
        }

        /// <summary>
        /// Lấy cấp độ thành viên
        /// </summary>
        public MemberTier? GetTier(int tierId)
        {
            return _tiers.FirstOrDefault(t => t.Id == tierId);
        }

        /// <summary>
        /// Cập nhật điểm tích lũy và tự động cập nhật rank
        /// </summary>
        public void UpdatePoints(int customerId, int points)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                customer.Points += points;
                UpdateCustomerTier(customer);
            }
        }

        /// <summary>
        /// Tự động cập nhật rank dựa trên điểm tích lũy
        /// </summary>
        private void UpdateCustomerTier(Customer customer)
        {
            var newTierId = GetTierIdByPoints(customer.Points);
            if (newTierId > customer.TierId)
            {
                customer.TierId = newTierId;
                var newTier = GetTier(newTierId);
                Console.WriteLine($"[RANK UP] {customer.Name} đã lên rank {newTier?.Name}!");
            }
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

        /// <summary>
        /// Đồng bộ hóa TierId của tất cả khách hàng dựa trên điểm thực tế
        /// </summary>
        public void SyncAllCustomerTiers()
        {
            foreach (var customer in _customers)
            {
                var correctTierId = GetTierIdByPoints(customer.Points);
                if (customer.TierId != correctTierId)
                {
                    customer.TierId = correctTierId;
                }
            }
        }
    }
}
