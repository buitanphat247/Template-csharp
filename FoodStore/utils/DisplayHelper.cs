using System;

namespace FoodStore.Utils
{
    /// <summary>
    /// Helper class cung cấp các utility methods cho việc hiển thị dữ liệu
    /// Bao gồm: format tiền tệ, ngày tháng, tạo UI elements và thông báo
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Format tiền tệ theo định dạng Việt Nam với dấu phẩy phân cách hàng nghìn
        /// </summary>
        /// <param name="amount">Số tiền cần format</param>
        /// <returns>Chuỗi tiền tệ đã format (VD: "1,000,000 VNĐ")</returns>
        public static string FormatCurrency(decimal amount)
        {
            return $"{amount:N0} VNĐ"; // N0 format với dấu phẩy phân cách hàng nghìn
        }

        /// <summary>
        /// Format ngày tháng theo định dạng Việt Nam
        /// </summary>
        /// <param name="dateTime">DateTime cần format</param>
        /// <returns>Chuỗi ngày tháng đã format (VD: "25/12/2023 14:30:45")</returns>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss"); // Định dạng: ngày/tháng/năm giờ:phút:giây
        }

        /// <summary>
        /// Tạo dòng phân cách với ký tự '=' để tách biệt các section
        /// </summary>
        /// <param name="length">Độ dài dòng phân cách (mặc định 50 ký tự)</param>
        /// <returns>Chuỗi phân cách</returns>
        public static string CreateSeparator(int length = 50)
        {
            return new string('=', length); // Tạo chuỗi gồm 'length' ký tự '='
        }

        /// <summary>
        /// Hiển thị tiêu đề với format đẹp, có dòng phân cách trên và dưới
        /// </summary>
        /// <param name="title">Tiêu đề cần hiển thị</param>
        public static void DisplayTitle(string title)
        {
            Console.WriteLine(CreateSeparator()); // Dòng phân cách trên
            Console.WriteLine($"  {title}"); // Tiêu đề với 2 space indent
            Console.WriteLine(CreateSeparator()); // Dòng phân cách dưới
        }

        /// <summary>
        /// Hiển thị thông báo lỗi với màu đỏ để dễ nhận biết
        /// </summary>
        /// <param name="message">Nội dung thông báo lỗi</param>
        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Đặt màu chữ đỏ
            Console.WriteLine($"Lỗi: {message}"); // Hiển thị với prefix "Lỗi:"
            Console.ResetColor(); // Reset về màu mặc định
        }

        /// <summary>
        /// Hiển thị thông báo thành công với màu xanh và icon checkmark
        /// </summary>
        /// <param name="message">Nội dung thông báo thành công</param>
        public static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Đặt màu chữ xanh
            Console.WriteLine($"✓ {message}"); // Hiển thị với icon ✓
            Console.ResetColor(); // Reset về màu mặc định
        }
    }
}
