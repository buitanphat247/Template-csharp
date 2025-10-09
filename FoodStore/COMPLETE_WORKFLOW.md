# Luồng Hoạt Động Đầy Đủ - FoodStore System

Giải thích chi tiết toàn bộ luồng hoạt động của dự án FoodStore từ khi khởi động đến khi kết thúc.

## 🚀 **1. Khởi động ứng dụng (Program.cs)**

### **Bước 1.1: Tạo Services**
```csharp
// Program.cs - Main method
static void Main()
{
    // Tạo tất cả services cần thiết
    var productService = new ProductService();
    var orderService = new OrderService();
    var customerService = new CustomerService();
    var employeeService = new EmployeeService();
    
    // Khởi tạo dữ liệu mẫu
    DataSeeder.SeedProducts(productService);
    DataSeeder.SeedCustomers(customerService);
}
```

**Chi tiết:**
- `ProductService`: Quản lý sản phẩm (thêm, sửa, xóa, tìm kiếm)
- `OrderService`: Quản lý đơn hàng (tạo, cập nhật, tính toán)
- `CustomerService`: Quản lý khách hàng (đăng ký, đăng nhập, rank)
- `EmployeeService`: Quản lý nhân viên (đăng nhập, phân quyền)

### **Bước 1.2: Inject Dependencies vào UIManager**
```csharp
// Program.cs - Tiếp tục
var uiManager = new UIManager(
    productService,    // ← Inject ProductService
    orderService,      // ← Inject OrderService
    customerService,   // ← Inject CustomerService
    employeeService    // ← Inject EmployeeService
);

// Bắt đầu ứng dụng
uiManager.ShowMainMenu();
```

---

## 🎯 **2. Menu chính (UIManager.ShowMainMenu)**

### **Bước 2.1: Hiển thị menu**
```csharp
// UIManager.cs
public void ShowMainMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== HỆ THỐNG QUẢN LÝ CỬA HÀNG GẠO ===");
        Console.WriteLine("1. Đăng nhập khách hàng");
        Console.WriteLine("2. Đăng ký khách hàng mới");
        Console.WriteLine("3. Đăng nhập nhân viên");
        Console.WriteLine("4. Thoát");
        Console.Write("\nChọn chức năng (1-4): ");
        
        var choice = Console.ReadLine();
        // Xử lý lựa chọn...
    }
}
```

### **Bước 2.2: Xử lý lựa chọn**
```csharp
switch (choice)
{
    case "1":
        // Chuyển đến CustomerUI để đăng nhập
        _customerUI.ShowCustomerLogin();
        break;
    case "2":
        // Chuyển đến CustomerUI để đăng ký
        _customerUI.ShowCustomerRegister();
        break;
    case "3":
        // Chuyển đến EmployeeUI để đăng nhập
        _employeeUI.ShowEmployeeLogin();
        break;
    case "4":
        // Thoát ứng dụng
        return;
}
```

---

## 👤 **3. Luồng Khách Hàng**

### **3.1. Đăng nhập khách hàng (CustomerUI.ShowCustomerLogin)**

```csharp
// CustomerUI.cs
public void ShowCustomerLogin()
{
    Console.Clear();
    Console.WriteLine("=== ĐĂNG NHẬP KHÁCH HÀNG ===");
    Console.Write("Nhập số điện thoại: ");
    var phone = Console.ReadLine();

    // Sử dụng CustomerService để xác thực
    var customer = _customerService.LoginCustomer(phone ?? "");
    
    if (customer != null)
    {
        DisplayHelper.DisplaySuccess($"Chào mừng {customer.Name}!");
        Console.ReadKey();
        // Chuyển đến giao diện khách hàng
        ShowCustomerInterface(customer);
    }
    else
    {
        DisplayHelper.DisplayError("Không tìm thấy khách hàng!");
        Console.ReadKey();
    }
}
```

