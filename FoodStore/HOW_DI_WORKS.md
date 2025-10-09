# Cách Dependency Injection Hoạt Động - Ví Dụ Thực Tế

Giải thích cách DI hoạt động trong FoodStore với ví dụ cụ thể từ code thực tế.

## 🎯 **Ví dụ đơn giản: Tại sao cần DI?**

### **❌ Cách cũ (KHÔNG dùng DI)**
```csharp
public class CustomerUI
{
    public void ShowCustomerLogin()
    {
        // Tự tạo service bên trong method - BAD!
        var customerService = new CustomerService();
        var productService = new ProductService();
        
        var customer = customerService.LoginCustomer("0901234567");
        // ...
    }
}
```

**Vấn đề:**
- Khó test (không thể mock CustomerService)
- Code cứng nhắc (luôn tạo CustomerService mới)
- Vi phạm Single Responsibility Principle

### **✅ Cách mới (Dùng DI)**
```csharp
public class CustomerUI
{
    private readonly CustomerService _customerService;
    private readonly ProductService _productService;

    // Nhận services từ bên ngoài - GOOD!
    public CustomerUI(CustomerService customerService, ProductService productService)
    {
        _customerService = customerService;
        _productService = productService;
    }

    public void ShowCustomerLogin()
    {
        // Sử dụng service đã được inject
        var customer = _customerService.LoginCustomer("0901234567");
        // ...
    }
}
```

**Lợi ích:**
- Dễ test (có thể mock CustomerService)
- Linh hoạt (có thể thay đổi implementation)
- Code sạch và dễ hiểu

---

## 🔄 **Tất cả cách tiêm DI trong FoodStore**

### **1. Program.cs - Nơi bắt đầu tiêm DI**
```csharp
// Program.cs - Entry point của ứng dụng
static void Main()
{
    // Tạo tất cả services trước
    var productService = new ProductService();
    var orderService = new OrderService();
    var customerService = new CustomerService();
    var employeeService = new EmployeeService();

    // Inject tất cả 4 services vào UIManager
    var uiManager = new UIManager(
        productService,    // ← Inject ProductService
        orderService,      // ← Inject OrderService  
        customerService,   // ← Inject CustomerService
        employeeService    // ← Inject EmployeeService
    );

    uiManager.ShowMainMenu();
}
```

### **2. UIManager - Nhận DI và tạo UI components**
```csharp
public class UIManager
{
    // Lưu trữ tất cả 4 services được inject từ Program.cs
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;

    // UI components được tạo trong constructor
    private readonly CustomerUI _customerUI;
    private readonly EmployeeUI _employeeUI;

    // Constructor nhận 4 services từ Program.cs
    public UIManager(
        ProductService productService,
        OrderService orderService,
        CustomerService customerService,
        EmployeeService employeeService
    )
    {
        // Lưu trữ services
        _productService = productService;
        _orderService = orderService;
        _customerService = customerService;
        _employeeService = employeeService;

        // Tạo CustomerUI với tất cả 4 services
        _customerUI = new CustomerUI(
            productService,    // ← Truyền ProductService xuống
            orderService,      // ← Truyền OrderService xuống
            customerService,   // ← Truyền CustomerService xuống
            employeeService    // ← Truyền EmployeeService xuống
        );

        // Tạo EmployeeUI với tất cả 4 services
        _employeeUI = new EmployeeUI(
            productService,    // ← Truyền ProductService xuống
            orderService,      // ← Truyền OrderService xuống
            customerService,   // ← Truyền CustomerService xuống
            employeeService    // ← Truyền EmployeeService xuống
        );
    }
}
```

### **3. CustomerUI - Nhận DI và tạo sub-UI components**
```csharp
public class CustomerUI
{
    // Lưu trữ tất cả 4 services được inject từ UIManager
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;

    // Constructor nhận 4 services từ UIManager
    public CustomerUI(
        ProductService productService,
        OrderService orderService,
        CustomerService customerService,
        EmployeeService employeeService
    )
    {
        _productService = productService;
        _orderService = orderService;
        _customerService = customerService;
        _employeeService = employeeService;
    }

    // Trong ShowCustomerInterface() - Tạo sub-UI components với DI
    public void ShowCustomerInterface(Customer customer)
    {
        switch (choice)
        {
            case "1":
                // Tạo ShoppingUI với tất cả 4 services
                var shoppingUI = new ShoppingUI(
                    _productService,    // ← Truyền ProductService
                    _orderService,      // ← Truyền OrderService
                    _customerService,   // ← Truyền CustomerService
                    _employeeService    // ← Truyền EmployeeService
                );
                shoppingUI.ShowShoppingInterface(customer);
                break;
            case "2":
                // Tạo ProfileUI với 2 services cần thiết
                var profileUI = new ProfileUI(
                    _orderService,      // ← Chỉ truyền OrderService
                    _customerService    // ← Chỉ truyền CustomerService
                );
                profileUI.ShowCustomerProfile(customer);
                break;
            case "3":
                // Tạo RankUI với 1 service cần thiết
                var rankUI = new RankUI(
                    _customerService    // ← Chỉ truyền CustomerService
                );
                rankUI.ShowCustomerRank(customer);
                break;
        }
    }
}
```

