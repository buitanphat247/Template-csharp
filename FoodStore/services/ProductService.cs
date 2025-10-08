using System;
using System.Collections.Generic;
using System.Linq;
using FoodStore.Models;

namespace FoodStore.Services
{
    /// <summary>
    /// Service xử lý logic sản phẩm
    /// </summary>
    public class ProductService
    {
        private List<Product> _products = new List<Product>();
        private int _nextId = 1;

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        public void AddProduct(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
        }

        /// <summary>
        /// Lấy tất cả sản phẩm
        /// </summary>
        public List<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        /// <summary>
        /// Tìm sản phẩm theo ID
        /// </summary>
        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo tên
        /// </summary>
        public List<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _products.ToList();

            return _products
                .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Cập nhật tồn kho
        /// </summary>
        public bool UpdateStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            if (product != null && product.Stock >= quantity)
            {
                product.Stock -= quantity;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra tồn kho
        /// </summary>
        public bool CheckStock(int productId, int quantity)
        {
            var product = GetProductById(productId);
            return product != null && product.Stock >= quantity;
        }
    }
}