**Chi tiết CustomerService.LoginCustomer:**
```csharp
// CustomerService.cs
public Customer? LoginCustomer(string phone)
{
    // Làm sạch số điện thoại
    var cleanedPhone = phone.Trim().Replace("\r", "").Replace("\n", "");
    
    // Tìm khách hàng trong danh sách
    var customer = _customers.FirstOrDefault(c => c.Phone.Trim() == cleanedPhone);
    
    if (customer != null)
    {
        // Đồng bộ hóa TierId dựa trên điểm tích lũy thực tế
        var correctTierId = GetTierIdByPoints(customer.Points);
        if (customer.TierId != correctTierId)
        {
            customer.TierId = correctTierId;
        }
    }
    
    return customer;
}
```

### **3.2. Đăng ký khách hàng mới (CustomerUI.ShowCustomerRegister)**

```csharp
// CustomerUI.cs
public void ShowCustomerRegister()
{
    Console.Clear();
    Console.WriteLine("=== ĐĂNG KÝ KHÁCH HÀNG MỚI ===");
    Console.Write("Nhập họ tên: ");
    var name = Console.ReadLine();
    Console.Write("Nhập số điện thoại: ");
    var phone = Console.ReadLine();

    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
    {
        // Sử dụng CustomerService để đăng ký
        var customer = _customerService.RegisterCustomer(name, phone);
        DisplayHelper.DisplaySuccess($"Đăng ký thành công! Chào mừng {customer.Name}!");
        Console.ReadKey();
        // Chuyển đến giao diện khách hàng
        ShowCustomerInterface(customer);
    }
    else
    {
        DisplayHelper.DisplayError("Thông tin không hợp lệ!");
        Console.ReadKey();
    }
}
```

**Chi tiết CustomerService.RegisterCustomer:**
```csharp
// CustomerService.cs
public Customer RegisterCustomer(string name, string phone, int tierId = 1)
{
    var customer = new Customer
    {
        Id = _nextCustomerId++, // Tự động tăng ID
        Name = name,
        Phone = phone,
        TierId = tierId, // Mặc định là cấp độ "Thường"
        Points = 0, // Điểm tích lũy ban đầu = 0
    };

    _customers.Add(customer);
    return customer;
}
```

### **3.3. Giao diện khách hàng chính (CustomerUI.ShowCustomerInterface)**

```csharp
// CustomerUI.cs
public void ShowCustomerInterface(Customer customer)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== CỬA HÀNG GẠO - {customer.Name} ===");
        Console.WriteLine($"Điểm tích lũy: {customer.Points}");
        Console.WriteLine();
        Console.WriteLine("1. Mua sắm");
        Console.WriteLine("2. Xem profile");
        Console.WriteLine("3. Xem rank");
        Console.WriteLine("4. Đăng xuất");
        Console.Write("\nChọn chức năng (1-4): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                // Tạo ShoppingUI với tất cả services
                var shoppingUI = new ShoppingUI(
                    _productService,
                    _orderService,
                    _customerService,
                    _employeeService
                );
                shoppingUI.ShowShoppingInterface(customer);
                break;
            case "2":
                // Tạo ProfileUI với OrderService và CustomerService
                var profileUI = new ProfileUI(_orderService, _customerService);
                profileUI.ShowCustomerProfile(customer);
                break;
            case "3":
                // Tạo RankUI với CustomerService
                var rankUI = new RankUI(_customerService);
                rankUI.ShowCustomerRank(customer);
                break;
            case "4":
                return; // Quay lại menu chính
        }
    }
}
```

---

## 🛒 **4. Luồng Mua Sắm (ShoppingUI)**

### **4.1. Khởi tạo giao diện mua sắm**

