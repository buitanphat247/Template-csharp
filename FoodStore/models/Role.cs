using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho vai trò của nhân viên trong hệ thống
    /// </summary>
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
