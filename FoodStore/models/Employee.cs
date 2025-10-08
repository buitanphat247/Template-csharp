using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho nhân viên trong hệ thống
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int RoleId { get; set; }

        // Navigation property
        public Role? Role { get; set; }
    }
}
