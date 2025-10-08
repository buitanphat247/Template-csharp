using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic nhân viên
    /// </summary>
    public class EmployeeService
    {
        private List<Employee> _employees = new List<Employee>();
        private List<Role> _roles = new List<Role>();

        public EmployeeService()
        {
            // Khởi tạo vai trò
            _roles.Add(
                new Role
                {
                    Id = 1,
                    Name = "Nhân viên bán hàng",
                    Description = "Bán hàng và tư vấn khách hàng",
                }
            );
            _roles.Add(
                new Role
                {
                    Id = 2,
                    Name = "Quản lý",
                    Description = "Quản lý cửa hàng và nhân viên",
                }
            );
            _roles.Add(
                new Role
                {
                    Id = 3,
                    Name = "Kế toán",
                    Description = "Quản lý tài chính và báo cáo",
                }
            );

            // Thêm nhân viên mẫu
            _employees.Add(
                new Employee
                {
                    Id = 1,
                    Name = "aa",
                    RoleId = 1,
                }
            );
            _employees.Add(
                new Employee
                {
                    Id = 2,
                    Name = "bb",
                    RoleId = 2,
                }
            );
            _employees.Add(
                new Employee
                {
                    Id = 3,
                    Name = "cc",
                    RoleId = 3,
                }
            );
        }

        /// <summary>
        /// Đăng nhập nhân viên
        /// </summary>
        public Employee? LoginEmployee(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            // Loại bỏ khoảng trắng, xuống dòng và ký tự đặc biệt
            var cleanedName = name.Trim().Replace("\r", "").Replace("\n", "");
            return _employees.FirstOrDefault(e =>
                string.Equals(e.Name.Trim(), cleanedName, StringComparison.OrdinalIgnoreCase)
            );
        }

        /// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            return _employees.ToList();
        }

        /// <summary>
        /// Lấy vai trò
        /// </summary>
        public Role? GetRole(int roleId)
        {
            return _roles.FirstOrDefault(r => r.Id == roleId);
        }
    }
}
