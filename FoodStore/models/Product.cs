using System;

namespace FoodStore.Models
{
    /// <summary>
    /// Model đại diện cho sản phẩm trong hệ thống cửa hàng thực phẩm
    /// Lưu trữ thông tin sản phẩm, giá cả, tồn kho và trạng thái
    /// </summary>
    public class Product
    {
        /// <summary>
        /// ID duy nhất của sản phẩm
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Danh mục sản phẩm (VD: Thực phẩm tươi, Đồ uống, Đồ khô, v.v.)
        /// </summary>
        public string Category { get; set; } = "";

        /// <summary>
        /// Giá bán của sản phẩm (đơn vị: VND)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Số lượng tồn kho hiện tại
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Trạng thái sản phẩm: "Active" (đang bán), "Inactive" (ngừng bán), "OutOfStock" (hết hàng)
        /// </summary>
        public string Status { get; set; } = "Active";
    }
}