### **4. EmployeeUI - Nhận DI và sử dụng Table components**
```csharp
public class EmployeeUI
{
    // Lưu trữ tất cả 4 services được inject từ UIManager
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;

    // Constructor nhận 4 services từ UIManager
    public EmployeeUI(
        ProductService productService,
        OrderService orderService,
        CustomerService customerService,
        EmployeeService employeeService
    )
    {
        _productService = productService;
        _orderService = orderService;
        _customerService = customerService;
        _employeeService = employeeService;
    }

    // Trong ShowEmployeeInterface() - Sử dụng Table components với DI
    public void ShowEmployeeInterface(Employee employee)
    {
        switch (choice)
        {
            case "1":
                // ProductTable nhận ProductService
                ProductTable.ShowProductTable(_productService);
                break;
            case "2":
                // OrderTable nhận OrderService
                OrderTable.ShowOrderTable(_orderService);
                break;
            case "3":
                // CustomerTable nhận CustomerService
                CustomerTable.ShowCustomerTable(_customerService);
                break;
            case "4":
                // RankTable nhận CustomerService
                RankTable.ShowEmployeeRankTable(_customerService);
                break;
        }
    }
}
```

### **5. ShoppingUI - Nhận DI và tạo ReceiptUI**
```csharp
public class ShoppingUI
{
    // Lưu trữ tất cả 4 services được inject từ CustomerUI
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;

    // ReceiptUI được tạo trong constructor
    private readonly ReceiptUI _receiptUI;

    // Constructor nhận 4 services từ CustomerUI
    public ShoppingUI(
        ProductService productService,
        OrderService orderService,
        CustomerService customerService,
        EmployeeService employeeService
    )
    {
        _productService = productService;
        _orderService = orderService;
        _customerService = customerService;
        _employeeService = employeeService;

        // Tạo ReceiptUI với CustomerService
        _receiptUI = new ReceiptUI(customerService);
    }

    // Sử dụng Table components với DI
    public void ShowShoppingInterface(Customer customer)
    {
        // ProductTable nhận ProductService
        ProductTable.ShowShoppingProductTable(_productService);

        // OrderTable nhận Order
        OrderTable.ShowCartTable(currentOrder);

        // ReceiptTable nhận Order và CustomerService
        ReceiptTable.ShowReceiptTable(order, _customerService);
    }
}
```

### **6. ProfileUI - Nhận DI tối thiểu**
```csharp
public class ProfileUI
{
    // Chỉ lưu trữ 2 services cần thiết
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;

    // Constructor nhận 2 services từ CustomerUI
    public ProfileUI(OrderService orderService, CustomerService customerService)
    {
        _orderService = orderService;
        _customerService = customerService;
    }

    // Sử dụng Table components với DI
    public void ShowCustomerProfile(Customer customer)
    {
        // CustomerTable nhận Customer, CustomerService, OrderService
        CustomerTable.ShowProfileTable(customer, _customerService, _orderService);
        
        // CustomerTable nhận OrderService và Customer
        CustomerTable.ShowOrderHistoryTable(_orderService, customer);
    }
}
```

### **7. RankUI - Nhận DI tối thiểu**
```csharp
public class RankUI
{
    // Chỉ lưu trữ 1 service cần thiết
    private readonly CustomerService _customerService;

    // Constructor nhận 1 service từ CustomerUI
    public RankUI(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // Sử dụng Table component với DI
    public void ShowCustomerRank(Customer customer)
    {
        // RankTable nhận Customer và CustomerService
        RankTable.ShowCustomerRankTable(customer, _customerService);
    }
}
```

### **8. ReceiptUI - Nhận DI tối thiểu**
```csharp
public class ReceiptUI
{
    // Chỉ lưu trữ 1 service cần thiết
    private readonly CustomerService _customerService;

    // Constructor nhận 1 service từ ShoppingUI
    public ReceiptUI(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // Sử dụng Table component với DI
    public void ShowReceipt(Order order)
    {
        // ReceiptTable nhận Order và CustomerService
        ReceiptTable.ShowReceiptTable(order, _customerService);
    }
}
```

### **9. DataSeeder - Static methods với DI**
```csharp
public class DataSeeder
{
    // Static methods nhận services làm parameters
    public static void SeedProducts(ProductService productService)
    {
        // Sử dụng ProductService để thêm sản phẩm
        foreach (var product in products)
        {
            productService.AddProduct(product);
        }
    }

    public static void SeedCustomers(CustomerService customerService)
    {
        // Sử dụng CustomerService để thêm khách hàng
        var customer1 = customerService.RegisterCustomer("Nguyễn Văn A", "0901234567");
        var customer2 = customerService.RegisterCustomer("Trần Thị B", "0901234568");
    }
}
```

---

## 📊 **Tóm tắt tất cả cách tiêm DI trong FoodStore**