```csharp
// ShoppingUI.cs
public void ShowShoppingInterface(Customer customer)
{
    // Tạo nhân viên mặc định cho đơn hàng
    var employee = new Employee
    {
        Id = 1,
        Name = "Nhân viên",
        RoleId = 1,
    };
    
    // Tạo đơn hàng mới cho khách hàng
    var currentOrder = _orderService.CreateOrder(customer, employee);

    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== MUA SẮM - {customer.Name} ===");
        Console.WriteLine($"Điểm tích lũy: {customer.Points}");
        Console.WriteLine();

        // Hiển thị bảng sản phẩm
        ProductTable.ShowShoppingProductTable(_productService);

        // Hiển thị giỏ hàng hiện tại
        OrderTable.ShowCartTable(currentOrder);
        
        // Hiển thị tổng tiền nếu có sản phẩm
        if (currentOrder.OrderDetails.Any())
        {
            _orderService.CalculateTotal(currentOrder, _customerService);
            Console.WriteLine($"Tổng: {DisplayHelper.FormatCurrency(currentOrder.TotalAmount)}");
        }

        // Menu lựa chọn
        Console.WriteLine("\nChọn sản phẩm (ID) để thêm vào giỏ hàng:");
        Console.WriteLine("Nhập 0 để thanh toán");
        Console.WriteLine("Nhập -1 để quay lại menu chính");
        Console.Write("Lựa chọn: ");

        var input = Console.ReadLine();
        // Xử lý lựa chọn...
    }
}
```

**Chi tiết OrderService.CreateOrder:**
```csharp
// OrderService.cs
public Order CreateOrder(Customer customer, Employee employee)
{
    var order = new Order
    {
        Id = _nextOrderId++,
        CustomerId = customer.Id,
        EmployeeId = employee.Id,
        CreatedAt = DateTime.Now,
        Status = "Pending",
        TotalAmount = 0,
        Customer = customer,
        Employee = employee,
        OrderDetails = new List<OrderDetail>()
    };

    _orders.Add(order);
    return order;
}
```

### **4.2. Thêm sản phẩm vào giỏ hàng**

```csharp
// ShoppingUI.cs - Xử lý lựa chọn sản phẩm
if (int.TryParse(input, out int choice))
{
    if (choice > 0)
    {
        // Lấy sản phẩm theo ID
        var product = _productService.GetProductById(choice);
        if (product != null)
        {
            Console.Write($"Nhập số lượng {product.Name}: ");
            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                // Kiểm tra tồn kho
                if (_productService.CheckStock(product.Id, quantity))
                {
                    // Thêm sản phẩm vào đơn hàng
                    _orderService.AddOrderDetail(currentOrder, product, quantity);
                    // Cập nhật tồn kho
                    _productService.UpdateStock(product.Id, quantity);
                    DisplayHelper.DisplaySuccess($"Đã thêm {product.Name} x{quantity} vào giỏ hàng!");
                }
                else
                {
                    DisplayHelper.DisplayError("Không đủ hàng trong kho!");
                }
            }
        }
    }
}
```

**Chi tiết OrderService.AddOrderDetail:**
```csharp
// OrderService.cs
public void AddOrderDetail(Order order, Product product, int quantity)
{
    var orderDetail = new OrderDetail
    {
        Id = _nextOrderDetailId++,
        OrderId = order.Id,
        ProductId = product.Id,
        Quantity = quantity,
        UnitPrice = product.Price,
        DiscountPercent = 0, // Có thể mở rộng trong tương lai
        Total = product.Price * quantity,
        Product = product
    };

    order.OrderDetails.Add(orderDetail);
}
```

### **4.3. Thanh toán (ShoppingUI.ProcessPayment)**

```csharp
// ShoppingUI.cs
private void ProcessPayment(Order order, Customer customer)
{
    Console.Clear();

    // Lấy thông tin cấp độ thành viên dựa trên điểm tích lũy thực tế
    var currentTierId = GetTierIdByPoints(customer.Points);
    var tier = _customerService.GetTier(currentTierId);
    var discountPercent = tier?.DiscountPercent ?? 0;
    var subtotal = order.OrderDetails.Sum(od => od.Total);
    var discountAmount = subtotal * (decimal)(discountPercent / 100f);

    // Tính tổng tiền cuối cùng sau khi áp dụng chiết khấu
    order.TotalAmount = subtotal - discountAmount;

    // Cập nhật trạng thái đơn hàng
    _orderService.UpdateOrderStatus(order.Id, "Confirmed");

    // Tính điểm tích lũy (1 điểm = 1000 VNĐ)
    var points = (int)(order.TotalAmount / 1000);
    _customerService.UpdatePoints(order.CustomerId, points);

    // Cập nhật trạng thái thanh toán
    _orderService.UpdateOrderStatus(order.Id, "Paid");

    // Hiển thị hóa đơn
    ReceiptTable.ShowReceiptTable(order, _customerService);
}
```

