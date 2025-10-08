# Models - FoodStore System

Thư mục này chứa các model (entity) đại diện cho cấu trúc dữ liệu của hệ thống cửa hàng thực phẩm.

## 📋 Danh sách Models

### 1. **Customer.cs**
- **Mục đích**: Quản lý thông tin khách hàng
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên khách hàng
  - `Phone`: Số điện thoại
  - `TierId`: ID cấp độ thành viên
  - `Points`: Điểm tích lũy
- **Mối quan hệ**: 
  - `Tier` (Navigation Property) → MemberTier

### 2. **Employee.cs**
- **Mục đích**: Quản lý thông tin nhân viên
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên nhân viên
  - `RoleId`: ID vai trò
- **Mối quan hệ**:
  - `Role` (Navigation Property) → Role

### 3. **Product.cs**
- **Mục đích**: Quản lý sản phẩm trong cửa hàng
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên sản phẩm
  - `Category`: Danh mục sản phẩm
  - `Price`: Giá bán
  - `Stock`: Số lượng tồn kho
  - `Status`: Trạng thái (Active/Inactive/OutOfStock)

### 4. **Order.cs**
- **Mục đích**: Quản lý đơn hàng
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `CustomerId`: ID khách hàng
  - `EmployeeId`: ID nhân viên xử lý
  - `TotalAmount`: Tổng tiền
  - `Status`: Trạng thái đơn hàng
  - `CreatedAt`: Thời gian tạo
- **Mối quan hệ**:
  - `Customer` (Navigation Property) → Customer
  - `Employee` (Navigation Property) → Employee
  - `OrderDetails` (Collection) → List<OrderDetail>

### 5. **OrderDetail.cs**
- **Mục đích**: Chi tiết sản phẩm trong đơn hàng
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `OrderId`: ID đơn hàng
  - `ProductId`: ID sản phẩm
  - `Quantity`: Số lượng
  - `UnitPrice`: Giá đơn vị
  - `DiscountPercent`: Phần trăm giảm giá
  - `Total`: Tổng tiền chi tiết
- **Mối quan hệ**:
  - `Order` (Navigation Property) → Order
  - `Product` (Navigation Property) → Product

### 6. **MemberTier.cs**
- **Mục đích**: Quản lý cấp độ thành viên
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên cấp độ (Bronze, Silver, Gold, Platinum)
  - `DiscountPercent`: Phần trăm giảm giá

### 7. **Role.cs**
- **Mục đích**: Quản lý vai trò nhân viên
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên vai trò (Manager, Cashier, Stock Keeper)
  - `Description`: Mô tả vai trò

### 8. **DiscountPolicy.cs**
- **Mục đích**: Quản lý chính sách giảm giá
- **Thuộc tính chính**:
  - `Id`: ID duy nhất
  - `Name`: Tên chính sách
  - `Type`: Loại giảm giá (Product/Tier/Order)
  - `ProductId`: ID sản phẩm (nếu Type = "Product")
  - `TierId`: ID cấp độ (nếu Type = "Tier")
  - `MinOrderAmount`: Số tiền tối thiểu
  - `DiscountPercent`: Phần trăm giảm giá
  - `StartDate/EndDate`: Thời gian áp dụng
  - `IsActive`: Trạng thái hoạt động
- **Mối quan hệ**:
  - `Product` (Navigation Property) → Product
  - `Tier` (Navigation Property) → MemberTier

## 🔗 Sơ đồ mối quan hệ

```
Customer (1) ←→ (1) MemberTier
    ↓ (1)
    ↓ (Many)
Order (Many) ←→ (1) Employee
    ↓ (1)
    ↓ (Many)
OrderDetail (Many) ←→ (1) Product

Employee (Many) ←→ (1) Role

DiscountPolicy (Many) ←→ (1) Product
DiscountPolicy (Many) ←→ (1) MemberTier
```

## 📊 Mối quan hệ chi tiết

### One-to-Many (1:N)
- **Customer → Order**: Một khách hàng có thể có nhiều đơn hàng
- **Employee → Order**: Một nhân viên có thể xử lý nhiều đơn hàng
- **Order → OrderDetail**: Một đơn hàng có nhiều chi tiết
- **Product → OrderDetail**: Một sản phẩm có thể xuất hiện trong nhiều chi tiết đơn hàng
- **MemberTier → Customer**: Một cấp độ có nhiều khách hàng
- **Role → Employee**: Một vai trò có nhiều nhân viên

### Many-to-One (N:1)
- **Order → Customer**: Nhiều đơn hàng thuộc về một khách hàng
- **Order → Employee**: Nhiều đơn hàng được xử lý bởi một nhân viên
- **OrderDetail → Order**: Nhiều chi tiết thuộc về một đơn hàng
- **OrderDetail → Product**: Nhiều chi tiết tham chiếu đến một sản phẩm

### Many-to-Many (N:N)
- **Product ↔ DiscountPolicy**: Một sản phẩm có thể có nhiều chính sách giảm giá
- **MemberTier ↔ DiscountPolicy**: Một cấp độ có thể có nhiều chính sách giảm giá

## 🎯 Mục đích sử dụng

### Quản lý khách hàng
- Lưu trữ thông tin cá nhân
- Theo dõi cấp độ thành viên
- Tích lũy điểm thưởng

### Quản lý sản phẩm
- Danh mục sản phẩm đa dạng
- Quản lý tồn kho
- Theo dõi trạng thái sản phẩm

### Quản lý đơn hàng
- Theo dõi toàn bộ quy trình đặt hàng
- Tính toán giá và giảm giá
- Quản lý trạng thái đơn hàng

### Hệ thống giảm giá
- Nhiều loại giảm giá linh hoạt
- Áp dụng theo sản phẩm, cấp độ, hoặc đơn hàng
- Quản lý thời gian áp dụng

## 🔧 Lưu ý kỹ thuật

- Tất cả model đều sử dụng **XML Documentation** để mô tả chi tiết
- Navigation properties được đánh dấu nullable (`?`) để tránh lỗi null reference
- Foreign keys được đặt tên theo convention: `{EntityName}Id`
- Sử dụng `List<T>` cho mối quan hệ One-to-Many
- Default values được thiết lập phù hợp với business logic
