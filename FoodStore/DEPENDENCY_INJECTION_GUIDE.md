# Dependency Injection Guide - FoodStore System

Hướng dẫn chi tiết về Dependency Injection (DI) được sử dụng trong hệ thống FoodStore.

## 📚 **Tổng quan về Dependency Injection**

### **Dependency Injection là gì?**
Dependency Injection (DI) là một design pattern cho phép một object nhận các dependencies từ bên ngoài thay vì tự tạo ra chúng. Điều này giúp code trở nên linh hoạt, dễ test và dễ bảo trì hơn.

### **Tại sao sử dụng DI?**
- **Loose Coupling**: Giảm sự phụ thuộc giữa các class
- **Testability**: Dễ dàng mock dependencies cho unit testing
- **Maintainability**: Thay đổi implementation không ảnh hưởng đến code sử dụng
- **Flexibility**: Có thể thay đổi behavior mà không cần sửa code

---

## 🏗️ **Kiến trúc DI trong FoodStore**

### **1. Service Layer (Dependencies)**
```csharp
// Các service cung cấp business logic
public class ProductService { ... }
public class OrderService { ... }
public class CustomerService { ... }
public class EmployeeService { ... }
```

### **2. View Layer (Consumers)**
```csharp
// Các UI component sử dụng services
public class UIManager { ... }
public class CustomerUI { ... }
public class EmployeeUI { ... }
public class ShoppingUI { ... }
```

### **3. Program.cs (DI Container)**
```csharp
// Nơi khởi tạo và inject dependencies
var productService = new ProductService();
var orderService = new OrderService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();

var uiManager = new UIManager(
    productService,
    orderService,
    customerService,
    employeeService
);
```

---

## 🔄 **Luồng Dependency Injection**

### **1. Khởi tạo Services**
```csharp
// Program.cs - Tạo instances của các service
var productService = new ProductService();
var orderService = new OrderService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();
```

### **2. Inject vào UIManager**
```csharp
// UIManager nhận tất cả services
public UIManager(
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
```

### **3. UIManager tạo UI Components**
```csharp
// UIManager tạo CustomerUI và EmployeeUI
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

### **4. UI Components tạo Sub-Components**
```csharp
// CustomerUI tạo các UI con khi cần
var shoppingUI = new ShoppingUI(
    _productService,
    _orderService,
    _customerService,
    _employeeService
);

var profileUI = new ProfileUI(_orderService, _customerService);
var rankUI = new RankUI(_customerService);
```

---

## 📊 **Dependency Graph**

### **Visual Representation**:
```
Program.cs
    │
    ├── ProductService
    ├── OrderService  
    ├── CustomerService
    ├── EmployeeService
    │
    └── UIManager
        │
        ├── CustomerUI
        │   ├── ShoppingUI
        │   │   └── ReceiptUI
        │   ├── ProfileUI
        │   └── RankUI
        │
        └── EmployeeUI
```

### **Service Usage Matrix**:
| UI Component | ProductService | OrderService | CustomerService | EmployeeService |
|--------------|----------------|--------------|-----------------|-----------------|
| UIManager    | ✅             | ✅           | ✅              | ✅              |
| CustomerUI   | ✅             | ✅           | ✅              | ✅              |
| EmployeeUI   | ✅             | ✅           | ✅              | ✅              |
| ShoppingUI   | ✅             | ✅           | ✅              | ✅              |
| ProfileUI    | ❌             | ✅           | ✅              | ❌              |
| RankUI       | ❌             | ❌           | ✅              | ❌              |
| ReceiptUI    | ❌             | ❌           | ✅              | ❌              |

---

## 🎯 **Các Pattern DI được sử dụng**

### **1. Constructor Injection**
```csharp
// Pattern chính được sử dụng trong FoodStore
public class CustomerUI
{
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;

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
}
```

### **2. Method Injection**
```csharp
// Sử dụng trong DataSeeder
public static void SeedProducts(ProductService productService)
{
    // Sử dụng productService
}

public static void SeedCustomers(CustomerService customerService)
{
    // Sử dụng customerService
}
```

### **3. Property Injection**
```csharp
// Không được sử dụng trong FoodStore nhưng có thể áp dụng
public class SomeUI
{
    public ProductService ProductService { get; set; }
}
```

---

## 🔧 **Implementation Details**

### **1. Service Registration**
```csharp
// Program.cs - Manual registration
var productService = new ProductService();
var orderService = new OrderService();
var customerService = new CustomerService();
var employeeService = new EmployeeService();

