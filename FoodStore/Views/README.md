# Views - FoodStore System

Thư mục này chứa tất cả các giao diện người dùng (UI) của hệ thống FoodStore, được tổ chức theo mô hình MVC với Dependency Injection.

## 📋 Danh sách UI Components

### 1. **UIManager.cs**
- **Mục đích**: Entry point chính của hệ thống, quản lý tất cả UI components
- **Dependency Injection**: Nhận tất cả 4 service chính từ Program.cs
- **Chức năng**: Hiển thị menu chính, điều hướng đến các UI con

#### 🎯 **Dependencies**:
```csharp
// Services được inject từ Program.cs
private readonly ProductService _productService;
private readonly OrderService _orderService;
private readonly CustomerService _customerService;
private readonly EmployeeService _employeeService;

// UI components được khởi tạo trong constructor
private readonly CustomerUI _customerUI;
private readonly EmployeeUI _employeeUI;
```

#### 🔄 **Dependency Flow**:
```
Program.cs → UIManager (inject 4 services)
UIManager → CustomerUI (pass 4 services)
UIManager → EmployeeUI (pass 4 services)
```

---

### 2. **CustomerUI.cs**
- **Mục đích**: Giao diện dành cho khách hàng (đăng nhập, đăng ký, mua sắm)
- **Dependency Injection**: Nhận 4 service từ UIManager
- **Chức năng**: Quản lý luồng khách hàng từ đăng nhập đến mua sắm

#### 🎯 **Dependencies**:
```csharp
// Services được inject từ UIManager
private readonly ProductService _productService;
private readonly OrderService _orderService;
private readonly CustomerService _customerService;
private readonly EmployeeService _employeeService;
```

#### 🔄 **Dependency Flow**:
```
UIManager → CustomerUI (inject 4 services)
CustomerUI → ShoppingUI (pass 4 services)
CustomerUI → ProfileUI (pass 2 services: OrderService, CustomerService)
CustomerUI → RankUI (pass 1 service: CustomerService)
```

---

### 3. **EmployeeUI.cs**
- **Mục đích**: Giao diện dành cho nhân viên (quản trị hệ thống)
- **Dependency Injection**: Nhận 4 service từ UIManager
- **Chức năng**: Quản lý sản phẩm, đơn hàng, khách hàng, rank

#### 🎯 **Dependencies**:
```csharp
// Services được inject từ UIManager
private readonly ProductService _productService;
private readonly OrderService _orderService;
private readonly CustomerService _customerService;
private readonly EmployeeService _employeeService;
```

#### 🔄 **Dependency Flow**:
```
UIManager → EmployeeUI (inject 4 services)
EmployeeUI → ProductTable (pass ProductService)
EmployeeUI → OrderTable (pass OrderService)
EmployeeUI → CustomerTable (pass CustomerService)
EmployeeUI → RankTable (pass CustomerService)
```

---

### 4. **ShoppingUI.cs**
- **Mục đích**: Giao diện mua sắm cho khách hàng
- **Dependency Injection**: Nhận 4 service từ CustomerUI
- **Chức năng**: Hiển thị sản phẩm, quản lý giỏ hàng, xử lý thanh toán

#### 🎯 **Dependencies**:
```csharp
// Services được inject từ CustomerUI
private readonly ProductService _productService;
private readonly OrderService _orderService;
private readonly CustomerService _customerService;
private readonly EmployeeService _employeeService;

// UI component được khởi tạo trong constructor
private readonly ReceiptUI _receiptUI;
```

#### 🔄 **Dependency Flow**:
```
CustomerUI → ShoppingUI (inject 4 services)
ShoppingUI → ReceiptUI (inject CustomerService)
ShoppingUI → ProductTable (pass ProductService)
ShoppingUI → OrderTable (pass Order)
```

---

### 5. **ProfileUI.cs**
- **Mục đích**: Hiển thị profile cá nhân của khách hàng
- **Dependency Injection**: Nhận 2 service từ CustomerUI
- **Chức năng**: Hiển thị thông tin cá nhân và lịch sử đơn hàng