**Chi tiết CustomerService.UpdatePoints:**
```csharp
// CustomerService.cs
public void UpdatePoints(int customerId, int points)
{
    var customer = _customers.FirstOrDefault(c => c.Id == customerId);
    if (customer != null)
    {
        customer.Points += points; // Cộng điểm vào tổng điểm hiện tại
        UpdateCustomerTier(customer); // Kiểm tra và cập nhật cấp độ
    }
}

private void UpdateCustomerTier(Customer customer)
{
    var newTierId = GetTierIdByPoints(customer.Points);
    if (newTierId > customer.TierId) // Chỉ nâng cấp, không hạ cấp
    {
        customer.TierId = newTierId;
        var newTier = GetTier(newTierId);
        Console.WriteLine($"[RANK UP] {customer.Name} đã lên rank {newTier?.Name}!");
    }
}
```

---

## 👨‍💼 **5. Luồng Nhân Viên**

### **5.1. Đăng nhập nhân viên (EmployeeUI.ShowEmployeeLogin)**

```csharp
// EmployeeUI.cs
public void ShowEmployeeLogin()
{
    Console.Clear();
    Console.WriteLine("=== ĐĂNG NHẬP NHÂN VIÊN ===");
    Console.WriteLine("Nhân viên có sẵn:");
    
    // Lấy danh sách tất cả nhân viên
    var employees = _employeeService.GetAllEmployees();
    foreach (var emp in employees)
    {
        Console.WriteLine($"- {emp.Name}");
    }
    Console.Write("\nNhập tên nhân viên: ");
    var name = Console.ReadLine();

    // Xác thực đăng nhập
    var employee = _employeeService.LoginEmployee(name ?? "");
    if (employee != null)
    {
        DisplayHelper.DisplaySuccess($"Chào mừng {employee.Name}!");
        Console.ReadKey();
        ShowEmployeeInterface(employee);
    }
    else
    {
        DisplayHelper.DisplayError("Không tìm thấy nhân viên!");
        Console.ReadKey();
    }
}
```

**Chi tiết EmployeeService.LoginEmployee:**
```csharp
// EmployeeService.cs
public Employee? LoginEmployee(string name)
{
    if (string.IsNullOrEmpty(name))
        return null;

    return _employees.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}
```

### **5.2. Giao diện nhân viên chính (EmployeeUI.ShowEmployeeInterface)**

```csharp
// EmployeeUI.cs
public void ShowEmployeeInterface(Employee employee)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"=== QUẢN LÝ CỬA HÀNG - {employee.Name} ===");
        Console.WriteLine("1. Xem danh sách sản phẩm");
        Console.WriteLine("2. Xem đơn hàng");
        Console.WriteLine("3. Quản lý khách hàng");
        Console.WriteLine("4. Quản lý rank");
        Console.WriteLine("5. Đăng xuất");
        Console.Write("\nChọn chức năng (1-5): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                // Hiển thị bảng sản phẩm đầy đủ
                ProductTable.ShowProductTable(_productService);
                break;
            case "2":
                // Hiển thị bảng đơn hàng với thống kê
                OrderTable.ShowOrderTable(_orderService);
                break;
            case "3":
                // Hiển thị bảng khách hàng
                CustomerTable.ShowCustomerTable(_customerService);
                break;
            case "4":
                // Hiển thị bảng quản lý rank
                RankTable.ShowEmployeeRankTable(_customerService);
                break;
            case "5":
                return; // Quay lại menu chính
        }
    }
}
```