// Có thể sử dụng DI Container trong tương lai
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IEmployeeService, EmployeeService>();
```

### **2. Dependency Resolution**
```csharp
// UIManager constructor - Manual resolution
public UIManager(
    ProductService productService,
    OrderService orderService,
    CustomerService customerService,
    EmployeeService employeeService
)
{
    // Store dependencies
    _productService = productService;
    _orderService = orderService;
    _customerService = customerService;
    _employeeService = employeeService;

    // Create child components
    _customerUI = new CustomerUI(
        productService,
        orderService,
        customerService,
        employeeService
    );
}
```

### **3. Service Lifecycle**
```csharp
// Singleton pattern cho services
public class ProductService
{
    private static ProductService _instance;
    private static readonly object _lock = new object();

    public static ProductService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new ProductService();
                }
            }
            return _instance;
        }
    }
}
```

---

## 🧪 **Testing với DI**

### **1. Unit Testing**
```csharp
[Test]
public void CustomerUI_ShowCustomerLogin_WithValidPhone_ShouldSucceed()
{
    // Arrange - Mock dependencies
    var mockProductService = new Mock<ProductService>();
    var mockOrderService = new Mock<OrderService>();
    var mockCustomerService = new Mock<CustomerService>();
    var mockEmployeeService = new Mock<EmployeeService>();

    // Setup mock behavior
    mockCustomerService.Setup(x => x.LoginCustomer("0901234567"))
                      .Returns(new Customer { Name = "Test User" });

    // Act - Inject mocked dependencies
    var customerUI = new CustomerUI(
        mockProductService.Object,
        mockOrderService.Object,
        mockCustomerService.Object,
        mockEmployeeService.Object
    );

    // Assert
    // Test the behavior
}
```

### **2. Integration Testing**
```csharp
[Test]
public void ShoppingUI_ProcessPayment_ShouldUpdateCustomerPoints()
{
    // Arrange - Use real services
    var productService = new ProductService();
    var orderService = new OrderService();
    var customerService = new CustomerService();
    var employeeService = new EmployeeService();

    // Seed test data
    DataSeeder.SeedProducts(productService);
    DataSeeder.SeedCustomers(customerService);

    // Act
    var shoppingUI = new ShoppingUI(
        productService,
        orderService,
        customerService,
        employeeService
    );

    // Test integration
}
```

---

## 📈 **Mở rộng DI trong tương lai**

### **1. Interface-based DI**
```csharp
// Tạo interfaces cho services
public interface IProductService
{
    void AddProduct(Product product);
    Product GetProductById(int id);
    List<Product> GetAllProducts();
}

public interface IOrderService
{
    Order CreateOrder(Customer customer, Employee employee);
    void AddOrderDetail(Order order, Product product, int quantity);
    void CalculateTotal(Order order, CustomerService customerService);
}

// Implement interfaces
public class ProductService : IProductService
{
    // Implementation
}

// Inject interfaces instead of concrete classes
public class CustomerUI
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    // ...
}
```

### **2. DI Container (Microsoft.Extensions.DependencyInjection)**
```csharp
// Program.cs - Sử dụng DI Container
var services = new ServiceCollection();

// Register services
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IEmployeeService, EmployeeService>();

// Register UI components
services.AddScoped<UIManager>();
services.AddScoped<CustomerUI>();
services.AddScoped<EmployeeUI>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Resolve UIManager
var uiManager = serviceProvider.GetService<UIManager>();
```

### **3. Configuration-based DI**
```csharp
// appsettings.json
{
  "Services": {
    "ProductService": "FoodStore.Services.ProductService",
    "OrderService": "FoodStore.Services.OrderService",
    "CustomerService": "FoodStore.Services.CustomerService",
    "EmployeeService": "FoodStore.Services.EmployeeService"
  }
}

// Program.cs - Load from configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceType = Type.GetType(configuration["Services:ProductService"]);
var productService = Activator.CreateInstance(serviceType) as IProductService;
```

### **4. Lazy Loading**
```csharp
// Sử dụng Lazy<T> cho expensive dependencies
public class UIManager
{
    private readonly Lazy<CustomerUI> _customerUI;
    private readonly Lazy<EmployeeUI> _employeeUI;

    public UIManager(
        ProductService productService,
        OrderService orderService,
        CustomerService customerService,
        EmployeeService employeeService
    )
    {
        _customerUI = new Lazy<CustomerUI>(() => new CustomerUI(
            productService,
            orderService,
            customerService,
            employeeService
        ));

        _employeeUI = new Lazy<EmployeeUI>(() => new EmployeeUI(
            productService,
            orderService,
            customerService,
            employeeService
        ));
    }
}
```

---

## ⚠️ **Common Pitfalls và Solutions**

### **1. Circular Dependencies**
```csharp
// ❌ BAD - Circular dependency
public class ServiceA
{
    private readonly ServiceB _serviceB;
    public ServiceA(ServiceB serviceB) => _serviceB = serviceB;
}