#### 🎯 **Dependencies**:
```csharp
// Services được inject từ CustomerUI
private readonly OrderService _orderService;
private readonly CustomerService _customerService;
```

#### 🔄 **Dependency Flow**:
```
CustomerUI → ProfileUI (inject 2 services)
ProfileUI → CustomerTable (pass Customer, CustomerService, OrderService)
```

---

### 6. **RankUI.cs**
- **Mục đích**: Hiển thị hệ thống cấp độ thành viên
- **Dependency Injection**: Nhận 1 service từ CustomerUI
- **Chức năng**: Hiển thị rank hiện tại và rank tiếp theo

#### 🎯 **Dependencies**:
```csharp
// Service được inject từ CustomerUI
private readonly CustomerService _customerService;
```

#### 🔄 **Dependency Flow**:
```
CustomerUI → RankUI (inject CustomerService)
RankUI → RankTable (pass Customer, CustomerService)
```

---

### 7. **ReceiptUI.cs**
- **Mục đích**: Hiển thị hóa đơn bán hàng đẹp mắt
- **Dependency Injection**: Nhận 1 service từ ShoppingUI
- **Chức năng**: Tạo hóa đơn chuyên nghiệp với Spectre.Console

#### 🎯 **Dependencies**:
```csharp
// Service được inject từ ShoppingUI
private readonly CustomerService _customerService;
```

#### 🔄 **Dependency Flow**:
```
ShoppingUI → ReceiptUI (inject CustomerService)
ReceiptUI → Spectre.Console (tạo hóa đơn đẹp)
```

---

### 8. **DataSeeder.cs**
- **Mục đích**: Quản lý dữ liệu mẫu cho hệ thống
- **Dependency Injection**: Không sử dụng DI, sử dụng static methods
- **Chức năng**: Khởi tạo sản phẩm và khách hàng mẫu

#### 🎯 **Dependencies**:
```csharp
// Static methods nhận services làm parameters
public static void SeedProducts(ProductService productService)
public static void SeedCustomers(CustomerService customerService)
```

#### 🔄 **Dependency Flow**:
```
Program.cs → DataSeeder.SeedProducts(ProductService)
Program.cs → DataSeeder.SeedCustomers(CustomerService)
```

---

## 🏗️ **Kiến trúc Dependency Injection**

### **Dependency Hierarchy**:
```
Program.cs
├── UIManager (4 services)
│   ├── CustomerUI (4 services)
│   │   ├── ShoppingUI (4 services)
│   │   │   └── ReceiptUI (1 service)
│   │   ├── ProfileUI (2 services)
│   │   └── RankUI (1 service)
│   └── EmployeeUI (4 services)
└── DataSeeder (static methods)
```

### **Service Distribution**:
- **ProductService**: UIManager → CustomerUI → ShoppingUI
- **OrderService**: UIManager → CustomerUI → ShoppingUI, ProfileUI
- **CustomerService**: UIManager → CustomerUI → ShoppingUI, ProfileUI, RankUI, ReceiptUI
- **EmployeeService**: UIManager → CustomerUI → ShoppingUI, EmployeeUI

---

## 🔄 **Luồng Dependency Injection**

### **1. Khởi tạo từ Program.cs**:
```csharp
// Tạo services
var productService = new ProductService();
var orderService = new OrderService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();

// Inject vào UIManager
var uiManager = new UIManager(
    productService,
    orderService,
    customerService,
    employeeService
);
```

### **2. UIManager tạo UI components**:
```csharp
// UIManager constructor
_customerUI = new CustomerUI(
    productService,
    orderService,
    customerService,
    employeeService
);

_employeeUI = new EmployeeUI(
    productService,
    orderService,
    customerService,
    employeeService
);
```

