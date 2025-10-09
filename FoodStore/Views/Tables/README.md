# Tables - FoodStore System

Thư mục này chứa các table class sử dụng Spectre.Console để hiển thị dữ liệu dưới dạng bảng đẹp mắt và dễ đọc.

## 📋 Danh sách Tables

### 1. **CustomerTable.cs**
- **Mục đích**: Hiển thị thông tin khách hàng và profile
- **Chức năng chính**:
  - Danh sách khách hàng cho nhân viên
  - Profile chi tiết với thống kê mua hàng
  - Lịch sử đơn hàng đã thanh toán

#### 🎯 **Methods chính**:

##### **ShowCustomerTable(CustomerService customerService)**
- Hiển thị bảng danh sách tất cả khách hàng
- Bao gồm: ID, tên, số điện thoại, điểm tích lũy, rank hiện tại
- Sử dụng cho nhân viên quản lý

##### **ShowProfileTable(Customer customer, CustomerService customerService, OrderService orderService)**
- Hiển thị profile chi tiết của khách hàng
- Thông tin cá nhân: tên, số điện thoại, cấp độ thành viên
- Thống kê mua hàng: tổng đơn hàng, tổng chi tiêu
- Chỉ tính đơn hàng đã thanh toán (Status = "Paid")

##### **ShowOrderHistoryTable(OrderService orderService, Customer customer)**
- Hiển thị lịch sử 5 đơn hàng gần nhất
- Sắp xếp theo ngày tạo
- Chỉ hiển thị đơn hàng đã thanh toán
- Format ngày tháng theo chuẩn VN

---

### 2. **OrderTable.cs**
- **Mục đích**: Hiển thị đơn hàng và giỏ hàng
- **Chức năng chính**:
  - Danh sách đơn hàng đã thanh toán cho nhân viên
  - Thống kê tổng thu và đơn hàng trung bình
  - Giỏ hàng hiện tại cho khách hàng

#### 🎯 **Methods chính**:

##### **ShowOrderTable(OrderService orderService)**
- Hiển thị bảng đơn hàng đã thanh toán
- Thông tin: ID đơn hàng, khách hàng, ngày tạo, tổng tiền, trạng thái
- Thống kê: tổng số đơn hàng, tổng thu, đơn hàng trung bình
- Sắp xếp theo ngày tạo

##### **ShowCartTable(Order currentOrder)**
- Hiển thị giỏ hàng hiện tại của khách hàng
- Thông tin: sản phẩm, số lượng, đơn giá, thành tiền
- Chỉ hiển thị khi có sản phẩm trong giỏ hàng
- Format tiền tệ theo chuẩn VN

---

### 3. **ProductTable.cs**
- **Mục đích**: Hiển thị danh sách sản phẩm
- **Chức năng chính**:
  - Bảng sản phẩm đầy đủ cho nhân viên quản lý
  - Bảng sản phẩm đơn giản cho khách hàng mua sắm

#### 🎯 **Methods chính**:

##### **ShowProductTable(ProductService productService)**
- Hiển thị bảng sản phẩm đầy đủ cho nhân viên
- Thông tin: ID, tên sản phẩm, giá, tồn kho, trạng thái
- Trạng thái: "Còn hàng" hoặc "Hết hàng" dựa trên tồn kho
- Có thể quay lại menu chính

##### **ShowShoppingProductTable(ProductService productService)**
- Hiển thị bảng sản phẩm đơn giản cho khách hàng
- Thông tin: ID, tên sản phẩm, giá, tồn kho
- Không hiển thị trạng thái để đơn giản hóa
- Sử dụng trong quá trình mua sắm

---

### 4. **RankTable.cs**
- **Mục đích**: Hiển thị hệ thống cấp độ thành viên và xếp hạng
- **Chức năng chính**:
  - Bảng xếp hạng cho khách hàng
  - Quản lý rank cho nhân viên
  - Thống kê số lượng khách hàng theo cấp độ

#### 🎯 **Methods chính**:

