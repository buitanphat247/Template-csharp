using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ liên quan đến nhân viên
    /// Bao gồm: đăng nhập, quản lý vai trò và phân quyền
    /// </summary>
    public class EmployeeService
    {
        /// <summary>
        /// Danh sách nhân viên trong hệ thống
        /// </summary>
        private List<Employee> _employees = new List<Employee>();

        /// <summary>
        /// Danh sách vai trò có sẵn trong hệ thống
        /// </summary>
        private List<Role> _roles = new List<Role>();

        /// <summary>
        /// Constructor - Khởi tạo service và dữ liệu mẫu
        /// </summary>
        public EmployeeService()
        {
            // Khởi tạo các vai trò trong hệ thống
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

            // Thêm nhân viên mẫu để test hệ thống
            _employees.Add(
                new Employee
                {
                    Id = 1,
                    Name = "aa",
                    RoleId = 1, // Nhân viên bán hàng
                }
            );
            _employees.Add(
                new Employee
                {
                    Id = 2,
                    Name = "bb",
                    RoleId = 2, // Quản lý
                }
            );
            _employees.Add(
                new Employee
                {
                    Id = 3,
                    Name = "cc",
                    RoleId = 3, // Kế toán
                }
            );
        }

        /// <summary>
        /// Đăng nhập nhân viên bằng tên
        /// Hỗ trợ tìm kiếm không phân biệt hoa thường
        /// </summary>
        /// <param name="name">Tên nhân viên để đăng nhập</param>
        /// <returns>Đối tượng Employee nếu tìm thấy, null nếu không tồn tại</returns>
        public Employee? LoginEmployee(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            // Làm sạch tên: loại bỏ khoảng trắng, xuống dòng và ký tự đặc biệt
            var cleanedName = name.Trim().Replace("\r", "").Replace("\n", "");
            return _employees.FirstOrDefault(e =>
                string.Equals(e.Name.Trim(), cleanedName, StringComparison.OrdinalIgnoreCase)
            );
        }

        /// <summary>
        /// Lấy danh sách tất cả nhân viên trong hệ thống
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        public List<Employee> GetAllEmployees()
        {
            return _employees.ToList();
        }

        /// <summary>
        /// Lấy thông tin vai trò theo ID
        /// </summary>
        /// <param name="roleId">ID vai trò</param>
        /// <returns>Đối tượng Role nếu tìm thấy, null nếu không tồn tại</returns>
        public Role? GetRole(int roleId)
        {
            return _roles.FirstOrDefault(r => r.Id == roleId);
        }
    }
}
