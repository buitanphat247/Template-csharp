using System;

namespace FoodStore.Utils
{
    /// <summary>
    /// Helper class cho việc hiển thị dữ liệu
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Format tiền tệ theo định dạng Việt Nam
        /// </summary>
        public static string FormatCurrency(decimal amount)
        {
            return $"{amount:N0} VNĐ";
        }

        /// <summary>
        /// Format ngày tháng
        /// </summary>
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Tạo dòng phân cách
        /// </summary>
        public static string CreateSeparator(int length = 50)
        {
            return new string('=', length);
        }

        /// <summary>
        /// Hiển thị tiêu đề với format đẹp
        /// </summary>
        public static void DisplayTitle(string title)
        {
            Console.WriteLine(CreateSeparator());
            Console.WriteLine($"  {title}");
            Console.WriteLine(CreateSeparator());
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        public static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Lỗi: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        public static void DisplaySuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }
    }
}

