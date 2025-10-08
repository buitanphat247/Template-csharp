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

### Console Application (Hiện tại)
```
FoodStore/
├── Domain/           # Các entity và business logic
│   ├── Customer.cs   # Khách hàng (abstract base)
│   ├── MemberCustomer.cs # Khách hàng thành viên
│   ├── RiceProduct.cs # Sản phẩm gạo
│   └── Order.cs      # Đơn hàng
├── Generics/         # Generic repository pattern
│   └── InMemoryRepository.cs
├── Pricing/          # Chiến lược tính giá và thuế
│   ├── IPriceRule.cs
│   ├── ITaxCalculator.cs
│   └── Vat8Percent.cs
├── Services/         # Business services
│   ├── OrderService.cs
│   └── InventoryService.cs
├── Program.cs        # Entry point console
└── README.md
```

### MVC Application (Dự kiến)
```
FoodStore/
├── Controllers/      # MVC Controllers
│   ├── HomeController.cs
│   ├── ProductController.cs
│   └── OrderController.cs
├── Views/           # Razor Views
│   ├── Home/
│   ├── Product/
│   └── Order/
├── Models/          # ViewModels
│   ├── ProductViewModel.cs
│   └── OrderViewModel.cs
├── wwwroot/         # Static files (CSS, JS, images)
├── appsettings.json # Cấu hình ứng dụng
├── Program.cs       # Entry point MVC
└── Domain/         # Tái sử dụng từ console
```

### So sánh Console App và Web MVC / Web API

| **Thành phần trong Console App** | **Thành phần tương ứng trong MVC / Web API** | **Vai trò** |
|-----------------------------------|-----------------------------------------------|-------------|
| **Domain** | **Models** | Chứa các class dữ liệu (entity) như `Customer`, `RiceProduct`, `Order` — mô tả cấu trúc dữ liệu thực tế |
| **Services** | **Services** hoặc một phần trong **Controllers** | Xử lý nghiệp vụ (business logic) – ví dụ: tạo đơn hàng, tính giá, cập nhật tồn kho, tích điểm thành viên |
| **Program.cs (Main)** | **Controllers** (và Startup / Program trong web) | Là "điểm vào" của chương trình. Trong web, Controller nhận request từ người dùng, gọi Service, rồi trả về response |
| **Generics / Helpers** | **Utils / Common / Helpers** | Chứa các hàm dùng chung như định dạng tiền, in hóa đơn, hoặc log dữ liệu |

#### **Mapping chi tiết:**

**Console App → MVC:**
- `Domain/Customer.cs` → `Models/Customer.cs` (giữ nguyên)
- `Services/OrderService.cs` → `Services/OrderService.cs` (giữ nguyên) 
- `Program.cs` → `Controllers/OrderController.cs` + `Views/Order/`
- `Generics/InMemoryRepository.cs` → `Data/Repository.cs` (thay bằng Entity Framework)

**Console App → Web API:**
- `Domain/` → `Models/` (giữ nguyên)
- `Services/` → `Services/` (giữ nguyên)
- `Program.cs` → `Controllers/` (trả về JSON thay vì Console.WriteLine)

#### **Logic sử dụng:**
- **Console**: Demo và testing business logic, không cần UI
- **MVC**: Giao diện web thực tế, tái sử dụng Domain và Services
- **Domain**: Chia sẻ giữa console và MVC (DRY principle)
- **Services**: Business logic có thể inject vào MVC controllers

#### **Migration Path:**
1. **Phase 1**: Phát triển business logic trong Console
2. **Phase 2**: Tạo MVC project, copy Domain + Services
3. **Phase 3**: Tạo Controllers gọi Services
4. **Phase 4**: Tạo Views và ViewModels
5. **Phase 5**: Cấu hình database thật thay InMemory

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