### **3. CustomerUI tạo UI components con**:
```csharp
// Trong CustomerUI.ShowCustomerInterface()
var shoppingUI = new ShoppingUI(
    _productService,
    _orderService,
    _customerService,
    _employeeService
);

var profileUI = new ProfileUI(_orderService, _customerService);
var rankUI = new RankUI(_customerService);
```

### **4. ShoppingUI tạo ReceiptUI**:
```csharp
// Trong ShoppingUI constructor
_receiptUI = new ReceiptUI(customerService);
```

---

## 🎯 **Nguyên tắc Dependency Injection**

### **1. Constructor Injection**:
- Tất cả dependencies được inject qua constructor
- Không sử dụng `new` để tạo dependencies bên trong class
- Dependencies được lưu trữ trong `private readonly` fields

### **2. Single Responsibility**:
- Mỗi UI component chỉ quản lý một chức năng cụ thể
- Dependencies được truyền xuống chỉ những gì cần thiết
- Không inject dependencies không sử dụng

### **3. Dependency Inversion**:
- UI components phụ thuộc vào abstractions (interfaces) chứ không phụ thuộc vào concrete classes
- Có thể dễ dàng thay thế implementations khác nhau

### **4. Lifecycle Management**:
- UIManager quản lý lifecycle của tất cả UI components
- UI components con được tạo khi cần thiết
- Không có circular dependencies

---

## 🔧 **Lợi ích của Dependency Injection**

### **1. Testability**:
- Dễ dàng mock dependencies cho unit testing
- Có thể test UI components độc lập
- Kiểm soát được data flow

### **2. Maintainability**:
- Thay đổi implementation không ảnh hưởng đến UI
- Dependencies rõ ràng và dễ theo dõi
- Code dễ đọc và hiểu

### **3. Flexibility**:
- Có thể thay đổi service implementations
- Dễ dàng thêm tính năng mới
- Tách biệt concerns

### **4. Reusability**:
- UI components có thể tái sử dụng với different services
- Services có thể được sử dụng bởi nhiều UI components
- Giảm code duplication

---

## 📈 **Mở rộng trong tương lai**

### **1. Interface-based DI**:
```csharp
// Thay vì concrete classes
public class UIManager
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    // ...
}
```

### **2. DI Container**:
```csharp
// Sử dụng Microsoft.Extensions.DependencyInjection
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IOrderService, OrderService>();
// ...
```

### **3. Configuration-based DI**:
```csharp
// Đọc configuration từ appsettings.json
var services = new ServiceCollection();
services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
```

### **4. Lazy Loading**:
```csharp
// Sử dụng Lazy<T> cho expensive dependencies
private readonly Lazy<ReceiptUI> _receiptUI;
```

---

## 🎯 **Best Practices**

### **1. Dependency Injection**:
- Luôn inject dependencies qua constructor
- Sử dụng `private readonly` cho dependencies
- Không tạo dependencies bên trong methods

### **2. Service Lifecycle**:
- UIManager quản lý lifecycle của UI components
- UI components con được tạo khi cần thiết
- Dispose resources khi không cần thiết

### **3. Error Handling**:
- Validate dependencies trong constructor
- Handle null dependencies gracefully
- Log dependency injection errors

### **4. Performance**:
- Chỉ inject những gì cần thiết
- Sử dụng singleton pattern cho expensive services
- Lazy load UI components khi cần thiết

---

## 🔍 **Troubleshooting**

### **1. Circular Dependencies**:
- Kiểm tra dependency graph
- Sử dụng interfaces để break cycles
- Refactor code nếu cần thiết

### **2. Missing Dependencies**:
- Kiểm tra constructor parameters
- Đảm bảo tất cả dependencies được inject
- Sử dụng null checks

### **3. Performance Issues**:
- Kiểm tra số lượng dependencies
- Sử dụng lazy loading nếu cần
- Optimize service creation

### **4. Testing Issues**:
- Mock tất cả dependencies
- Sử dụng dependency injection trong tests
- Kiểm tra dependency resolution