---

## 📊 **6. Các Table Components**

### **6.1. ProductTable.ShowProductTable**

```csharp
// ProductTable.cs
public static void ShowProductTable(ProductService productService)
{
    Console.Clear();
    Console.WriteLine("=== DANH SÁCH SẢN PHẨM ===");
    var products = productService.GetAllProducts();

    // Tạo bảng với Spectre.Console
    var productTable = new Table();
    productTable.Border(TableBorder.Square);
    productTable.AddColumn("ID");
    productTable.AddColumn("Tên sản phẩm");
    productTable.AddColumn("Giá");
    productTable.AddColumn("Tồn kho");
    productTable.AddColumn("Trạng thái");

    // Thêm dữ liệu vào bảng
    foreach (var product in products)
    {
        var status = product.Stock > 0 ? "Còn hàng" : "Hết hàng";
        productTable.AddRow(
            product.Id.ToString(),
            product.Name,
            DisplayHelper.FormatCurrency(product.Price),
            product.Stock.ToString(),
            status
        );
    }

    AnsiConsole.Write(productTable);
    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
    Console.ReadKey();
}
```

### **6.2. OrderTable.ShowOrderTable**

```csharp
// OrderTable.cs
public static void ShowOrderTable(OrderService orderService)
{
    Console.Clear();
    Console.WriteLine("=== DANH SÁCH ĐƠN HÀNG ===");
    var orders = orderService.GetAllOrders().Where(o => o.Status == "Paid");

    // Tạo bảng đơn hàng
    var orderTable = new Table();
    orderTable.Border(TableBorder.Square);
    orderTable.AddColumn("ID Đơn hàng");
    orderTable.AddColumn("Khách hàng");
    orderTable.AddColumn("Ngày tạo");
    orderTable.AddColumn("Tổng tiền");
    orderTable.AddColumn("Trạng thái");

    // Thêm dữ liệu
    foreach (var order in orders)
    {
        orderTable.AddRow(
            $"#{order.Id}",
            order.Customer?.Name ?? "N/A",
            order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"),
            DisplayHelper.FormatCurrency(order.TotalAmount),
            order.Status
        );
    }

    AnsiConsole.Write(orderTable);

    // Thống kê
    var totalOrders = orders.Count();
    var totalRevenue = orders.Sum(o => o.TotalAmount);
    var averageOrder = totalOrders > 0 ? totalRevenue / totalOrders : 0;

    Console.WriteLine($"\nTổng số đơn hàng: {totalOrders}");
    Console.WriteLine($"Tổng thu: {DisplayHelper.FormatCurrency(totalRevenue)}");
    Console.WriteLine($"Đơn hàng trung bình: {DisplayHelper.FormatCurrency(averageOrder)}");

    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
    Console.ReadKey();
}
```

### **6.3. CustomerTable.ShowCustomerTable**

```csharp
// CustomerTable.cs
public static void ShowCustomerTable(CustomerService customerService)
{
    Console.Clear();
    Console.WriteLine("=== DANH SÁCH KHÁCH HÀNG ===");
    var customers = customerService.GetAllCustomers();

    // Tạo bảng khách hàng
    var customerTable = new Table();
    customerTable.Border(TableBorder.Square);
    customerTable.AddColumn("ID");
    customerTable.AddColumn("Họ tên");
    customerTable.AddColumn("Số điện thoại");
    customerTable.AddColumn("Điểm tích lũy");
    customerTable.AddColumn("Rank hiện tại");

    // Thêm dữ liệu
    foreach (var customer in customers)
    {
        var tier = customerService.GetTier(customer.TierId);
        customerTable.AddRow(
            customer.Id.ToString(),
            customer.Name,
            customer.Phone,
            customer.Points.ToString(),
            tier?.Name ?? "N/A"
        );
    }

    AnsiConsole.Write(customerTable);
    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
    Console.ReadKey();
}
```

---

## 🧾 **7. Luồng Hóa Đơn (ReceiptTable)**