public class ServiceB
{
    private readonly ServiceA _serviceA;
    public ServiceB(ServiceA serviceA) => _serviceA = serviceA;
}

// ✅ GOOD - Use interfaces to break cycles
public interface IServiceA { }
public interface IServiceB { }

public class ServiceA : IServiceA
{
    private readonly IServiceB _serviceB;
    public ServiceA(IServiceB serviceB) => _serviceB = serviceB;
}
```

### **2. Too Many Dependencies**
```csharp
// ❌ BAD - Too many dependencies
public class GodClass
{
    private readonly Service1 _service1;
    private readonly Service2 _service2;
    private readonly Service3 _service3;
    private readonly Service4 _service4;
    private readonly Service5 _service5;
    // ... 10+ dependencies
}

// ✅ GOOD - Split into smaller classes
public class SpecificUI
{
    private readonly Service1 _service1;
    private readonly Service2 _service2;
}
```

### **3. Service Locator Anti-pattern**
```csharp
// ❌ BAD - Service Locator
public class SomeUI
{
    public void DoSomething()
    {
        var service = ServiceLocator.GetService<ProductService>();
        // Use service
    }
}

// ✅ GOOD - Constructor Injection
public class SomeUI
{
    private readonly ProductService _productService;
    
    public SomeUI(ProductService productService)
    {
        _productService = productService;
    }
}
```

---

## 🎯 **Best Practices**

### **1. Constructor Injection**
- Luôn sử dụng constructor injection
- Sử dụng `private readonly` cho dependencies
- Validate dependencies trong constructor

### **2. Interface Segregation**
- Tạo interfaces nhỏ và focused
- Không tạo "fat" interfaces
- Sử dụng interfaces thay vì concrete classes

### **3. Single Responsibility**
- Mỗi service chỉ có một responsibility
- Tách biệt concerns rõ ràng
- Tránh "god" classes

### **4. Dependency Inversion**
- Depend on abstractions, not concretions
- High-level modules không phụ thuộc vào low-level modules
- Cả hai phụ thuộc vào abstractions

### **5. Lifecycle Management**
- Quản lý lifecycle của dependencies
- Dispose resources khi cần thiết
- Sử dụng appropriate lifetime (Singleton, Scoped, Transient)

---

## 🔍 **Debugging DI Issues**

### **1. Missing Dependencies**
```csharp
// Check constructor parameters
public UIManager(
    ProductService productService,    // ✅ Present
    OrderService orderService,        // ✅ Present
    CustomerService customerService,  // ✅ Present
    EmployeeService employeeService   // ✅ Present
)
```

### **2. Circular Dependencies**
```csharp
// Use dependency graph visualization
// A -> B -> C -> A (Circular!)
// A -> B -> C -> D (Good)
```

### **3. Null Dependencies**
```csharp
// Add null checks
public UIManager(
    ProductService productService,
    OrderService orderService,
    CustomerService customerService,
    EmployeeService employeeService
)
{
    _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
    _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
}
```

### **4. Performance Issues**
```csharp
// Use lazy loading for expensive dependencies
private readonly Lazy<ExpensiveService> _expensiveService;

public SomeUI(ExpensiveService expensiveService)
{
    _expensiveService = new Lazy<ExpensiveService>(() => expensiveService);
}
```

---

## 📚 **Tài liệu tham khảo**

### **1. Design Patterns**
- [Dependency Injection Pattern](https://en.wikipedia.org/wiki/Dependency_injection)
- [Inversion of Control](https://en.wikipedia.org/wiki/Inversion_of_control)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

### **2. .NET DI Container**
- [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Service Lifetimes](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#service-lifetimes)
- [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

### **3. Testing**
- [Unit Testing with DI](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [Mocking Dependencies](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-moq)

---

## 🎉 **Kết luận**

Dependency Injection là một pattern mạnh mẽ giúp code trở nên:
- **Linh hoạt**: Dễ dàng thay đổi implementations
- **Testable**: Dễ dàng mock dependencies cho testing
- **Maintainable**: Dễ dàng bảo trì và mở rộng
- **Reusable**: Components có thể tái sử dụng

Trong FoodStore system, DI được sử dụng để:
- Tách biệt UI layer và business logic layer
- Tạo ra code dễ test và maintain
- Cho phép mở rộng hệ thống trong tương lai
- Tuân thủ SOLID principles

Việc hiểu và áp dụng đúng DI sẽ giúp bạn viết ra những ứng dụng chất lượng cao và dễ bảo trì.
