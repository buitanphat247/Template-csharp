# Utils - FoodStore System

Thư mục này chứa các utility class cung cấp các helper methods và formatter cho hệ thống cửa hàng thực phẩm.

## 📋 Danh sách Utils

### 1. **DisplayHelper.cs**
- **Mục đích**: Utility class cho việc hiển thị dữ liệu và tạo UI elements
- **Chức năng chính**:
  - Format tiền tệ theo chuẩn Việt Nam
  - Format ngày tháng theo định dạng VN
  - Tạo UI elements (tiêu đề, phân cách)
  - Hiển thị thông báo với màu sắc

#### 🎨 **Tính năng hiển thị**:
| Method | Chức năng | Ví dụ |
|--------|-----------|-------|
| `FormatCurrency()` | Format tiền tệ VNĐ | "1,000,000 VNĐ" |
| `FormatDateTime()` | Format ngày tháng | "25/12/2023 14:30:45" |
| `CreateSeparator()` | Tạo dòng phân cách | "==================================================" |
| `DisplayTitle()` | Hiển thị tiêu đề đẹp | Tiêu đề có dòng phân cách |
| `DisplayError()` | Thông báo lỗi (màu đỏ) | "Lỗi: Không tìm thấy sản phẩm" |
| `DisplaySuccess()` | Thông báo thành công (màu xanh) | "✓ Đơn hàng đã được tạo" |

#### 🔧 **Methods chi tiết**:

##### **FormatCurrency(decimal amount)**
- Format số tiền với dấu phẩy phân cách hàng nghìn
- Thêm đơn vị "VNĐ" vào cuối
- Sử dụng format `N0` để hiển thị số nguyên

##### **FormatDateTime(DateTime dateTime)**
- Format theo chuẩn Việt Nam: dd/MM/yyyy HH:mm:ss
- Hiển thị đầy đủ thông tin ngày, tháng, năm, giờ, phút, giây

##### **CreateSeparator(int length = 50)**
- Tạo dòng phân cách với ký tự '='
- Độ dài mặc định 50 ký tự, có thể tùy chỉnh

##### **DisplayTitle(string title)**
- Hiển thị tiêu đề với format đẹp
- Có dòng phân cách trên và dưới
- Tiêu đề được indent 2 space

##### **DisplayError(string message)**
- Hiển thị thông báo lỗi với màu đỏ
- Có prefix "Lỗi:" để dễ nhận biết
- Tự động reset màu sau khi hiển thị

##### **DisplaySuccess(string message)**
- Hiển thị thông báo thành công với màu xanh
- Có icon ✓ để tăng tính trực quan
- Tự động reset màu sau khi hiển thị

---

### 2. **ReceiptFormatter.cs**
- **Mục đích**: Formatter chuyên dụng để tính toán và format hóa đơn
- **Chức năng chính**:
  - Tính chiết khấu theo cấp độ thành viên
  - Tính VAT 8% theo quy định Việt Nam
  - Tính tổng tiền cuối cùng
  - Tính tổng khối lượng đơn hàng

#### 🧮 **Công thức tính toán hóa đơn**:
```
1. Subtotal = Tổng tiền các sản phẩm
2. Customer Discount = Subtotal × (Discount Percent / 100)
3. VAT = (Subtotal - Discounts) × 8%
4. Final Total = Subtotal - Discounts + VAT
```

#### 📊 **Bảng chiết khấu theo cấp độ**:
| Cấp độ | Điểm yêu cầu | Chiết khấu | Mô tả |
|--------|--------------|------------|-------|
| Thường | 0-99 | 0% | Không giảm giá |
| Bạc | 100-499 | 3% | Giảm giá cơ bản |
| Vàng | 500-999 | 5% | Giảm giá trung bình |
| Kim Cương | 1000+ | 10% | Giảm giá cao nhất |

#### 🔧 **Methods chi tiết**:

##### **CalculateCustomerDiscount(Order order, CustomerService customerService)**
- Tính chiết khấu dựa trên cấp độ thành viên
- Sử dụng điểm tích lũy thực tế để xác định cấp độ
- Trả về số tiền được giảm giá (VNĐ)

##### **CalculateSubtotal(Order order)**
- Tính tổng tiền trước thuế
- Chưa áp dụng giảm giá và VAT
- Tổng của tất cả chi tiết đơn hàng

##### **CalculateVAT(decimal subtotal, decimal customerDiscount, decimal orderDiscount = 0)**
- Tính thuế VAT 8% theo quy định Việt Nam
- VAT được tính trên số tiền sau khi trừ chiết khấu
- Hỗ trợ chiết khấu đơn hàng (có thể mở rộng)

##### **CalculateFinalTotal(Order order, CustomerService customerService)**
- Tính tổng tiền cuối cùng khách hàng phải trả
- Áp dụng đầy đủ công thức: Subtotal - Discounts + VAT
- Trả về số tiền cuối cùng (VNĐ)