### **7.1. Hiển thị hóa đơn**

```csharp
// ReceiptTable.cs
public static void ShowReceiptTable(Order order, CustomerService customerService)
{
    Console.Clear();

    // Header
    var header = new Panel("=== HÓA ĐƠN BÁN GẠO ===")
        .Border(BoxBorder.Double)
        .BorderColor(Color.Yellow);
    AnsiConsole.Write(header);

    // Thông tin đơn hàng và khách hàng
    var orderInfo = new Table();
    orderInfo.Border(TableBorder.Square);
    orderInfo.AddColumn("Thông tin");
    orderInfo.AddColumn("Giá trị");

    orderInfo.AddRow("Số đơn", $"#{order.Id}");
    orderInfo.AddRow("Ngày tạo", order.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"));

    if (order.Customer != null)
    {
        var currentTierId = GetTierIdByPoints(order.Customer.Points);
        var tier = customerService.GetTier(currentTierId);

        orderInfo.AddRow("Khách hàng", order.Customer.Name);
        orderInfo.AddRow("Số điện thoại", order.Customer.Phone);
        orderInfo.AddRow("Rank hiện tại", $"{tier?.Name} ({tier?.DiscountPercent}% giảm giá)");
        orderInfo.AddRow("Điểm tích lũy", order.Customer.Points.ToString());
    }

    AnsiConsole.Write(orderInfo);

    // Bảng sản phẩm
    var productTable = new Table();
    productTable.Border(TableBorder.Square);
    productTable.AddColumn("Sản phẩm");
    productTable.AddColumn("SL");
    productTable.AddColumn("Đơn giá");
    productTable.AddColumn("Giảm%");
    productTable.AddColumn("Thành tiền");

    decimal subtotal = 0;
    foreach (var detail in order.OrderDetails)
    {
        var productName = detail.Product?.Name ?? "N/A";
        var quantity = detail.Quantity.ToString();
        var unitPrice = DisplayHelper.FormatCurrency(detail.UnitPrice);
        var discount = $"{detail.DiscountPercent:F0}%";
        var total = DisplayHelper.FormatCurrency(detail.Total);

        productTable.AddRow(productName, quantity, unitPrice, discount, total);
        subtotal += detail.Total;
    }

    AnsiConsole.Write(productTable);

    // Tóm tắt thanh toán
    var customerDiscount = CalculateCustomerDiscount(order, subtotal, customerService);
    var orderDiscount = 0m;
    var vatRate = 0.08m;
    var vatAmount = (subtotal - customerDiscount - orderDiscount) * vatRate;
    var finalTotal = subtotal - customerDiscount - orderDiscount + vatAmount;

    var summaryTable = new Table();
    summaryTable.Border(TableBorder.Square);
    summaryTable.AddColumn("Mô tả");
    summaryTable.AddColumn("Số tiền");

    summaryTable.AddRow("Tạm tính", DisplayHelper.FormatCurrency(subtotal));
    summaryTable.AddRow("Chiết khấu khách hàng", $"-{DisplayHelper.FormatCurrency(customerDiscount)}");
    summaryTable.AddRow("Chiết khấu đơn hàng", $"-{DisplayHelper.FormatCurrency(orderDiscount)}");
    summaryTable.AddRow("VAT 8%", DisplayHelper.FormatCurrency(vatAmount));

    // Tổng cộng
    var totalRow = new Table();
    totalRow.Border(TableBorder.Square);
    totalRow.AddColumn("TỔNG CỘNG");
    totalRow.AddColumn(DisplayHelper.FormatCurrency(finalTotal));

    AnsiConsole.Write(summaryTable);
    AnsiConsole.Write(totalRow);

    // Footer
    var footer = new Panel("Cảm ơn quý khách!")
        .Border(BoxBorder.Rounded)
        .BorderColor(Color.Blue);
    AnsiConsole.Write(footer);

    Console.WriteLine("\nNhấn phím bất kỳ để quay lại...");
    Console.ReadKey();
}
```