### **DI Hierarchy - Sơ đồ phân cấp**
```
Program.cs (4 services)
    ↓
UIManager (4 services)
    ↓
├── CustomerUI (4 services)
│   ├── ShoppingUI (4 services)
│   │   └── ReceiptUI (1 service)
│   ├── ProfileUI (2 services)
│   └── RankUI (1 service)
│
└── EmployeeUI (4 services)
    ├── ProductTable (1 service)
    ├── OrderTable (1 service)
    ├── CustomerTable (1 service)
    └── RankTable (1 service)
```

### **Service Distribution - Phân bố services**
| Component | ProductService | OrderService | CustomerService | EmployeeService |
|-----------|----------------|--------------|-----------------|-----------------|
| **Program.cs** | ✅ Create | ✅ Create | ✅ Create | ✅ Create |
| **UIManager** | ✅ Inject | ✅ Inject | ✅ Inject | ✅ Inject |
| **CustomerUI** | ✅ Inject | ✅ Inject | ✅ Inject | ✅ Inject |
| **EmployeeUI** | ✅ Inject | ✅ Inject | ✅ Inject | ✅ Inject |
| **ShoppingUI** | ✅ Inject | ✅ Inject | ✅ Inject | ✅ Inject |
| **ProfileUI** | ❌ | ✅ Inject | ✅ Inject | ❌ |
| **RankUI** | ❌ | ❌ | ✅ Inject | ❌ |
| **ReceiptUI** | ❌ | ❌ | ✅ Inject | ❌ |
| **DataSeeder** | ✅ Parameter | ❌ | ✅ Parameter | ❌ |

### **DI Patterns được sử dụng**

#### **1. Constructor Injection (Chính)**
- Tất cả UI components sử dụng constructor injection
- Services được inject qua constructor parameters
- Lưu trữ trong `private readonly` fields

#### **2. Method Injection**
- DataSeeder sử dụng method injection
- Services được truyền qua method parameters
- Không lưu trữ services trong class

#### **3. Static Method Injection**
- DataSeeder.SeedProducts(ProductService)
- DataSeeder.SeedCustomers(CustomerService)
- Services được truyền trực tiếp vào static methods

### **DI Flow - Luồng tiêm DI**

#### **Bước 1: Program.cs tạo services**
```csharp
var productService = new ProductService();
var orderService = new OrderService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();
```

#### **Bước 2: UIManager nhận tất cả services**
```csharp
var uiManager = new UIManager(
    productService,    // ← Inject 4 services
    orderService,
    customerService,
    employeeService
);
```

#### **Bước 3: UIManager tạo UI components**
```csharp
_customerUI = new CustomerUI(4 services);  // ← Truyền 4 services
_employeeUI = new EmployeeUI(4 services);  // ← Truyền 4 services
```

#### **Bước 4: UI components tạo sub-components**
```csharp
// CustomerUI tạo sub-UI components
var shoppingUI = new ShoppingUI(4 services);  // ← Truyền 4 services
var profileUI = new ProfileUI(2 services);    // ← Truyền 2 services
var rankUI = new RankUI(1 service);           // ← Truyền 1 service

// ShoppingUI tạo ReceiptUI
_receiptUI = new ReceiptUI(1 service);        // ← Truyền 1 service
```

#### **Bước 5: Table components nhận services**
```csharp
// EmployeeUI sử dụng Table components
ProductTable.ShowProductTable(_productService);
OrderTable.ShowOrderTable(_orderService);
CustomerTable.ShowCustomerTable(_customerService);
RankTable.ShowEmployeeRankTable(_customerService);
```

### **Lợi ích của cách tiêm DI này**

#### **1. Testability**
- Có thể mock bất kỳ service nào
- Test độc lập từng component
- Kiểm soát được data flow

#### **2. Flexibility**
- Có thể thay đổi service implementation
- Dễ dàng thêm services mới
- Tách biệt concerns rõ ràng

#### **3. Maintainability**
- Code dễ đọc và hiểu
- Dependencies rõ ràng
- Dễ debug và troubleshoot

#### **4. Reusability**
- UI components có thể tái sử dụng
- Services có thể được sử dụng bởi nhiều components
- Table components có thể sử dụng với different services

---

## 🎯 **Tóm tắt: DI hoạt động như thế nào?**

### **1. Tạo Dependencies (Services)**
```csharp
// Program.cs
var customerService = new CustomerService();
var productService = new ProductService();
```

### **2. Inject Dependencies**
```csharp
// Truyền services vào constructor
var customerUI = new CustomerUI(customerService, productService);
```

### **3. Sử dụng Dependencies**
```csharp
// CustomerUI sử dụng services đã inject
public void DoSomething()
{
    var customer = _customerService.LoginCustomer("phone");
    var products = _productService.GetAllProducts();
}
```

### **4. Lợi ích**
- **Testable**: Có thể mock services
- **Flexible**: Có thể thay đổi implementation
- **Maintainable**: Code dễ đọc và bảo trì
- **Reusable**: Components có thể tái sử dụng

---

## 🚀 **Kết luận**

DI đơn giản là:
1. **Tạo** services ở ngoài
2. **Truyền** services vào constructor
3. **Sử dụng** services trong class

Thay vì class tự tạo dependencies, nó nhận dependencies từ bên ngoài. Điều này làm cho code linh hoạt và dễ test hơn!
