# Services - FoodStore System

Thư mục này chứa các service layer xử lý logic nghiệp vụ của hệ thống cửa hàng thực phẩm.

## 📋 Danh sách Services

### 1. **CustomerService.cs**
- **Mục đích**: Quản lý khách hàng và hệ thống cấp độ thành viên
- **Chức năng chính**:
  - Đăng ký khách hàng mới
  - Đăng nhập khách hàng bằng số điện thoại
  - Quản lý 4 cấp độ thành viên với mức giảm giá khác nhau
  - Tự động nâng cấp cấp độ dựa trên điểm tích lũy
  - Đồng bộ hóa cấp độ thành viên

#### 🎯 **Cấp độ thành viên**:
| Cấp độ | Điểm yêu cầu | Giảm giá | Mô tả |
|--------|--------------|----------|-------|
| Thường | 0-99 | 0% | Khách hàng mới |
| Bạc | 100-499 | 3% | Khách hàng thân thiết |
| Vàng | 500-999 | 5% | Khách hàng VIP |
| Kim Cương | 1000+ | 10% | Khách hàng cao cấp |

#### 🔧 **Methods chính**:
- `RegisterCustomer()` - Đăng ký khách hàng mới
- `LoginCustomer()` - Đăng nhập bằng số điện thoại
- `UpdatePoints()` - Cập nhật điểm tích lũy
- `SyncAllCustomerTiers()` - Đồng bộ cấp độ tất cả khách hàng

---

### 2. **EmployeeService.cs**
- **Mục đích**: Quản lý nhân viên và phân quyền
- **Chức năng chính**:
  - Đăng nhập nhân viên bằng tên
  - Quản lý vai trò và quyền hạn
  - Tìm kiếm nhân viên

#### 👥 **Vai trò nhân viên**:
| Vai trò | Mô tả | Quyền hạn |
|---------|-------|-----------|
| Nhân viên bán hàng | Bán hàng và tư vấn khách hàng | Xử lý đơn hàng, bán sản phẩm |
| Quản lý | Quản lý cửa hàng và nhân viên | Toàn quyền truy cập hệ thống |
| Kế toán | Quản lý tài chính và báo cáo | Xem báo cáo, quản lý tài chính |

#### 🔧 **Methods chính**:
- `LoginEmployee()` - Đăng nhập nhân viên
- `GetAllEmployees()` - Lấy danh sách nhân viên
- `GetRole()` - Lấy thông tin vai trò

---

### 3. **OrderService.cs**
- **Mục đích**: Quản lý đơn hàng và tính toán giá
- **Chức năng chính**:
  - Tạo đơn hàng mới
  - Thêm sản phẩm vào đơn hàng
  - Tính tổng tiền với giảm giá theo cấp độ thành viên
  - Quản lý trạng thái đơn hàng

#### 📊 **Quy trình xử lý đơn hàng**:
1. **Tạo đơn hàng** → Trạng thái "Pending"
2. **Thêm sản phẩm** → Cập nhật chi tiết đơn hàng
3. **Tính tổng tiền** → Áp dụng giảm giá theo cấp độ
4. **Cập nhật trạng thái** → "Processing" → "Completed"

#### 🔧 **Methods chính**:
- `CreateOrder()` - Tạo đơn hàng mới
- `AddOrderDetail()` - Thêm sản phẩm vào đơn hàng
- `CalculateTotal()` - Tính tổng tiền với giảm giá
- `UpdateOrderStatus()` - Cập nhật trạng thái đơn hàng

---

### 4. **ProductService.cs**
- **Mục đích**: Quản lý sản phẩm và tồn kho
- **Chức năng chính**:
  - Thêm sản phẩm mới
  - Tìm kiếm sản phẩm theo tên
  - Quản lý tồn kho
  - Kiểm tra hàng có sẵn

#### 📦 **Quản lý sản phẩm**:
- **Thêm sản phẩm**: Tự động gán ID, lưu thông tin
- **Tìm kiếm**: Hỗ trợ tìm kiếm không phân biệt hoa thường
- **Tồn kho**: Kiểm tra và cập nhật số lượng hàng

#### 🔧 **Methods chính**:
- `AddProduct()` - Thêm sản phẩm mới
- `SearchProducts()` - Tìm kiếm sản phẩm
- `UpdateStock()` - Cập nhật tồn kho khi bán
- `CheckStock()` - Kiểm tra hàng có sẵn

---

## 🔄 **Luồng xử lý nghiệp vụ**

### **Quy trình bán hàng**:
```
1. Nhân viên đăng nhập (EmployeeService)
2. Khách hàng đăng nhập (CustomerService)
3. Tạo đơn hàng mới (OrderService)
4. Thêm sản phẩm vào đơn hàng (ProductService + OrderService)
5. Tính tổng tiền với giảm giá (OrderService)
6. Cập nhật tồn kho (ProductService)
7. Cập nhật điểm tích lũy (CustomerService)
8. Hoàn tất đơn hàng (OrderService)
```

### **Quy trình nâng cấp cấp độ thành viên**:
```
1. Khách hàng mua hàng → Tích lũy điểm
2. CustomerService.UpdatePoints() → Cộng điểm
3. UpdateCustomerTier() → Kiểm tra cấp độ mới
4. Nếu đủ điểm → Tự động nâng cấp
5. Thông báo rank up cho khách hàng
```

---

## 🏗️ **Kiến trúc Service Layer**

### **Nguyên tắc thiết kế**:
- **Single Responsibility**: Mỗi service chỉ xử lý một domain cụ thể
- **Dependency Injection**: Services có thể sử dụng lẫn nhau
- **Data Consistency**: Đảm bảo tính nhất quán dữ liệu
- **Business Logic**: Tách biệt logic nghiệp vụ khỏi UI

### **Mối quan hệ giữa các Services**:
```
OrderService ──→ CustomerService (lấy thông tin cấp độ)
OrderService ──→ ProductService (kiểm tra tồn kho)
CustomerService ──→ MemberTier (quản lý cấp độ)
EmployeeService ──→ Role (quản lý vai trò)
```

---

## 🔧 **Lưu ý kỹ thuật**

### **Auto-increment ID**:
- Mỗi service quản lý ID riêng
- `_nextCustomerId`, `_nextOrderId`, `_nextDetailId`, `_nextId`
- Tự động tăng khi tạo entity mới

### **Data Validation**:
- Kiểm tra null/empty input
- Validate business rules (điểm tích lũy, tồn kho)
- Clean input data (trim, remove special characters)

### **Error Handling**:
- Return null cho các method tìm kiếm
- Return boolean cho các method cập nhật
- Console output cho thông báo quan trọng

### **Performance**:
- Sử dụng LINQ để query dữ liệu
- In-memory storage (có thể nâng cấp lên database)
- Efficient search với StringComparison.OrdinalIgnoreCase

---

## 📈 **Mở rộng trong tương lai**

### **Tính năng có thể thêm**:
- **Logging**: Ghi log các hoạt động quan trọng
- **Caching**: Cache dữ liệu thường dùng
- **Validation**: Thêm validation rules chi tiết
- **Database**: Chuyển từ in-memory sang database
- **API**: Tạo REST API cho các service
- **Unit Tests**: Viết test cases cho từng service

### **Tối ưu hóa**:
- **Async/Await**: Xử lý bất đồng bộ
- **Repository Pattern**: Tách biệt data access
- **Factory Pattern**: Tạo objects phức tạp
- **Observer Pattern**: Thông báo sự kiện giữa services
