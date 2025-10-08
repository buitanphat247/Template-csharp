# FoodStore - Hệ thống quản lý cửa hàng gạo

## Mô tả
Dự án console application quản lý cửa hàng gạo với các tính năng:
- Quản lý sản phẩm gạo (ST25, Jasmine)
- Quản lý khách hàng (thành viên, VIP)
- Tạo đơn hàng và tính giá
- Hệ thống giảm giá và tích điểm
- Quản lý tồn kho

## Cấu hình dự án

### Yêu cầu hệ thống
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code
- Windows/Linux/macOS

### Cài đặt
1. Clone repository:
```bash
git clone <repository-url>
cd FoodStore
```

2. Restore packages:
```bash
dotnet restore
```

3. Build dự án:
```bash
dotnet build
```

4. Chạy ứng dụng:
```bash
dotnet run --project FoodStore
```

## Cấu trúc dự án
```
FoodStore/
├── Domain/           # Các entity và business logic
├── Generics/         # Generic repository pattern
├── Pricing/          # Chiến lược tính giá và thuế
├── Services/         # Business services
├── Program.cs        # Entry point
└── README.md
```

## ⚠️ Lưu ý quan trọng

**Các file demo cần xóa sau khi clone:**
- `FoodStore/models/text.cs` - File demo, cần xóa
- `FoodStore/services/text.cs` - File demo, cần xóa  
- `FoodStore/utils/text.cs` - File demo, cần xóa

```bash
# Xóa các file demo
rm FoodStore/models/text.cs
rm FoodStore/services/text.cs
rm FoodStore/utils/text.cs
```

## Tính năng chính
- **Generic Repository Pattern**: Quản lý dữ liệu linh hoạt
- **Strategy Pattern**: Chiến lược tính giá và thuế
- **Observer Pattern**: Event handling cho thay đổi trạng thái
- **Polymorphism**: Đa hình cho các loại khách hàng
- **Dependency Injection**: Cấu hình dịch vụ linh hoạt

## Demo
Chạy ứng dụng sẽ thực hiện:
1. Khởi tạo dữ liệu mẫu (sản phẩm, khách hàng)
2. Tạo đơn hàng với giảm giá
3. Tính toán thuế và tổng tiền
4. Cập nhật tồn kho
5. Tích điểm cho thành viên

## Cấu hình UTF-8
Dự án đã cấu hình UTF-8 để hiển thị tiếng Việt:
```csharp
Console.OutputEncoding = Encoding.UTF8;
```