##### **CalculateTotalWeight(Order order)**
- Tính tổng khối lượng đơn hàng (kg)
- Dựa trên tên sản phẩm để xác định trọng lượng
- Hỗ trợ các sản phẩm có trọng lượng khác nhau

##### **GetProductWeight(string productName)**
- Xác định trọng lượng sản phẩm dựa trên tên
- Phân tích tên sản phẩm để tìm thông tin trọng lượng
- Hỗ trợ: 1kg (mặc định), 2kg, 5kg, 10kg

---

## 🔄 **Luồng sử dụng Utils**

### **Quy trình hiển thị dữ liệu**:
```
1. Format dữ liệu (DisplayHelper)
   ├── FormatCurrency() → Hiển thị tiền tệ
   ├── FormatDateTime() → Hiển thị ngày tháng
   └── CreateSeparator() → Tạo phân cách

2. Hiển thị UI (DisplayHelper)
   ├── DisplayTitle() → Tiêu đề section
   ├── DisplaySuccess() → Thông báo thành công
   └── DisplayError() → Thông báo lỗi
```

### **Quy trình tính toán hóa đơn**:
```
1. Tính tổng tiền (ReceiptFormatter)
   ├── CalculateSubtotal() → Tổng trước thuế
   ├── CalculateCustomerDiscount() → Chiết khấu khách hàng
   └── CalculateVAT() → Thuế VAT

2. Tính tổng cuối cùng
   └── CalculateFinalTotal() → Tổng tiền cuối cùng

3. Tính khối lượng
   └── CalculateTotalWeight() → Tổng khối lượng đơn hàng
```

---

## 🏗️ **Kiến trúc Utils**

### **Nguyên tắc thiết kế**:
- **Static Methods**: Tất cả methods đều static, không cần khởi tạo
- **Pure Functions**: Không có side effects, chỉ tính toán và trả về kết quả
- **Single Responsibility**: Mỗi method chỉ làm một việc cụ thể
- **Reusability**: Có thể sử dụng lại ở nhiều nơi trong hệ thống

### **Mối quan hệ với các layer khác**:
```
Views ──→ Utils (hiển thị dữ liệu)
Services ──→ Utils (tính toán hóa đơn)
Models ──→ Utils (format dữ liệu)
```

---

## 🔧 **Lưu ý kỹ thuật**

### **Performance**:
- **Static Methods**: Không cần khởi tạo object, tiết kiệm memory
- **String Interpolation**: Sử dụng `$""` để format string hiệu quả
- **LINQ**: Sử dụng LINQ để tính toán nhanh chóng

### **Error Handling**:
- **Null Safety**: Kiểm tra null trước khi xử lý
- **Default Values**: Có giá trị mặc định cho các trường hợp đặc biệt
- **Graceful Degradation**: Xử lý lỗi một cách mềm mại

### **Localization**:
- **Vietnamese Format**: Tất cả format đều theo chuẩn Việt Nam
- **Currency**: Sử dụng VNĐ làm đơn vị tiền tệ
- **Date Format**: dd/MM/yyyy theo chuẩn VN

### **Extensibility**:
- **Configurable**: Có thể tùy chỉnh các tham số (độ dài separator, VAT rate)
- **Modular**: Dễ dàng thêm methods mới
- **Backward Compatible**: Không ảnh hưởng đến code hiện tại

---

## 📈 **Mở rộng trong tương lai**

### **Tính năng có thể thêm**:
- **Export Functions**: Xuất dữ liệu ra Excel, PDF
- **Chart Generation**: Tạo biểu đồ từ dữ liệu
- **Email Formatting**: Format nội dung email
- **SMS Formatting**: Format tin nhắn SMS
- **QR Code Generation**: Tạo mã QR cho hóa đơn

### **Tối ưu hóa**:
- **Caching**: Cache kết quả tính toán phức tạp
- **Async Methods**: Xử lý bất đồng bộ cho các tác vụ nặng
- **Configuration**: File config cho các tham số
- **Logging**: Ghi log các hoạt động quan trọng

### **Integration**:
- **Database**: Kết nối với database để lưu trữ
- **API**: Tạo REST API cho các utility functions
- **Microservices**: Tách thành các service riêng biệt
- **Cloud**: Deploy lên cloud để sử dụng chung

---

## 🎯 **Best Practices**

### **Sử dụng Utils**:
1. **Import đúng namespace**: `using FoodStore.Utils;`
2. **Kiểm tra null**: Luôn kiểm tra input trước khi sử dụng
3. **Handle exceptions**: Xử lý exception một cách phù hợp
4. **Performance**: Sử dụng static methods để tối ưu performance

### **Maintenance**:
1. **Documentation**: Cập nhật ghi chú khi thay đổi code
2. **Testing**: Viết unit test cho các methods quan trọng
3. **Versioning**: Quản lý version khi có breaking changes
4. **Backup**: Backup code trước khi thay đổi lớn
