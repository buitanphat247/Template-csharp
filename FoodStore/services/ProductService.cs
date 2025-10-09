using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ liên quan đến sản phẩm
    /// Bao gồm: quản lý sản phẩm, tìm kiếm, cập nhật tồn kho
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// Danh sách sản phẩm trong hệ thống
        /// </summary>
        private List<Product> _products = new List<Product>();

        /// <summary>
        /// ID tiếp theo cho sản phẩm mới (auto-increment)
        /// </summary>
        private int _nextId = 1;

        /// <summary>
        /// Thêm sản phẩm mới vào hệ thống
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần thêm</param>
        public void AddProduct(Product product)
        {
            product.Id = _nextId++; // Tự động gán ID
            _products.Add(product);
        }

        /// <summary>
        /// Lấy danh sách tất cả sản phẩm trong hệ thống
        /// </summary>
        /// <returns>Danh sách sản phẩm</returns>
        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        /// <summary>
        /// Tìm sản phẩm theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm cần tìm</param>
        /// <returns>Đối tượng Product nếu tìm thấy, null nếu không tồn tại</returns>
        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo tên (không phân biệt hoa thường)
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách sản phẩm khớp với từ khóa</returns>
        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _products.ToList(); // Trả về tất cả nếu không có từ khóa

            return _products
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Cập nhật tồn kho khi bán sản phẩm
        /// </summary>
        /// <param name="productId">ID sản phẩm cần cập nhật</param>
        /// <param name="quantity">Số lượng cần trừ khỏi tồn kho</param>
        /// <returns>True nếu cập nhật thành công, False nếu không đủ hàng</returns>
        public bool UpdateStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null && product.Stock >= quantity)
            {
                product.Stock -= quantity; // Trừ số lượng đã bán
                return true;
            }
            return false; // Không đủ hàng hoặc không tìm thấy sản phẩm
        }

        /// <summary>
        /// Kiểm tra tồn kho có đủ để bán hay không
        /// </summary>
        /// <param name="productId">ID sản phẩm cần kiểm tra</param>
        /// <param name="quantity">Số lượng cần kiểm tra</param>
        /// <returns>True nếu đủ hàng, False nếu không đủ</returns>
        public bool CheckStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            return product != null && product.Stock >= quantity;
        }
    }
}