##### **ShowCustomerRankTable(Customer customer, CustomerService customerService)**
- Hiển thị bảng xếp hạng cho khách hàng
- Thông tin rank hiện tại và rank tiếp theo
- Số điểm cần thiết để lên cấp
- Bảng tất cả cấp độ thành viên với trạng thái hiện tại

##### **ShowEmployeeRankTable(CustomerService customerService)**
- Hiển thị quản lý rank cho nhân viên
- Thống kê số lượng khách hàng theo từng cấp độ
- Danh sách khách hàng theo cấp độ (sắp xếp theo điểm)
- Báo cáo chi tiết về hệ thống thành viên

#### 🔧 **Helper Methods**:

##### **GetNextTier(int currentTierId, CustomerService customerService)**
- Lấy thông tin cấp độ thành viên tiếp theo
- Trả về null nếu đã ở cấp cao nhất

##### **GetPointsNeededForTier(int tierId)**
- Lấy số điểm cần thiết để đạt cấp độ
- Thường: 0 điểm, Bạc: 100 điểm, Vàng: 500 điểm, Kim Cương: 1000 điểm

##### **GetTierIdByPoints(int points)**
- Xác định cấp độ dựa trên điểm tích lũy
- Logic đồng bộ với CustomerService và OrderService

---

### 5. **ReceiptTable.cs**
- **Mục đích**: Hiển thị hóa đơn bán hàng chuyên nghiệp
- **Chức năng chính**:
  - Hóa đơn đẹp mắt với header, footer màu sắc
  - Thông tin đầy đủ: đơn hàng, khách hàng, sản phẩm
  - Tính toán chiết khấu, VAT và tổng tiền

#### 🎯 **Methods chính**:

##### **ShowReceiptTable(Order order, CustomerService customerService)**
- Hiển thị hóa đơn đầy đủ với layout chuyên nghiệp
- Header: "HÓA ĐƠN BÁN GẠO" với viền vàng
- Thông tin đơn hàng: số đơn, ngày tạo
- Thông tin khách hàng: tên, số điện thoại, rank, điểm tích lũy
- Bảng sản phẩm: tên, số lượng, đơn giá, giảm giá, thành tiền
- Tóm tắt thanh toán: tạm tính, chiết khấu, VAT, tổng cộng
- Footer: "Cảm ơn quý khách!" với viền xanh

#### 🔧 **Helper Methods**:

##### **CalculateCustomerDiscount(Order order, decimal subtotal, CustomerService customerService)**
- Tính chiết khấu dựa trên cấp độ thành viên
- Sử dụng điểm tích lũy thực tế để xác định cấp độ
- Trả về số tiền được giảm giá

##### **GetTierIdByPoints(int points)**
- Xác định cấp độ thành viên dựa trên điểm
- Logic đồng bộ với các service khác

---

## 🎨 **Thiết kế UI/UX**

### **Spectre.Console Features**:
- **Table Border**: Sử dụng `TableBorder.Square` cho viền vuông đẹp mắt
- **Color Coding**: 
  - Vàng cho header hóa đơn
  - Xanh cho footer hóa đơn
  - Đỏ cho thông báo lỗi
  - Xanh lá cho thông báo thành công
- **Panel**: Sử dụng Panel cho header và footer
- **Formatting**: Tiền tệ VNĐ, ngày tháng VN

### **Layout Structure**:
```
┌─ Header ─┐
│  Title   │
└──────────┘
┌─ Table ──┐
│  Data    │
└──────────┘
┌─ Footer ─┐
│ Message  │
└──────────┘
```

---

## 🔄 **Luồng sử dụng Tables**

### **Cho Nhân viên**:
```
1. CustomerTable.ShowCustomerTable() → Xem danh sách khách hàng
2. ProductTable.ShowProductTable() → Quản lý sản phẩm
3. OrderTable.ShowOrderTable() → Xem đơn hàng và thống kê
4. RankTable.ShowEmployeeRankTable() → Quản lý cấp độ thành viên
5. ReceiptTable.ShowReceiptTable() → In hóa đơn
```