---

## 🔄 **8. Luồng Hoàn Chỉnh - Từ A đến Z**

### **8.1. Khách hàng mua sắm hoàn chỉnh**

```
1. Program.cs khởi tạo services
   ↓
2. UIManager hiển thị menu chính
   ↓
3. Khách hàng chọn "Đăng nhập"
   ↓
4. CustomerUI.ShowCustomerLogin()
   ↓
5. CustomerService.LoginCustomer() xác thực
   ↓
6. CustomerUI.ShowCustomerInterface() hiển thị menu khách hàng
   ↓
7. Khách hàng chọn "Mua sắm"
   ↓
8. ShoppingUI.ShowShoppingInterface() hiển thị sản phẩm
   ↓
9. Khách hàng chọn sản phẩm và số lượng
   ↓
10. ProductService.CheckStock() kiểm tra tồn kho
    ↓
11. OrderService.AddOrderDetail() thêm vào giỏ hàng
    ↓
12. ProductService.UpdateStock() cập nhật tồn kho
    ↓
13. Khách hàng chọn "Thanh toán"
    ↓
14. ShoppingUI.ProcessPayment() xử lý thanh toán
    ↓
15. CustomerService.UpdatePoints() cập nhật điểm tích lũy
    ↓
16. OrderService.UpdateOrderStatus() cập nhật trạng thái đơn hàng
    ↓
17. ReceiptTable.ShowReceiptTable() hiển thị hóa đơn
    ↓
18. Quay lại menu khách hàng
```

### **8.2. Nhân viên quản lý hệ thống**

```
1. Program.cs khởi tạo services
   ↓
2. UIManager hiển thị menu chính
   ↓
3. Nhân viên chọn "Đăng nhập nhân viên"
   ↓
4. EmployeeUI.ShowEmployeeLogin() hiển thị danh sách nhân viên
   ↓
5. EmployeeService.LoginEmployee() xác thực
   ↓
6. EmployeeUI.ShowEmployeeInterface() hiển thị menu quản lý
   ↓
7. Nhân viên chọn chức năng quản lý:
   - Xem sản phẩm: ProductTable.ShowProductTable()
   - Xem đơn hàng: OrderTable.ShowOrderTable()
   - Quản lý khách hàng: CustomerTable.ShowCustomerTable()
   - Quản lý rank: RankTable.ShowEmployeeRankTable()
   ↓
8. Quay lại menu quản lý hoặc đăng xuất
```

---

## 🎯 **9. Tóm tắt Luồng Hoạt Động**

### **Kiến trúc tổng thể:**
```
Program.cs (Entry Point)
    ↓
UIManager (Main Controller)
    ↓
├── CustomerUI (Customer Flow)
│   ├── ShoppingUI (Shopping Process)
│   ├── ProfileUI (Profile Management)
│   └── RankUI (Rank System)
│
└── EmployeeUI (Employee Flow)
    ├── ProductTable (Product Management)
    ├── OrderTable (Order Management)
    ├── CustomerTable (Customer Management)
    └── RankTable (Rank Management)
```

### **Dependencies Flow:**
```
Services (Business Logic)
    ↓
UIManager (Inject all services)
    ↓
UI Components (Inject needed services)
    ↓
Table Components (Display data)
```

### **Data Flow:**
```
User Input → UI Component → Service → Business Logic → Data Update → UI Display
```

### **Key Features:**
- **Dependency Injection**: Tất cả services được inject từ Program.cs
- **Separation of Concerns**: UI, Business Logic, Data tách biệt rõ ràng
- **Reusable Components**: Table components có thể tái sử dụng
- **Error Handling**: Xử lý lỗi ở mọi tầng
- **User Experience**: Giao diện đẹp với Spectre.Console

Đây là luồng hoạt động đầy đủ của hệ thống FoodStore từ khi khởi động đến khi kết thúc, bao gồm tất cả các tương tác giữa các component và cách dữ liệu được xử lý qua các tầng khác nhau.