### **Cho Khách hàng**:
```
1. ProductTable.ShowShoppingProductTable() → Xem sản phẩm
2. OrderTable.ShowCartTable() → Xem giỏ hàng
3. CustomerTable.ShowProfileTable() → Xem profile cá nhân
4. CustomerTable.ShowOrderHistoryTable() → Xem lịch sử mua hàng
5. RankTable.ShowCustomerRankTable() → Xem cấp độ thành viên
6. ReceiptTable.ShowReceiptTable() → Xem hóa đơn
```

---

## 🏗️ **Kiến trúc Table Layer**

### **Nguyên tắc thiết kế**:
- **Static Methods**: Tất cả methods đều static, không cần khởi tạo
- **Single Responsibility**: Mỗi table chỉ hiển thị một loại dữ liệu cụ thể
- **Reusability**: Có thể sử dụng lại ở nhiều nơi trong hệ thống
- **Consistent Formatting**: Sử dụng DisplayHelper để format thống nhất

### **Mối quan hệ với các layer khác**:
```
Views ──→ Tables (hiển thị dữ liệu)
Services ──→ Tables (cung cấp dữ liệu)
Utils ──→ Tables (format dữ liệu)
Models ──→ Tables (cấu trúc dữ liệu)
```

---

## 🔧 **Lưu ý kỹ thuật**

### **Performance**:
- **Lazy Loading**: Chỉ load dữ liệu khi cần hiển thị
- **Pagination**: Giới hạn số lượng hiển thị (VD: 5 đơn hàng gần nhất)
- **Efficient Queries**: Sử dụng LINQ để filter và sort

### **Error Handling**:
- **Null Safety**: Kiểm tra null trước khi hiển thị
- **Default Values**: Hiển thị "N/A" hoặc "0" khi không có dữ liệu
- **Graceful Degradation**: Xử lý lỗi một cách mềm mại

### **Localization**:
- **Vietnamese Format**: Tất cả format đều theo chuẩn Việt Nam
- **Currency**: Sử dụng VNĐ làm đơn vị tiền tệ
- **Date Format**: dd/MM/yyyy HH:mm:ss theo chuẩn VN

### **Extensibility**:
- **Configurable**: Có thể tùy chỉnh số lượng hiển thị
- **Modular**: Dễ dàng thêm methods mới
- **Backward Compatible**: Không ảnh hưởng đến code hiện tại

---

## 📈 **Mở rộng trong tương lai**

### **Tính năng có thể thêm**:
- **Export Functions**: Xuất bảng ra Excel, PDF, CSV
- **Interactive Tables**: Bảng có thể tương tác (sort, filter)
- **Real-time Updates**: Cập nhật dữ liệu real-time
- **Custom Themes**: Nhiều theme màu sắc khác nhau
- **Responsive Design**: Tự động điều chỉnh theo kích thước màn hình

### **Tối ưu hóa**:
- **Caching**: Cache dữ liệu thường dùng
- **Async Display**: Hiển thị bất đồng bộ
- **Virtual Scrolling**: Cuộn ảo cho danh sách lớn
- **Search/Filter**: Tìm kiếm và lọc dữ liệu

### **Integration**:
- **Database**: Kết nối trực tiếp với database
- **API**: Tạo REST API cho các table
- **Web Interface**: Chuyển đổi sang web UI
- **Mobile**: Hỗ trợ hiển thị trên mobile

---

## 🎯 **Best Practices**

### **Sử dụng Tables**:
1. **Import đúng namespace**: `using FoodStore.Views.Tables;`
2. **Kiểm tra dữ liệu**: Luôn kiểm tra dữ liệu trước khi hiển thị
3. **Handle exceptions**: Xử lý exception một cách phù hợp
4. **Performance**: Sử dụng static methods để tối ưu performance

### **Maintenance**:
1. **Documentation**: Cập nhật ghi chú khi thay đổi code
2. **Testing**: Viết unit test cho các methods quan trọng
3. **Versioning**: Quản lý version khi có breaking changes
4. **Backup**: Backup code trước khi thay đổi lớn

### **Code Quality**:
1. **Consistent Naming**: Đặt tên method và biến nhất quán
2. **Single Responsibility**: Mỗi method chỉ làm một việc
3. **DRY Principle**: Không lặp lại code
4. **Clean Code**: Code sạch, dễ đọc và bảo trì
