# AfrahAPI - توثيق واجهة برمجة التطبيقات
# Afrah API Documentation

> **نظام إدارة حجوزات صالات الأفراح والمناسبات**
> Event Halls Booking Management System

---

## 📋 فهرس المحتويات | Table of Contents

1. [نظرة عامة | Overview](#overview)
2. [المصادقة | Authentication](#authentication)
3. [نقاط النهاية | Endpoints](#endpoints)
   - [الحسابات | Account](#account)
   - [الحجوزات | Booking](#booking)
   - [العملاء | Customer](#customer)
   - [الموظفين | Employee](#employee)
   - [الصالات | Hall](#hall)
   - [أصحاب الصالات | Hall Owner](#hall-owner)
   - [فئات الصالات | Hall Categories](#hall-categories)
   - [وسائط الصالات | Hall Media](#hall-media)
   - [خدمات الصالات | Hall Services](#hall-services)
   - [تقييمات الصالات | Hall Ratings](#hall-ratings)
   - [طرق دفع الصالات | Hall Payment Methods](#hall-payment-methods)
   - [التواريخ غير المتاحة | Hall Unavailable Dates](#hall-unavailable-dates)
   - [الفواتير | Invoice](#invoice)
   - [بنود الفاتورة | Invoice Items](#invoice-items)
   - [المدفوعات | Payment](#payment)
   - [طرق الدفع | Payment Methods](#payment-methods)
   - [الإشعارات | Notifications](#notifications)
   - [تقييمات الخدمات | Service Ratings](#service-ratings)
4. [هياكل البيانات | Data Models](#data-models)
5. [رموز الاستجابة | Response Codes](#response-codes)

---

<a name="overview"></a>
## 🌐 نظرة عامة | Overview

**Base URL:** `https://your-domain.com/api`

**Content-Type:** `application/json`

**المصادقة | Authentication:** JWT Bearer Token

### أدوار المستخدمين | User Roles
- `Customer` - العميل
- `HallOwner` - صاحب الصالة
- `Employee` - الموظف
- `Admin` - المسؤول

---

<a name="authentication"></a>
## 🔐 المصادقة | Authentication

جميع نقاط النهاية المحمية تتطلب إرسال Token في رأس الطلب:

```http
Authorization: Bearer {your_jwt_token}
```

---

<a name="endpoints"></a>
## 📡 نقاط النهاية | Endpoints

---

<a name="account"></a>
### 👤 الحسابات | Account Controller

**Base Path:** `/api/Account`

| Method | Endpoint | Description (AR) | Description (EN) | Auth Required |
|--------|----------|------------------|------------------|---------------|
| `POST` | `/register` | تسجيل مستخدم جديد | Register new user | ❌ |
| `POST` | `/login` | تسجيل الدخول | Login | ❌ |
| `POST` | `/change-password` | تغيير كلمة المرور | Change password | ✅ |
| `GET` | `/roles` | الحصول على الأدوار المتاحة | Get available roles | ❌ |

#### POST `/api/Account/register`
تسجيل مستخدم جديد في النظام

**Request Body:**
```json
{
  "email": "user@example.com",
  "userName": "username",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "phoneNumber": "+966500000000",
  "role": "Customer"
}
```

**Validation Rules:**
- `email`: مطلوب، بريد إلكتروني صالح
- `userName`: مطلوب، 3-50 حرف
- `password`: مطلوب، 8 أحرف على الأقل
- `role`: مطلوب (Customer, HallOwner, Employee)

**Response (200 OK):**
```json
{
  "success": true,
  "message": "تم التسجيل بنجاح",
  "userId": "guid"
}
```

---

#### POST `/api/Account/login`
تسجيل الدخول والحصول على JWT Token

**Request Body:**
```json
{
  "emailOrUserName": "user@example.com",
  "password": "Password123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_string",
  "expiresIn": 3600,
  "userInfo": {
    "userId": "guid",
    "userName": "username",
    "email": "user@example.com",
    "roles": ["Customer"]
  }
}
```

---

#### POST `/api/Account/change-password`
تغيير كلمة المرور (يتطلب تسجيل الدخول)

**Request Body:**
```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!",
  "confirmNewPassword": "NewPassword123!"
}
```

---

<a name="booking"></a>
### 📅 الحجوزات | Booking Controller

**Base Path:** `/api/Booking`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الحجوزات | Get all bookings |
| `GET` | `/{id}` | الحصول على حجز بمعرفه | Get booking by ID |
| `POST` | `/` | إنشاء حجز جديد | Create new booking |
| `PUT` | `/{id}` | تحديث حجز | Update booking |
| `DELETE` | `/{id}` | حذف حجز | Delete booking |
| `POST` | `/create-with-validation` | إنشاء حجز مع التحقق | Create booking with validation |
| `GET` | `/customer/{customerId}` | حجوزات عميل معين | Get customer bookings |
| `GET` | `/hall/{hallId}` | حجوزات صالة معينة | Get hall bookings |
| `PUT` | `/{id}/status` | تحديث حالة الحجز | Update booking status |
| `POST` | `/{id}/cancel` | إلغاء الحجز | Cancel booking |
| `POST` | `/{id}/confirm` | تأكيد الحجز | Confirm booking |
| `POST` | `/{id}/complete` | اكتمال الحجز | Complete booking |
| `GET` | `/calculate-cost` | حساب تكلفة الحجز | Calculate booking cost |

#### POST `/api/Booking`
إنشاء حجز جديد

**Request Body:**
```json
{
  "startDate": "2024-03-15T00:00:00",
  "endDate": "2024-03-15T23:59:59",
  "startTime": "18:00:00",
  "endTime": "23:00:00",
  "pricingMode": "PerHour",
  "totalPrice": 5000.00,
  "depositAmount": 1000.00,
  "depositDueDate": "2024-03-10T00:00:00",
  "isDepositPaid": false,
  "status": "Pending",
  "eventType": "زفاف",
  "numberOfGuests": 200,
  "notes": "ملاحظات إضافية",
  "discountPercentage": 10.0,
  "hallId": "guid",
  "customerId": "guid"
}
```

**Response (201 Created):**
```json
{
  "bookingId": "guid",
  "startDate": "2024-03-15T00:00:00",
  "endDate": "2024-03-15T23:59:59",
  "startTime": "18:00:00",
  "endTime": "23:00:00",
  "pricingMode": "PerHour",
  "totalPrice": 5000.00,
  "depositAmount": 1000.00,
  "depositDueDate": "2024-03-10T00:00:00",
  "isDepositPaid": false,
  "status": "Pending",
  "eventType": "زفاف",
  "numberOfGuests": 200,
  "notes": "ملاحظات إضافية",
  "discountPercentage": 10.0,
  "createdAt": "2024-03-01T12:00:00",
  "updatedAt": null,
  "hallId": "guid",
  "customerId": "guid"
}
```

---

#### GET `/api/Booking/calculate-cost`
حساب تكلفة الحجز

**Query Parameters:**
- `hallId` (Guid): معرف الصالة
- `startDate` (DateTime): تاريخ البدء
- `endDate` (DateTime): تاريخ الانتهاء

**Response:**
```json
{
  "hallId": "guid",
  "startDate": "2024-03-15T00:00:00",
  "endDate": "2024-03-15T23:59:59",
  "totalCost": 5000.00
}
```

---

<a name="customer"></a>
### 👥 العملاء | Customer Controller

**Base Path:** `/api/Customer`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع العملاء | Get all customers |
| `GET` | `/{id}` | الحصول على عميل بمعرفه | Get customer by ID |
| `POST` | `/` | إنشاء عميل جديد | Create new customer |
| `PUT` | `/{id}` | تحديث عميل | Update customer |
| `DELETE` | `/{id}` | حذف عميل | Delete customer |
| `GET` | `/user/{userId}` | الحصول على عميل بمعرف المستخدم | Get customer by user ID |
| `GET` | `/{id}/bookings` | الحصول على حجوزات العميل | Get customer bookings |
| `PUT` | `/{id}/profile` | تحديث ملف العميل الشخصي | Update customer profile |

---

<a name="employee"></a>
### 👷 الموظفين | Employee Controller

**Base Path:** `/api/Employee`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الموظفين | Get all employees |
| `GET` | `/{id}` | الحصول على موظف بمعرفه | Get employee by ID |
| `POST` | `/` | إنشاء موظف جديد | Create new employee |
| `PUT` | `/{id}` | تحديث موظف | Update employee |
| `DELETE` | `/{id}` | حذف موظف | Delete employee |
| `GET` | `/hall/{hallId}` | موظفي صالة معينة | Get employees by hall |

---

<a name="hall"></a>
### 🏛️ الصالات | Hall Controller

**Base Path:** `/api/Hall`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الصالات | Get all halls |
| `GET` | `/{id}` | الحصول على صالة بمعرفها | Get hall by ID |
| `POST` | `/` | إنشاء صالة جديدة | Create new hall |
| `PUT` | `/{id}` | تحديث صالة | Update hall |
| `DELETE` | `/{id}` | حذف صالة | Delete hall |
| `POST` | `/search` | البحث في الصالات | Search halls |
| `GET` | `/available` | الصالات المتاحة | Get available halls |
| `GET` | `/{id}/availability` | فحص توفر صالة | Check hall availability |
| `GET` | `/owner/{ownerId}` | صالات مالك معين | Get halls by owner |
| `GET` | `/category/{categoryId}` | الصالات حسب الفئة | Get halls by category |
| `POST` | `/{id}/verify` | التحقق من الصالة | Verify hall |
| `PUT` | `/{id}/availability` | تحديث توفر الصالة | Update hall availability |

#### POST `/api/Hall`
إنشاء صالة جديدة

**Request Body:**
```json
{
  "hallName": "قاعة الأفراح الكبرى",
  "description": "قاعة فاخرة للمناسبات الكبيرة",
  "address": "الرياض - حي النخيل",
  "latitude": 24.7136,
  "longitude": 46.6753,
  "capacity": 500,
  "pricingMode": "PerDay",
  "pricePerHour": 500.00,
  "pricePerDay": 5000.00,
  "defaultDepositAmount": 1000.00,
  "isAvailable": true,
  "allowsMultipleReservationsPerDay": false,
  "autoAcceptReservations": false,
  "mainImageUrl": "https://example.com/image.jpg",
  "cancellationPolicy": "إلغاء مجاني قبل 7 أيام",
  "baseCommissionRate": 5.0,
  "ownerUserID": "guid",
  "categoryID": "guid"
}
```

---

#### POST `/api/Hall/search`
البحث في الصالات مع الفلاتر

**Request Body:**
```json
{
  "keyword": "فخمة",
  "categoryId": "guid",
  "city": "الرياض",
  "minCapacity": 100,
  "maxCapacity": 500,
  "minPrice": 1000.00,
  "maxPrice": 10000.00,
  "onlyAvailable": true,
  "onlyVerified": true,
  "startDate": "2024-03-15T00:00:00",
  "endDate": "2024-03-16T00:00:00"
}
```

---

#### GET `/api/Hall/available`
الحصول على الصالات المتاحة لنطاق تواريخ

**Query Parameters:**
- `startDate` (DateTime): تاريخ البدء
- `endDate` (DateTime): تاريخ الانتهاء

---

<a name="hall-owner"></a>
### 🏢 أصحاب الصالات | Hall Owner Controller

**Base Path:** `/api/HallOwner`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع أصحاب الصالات | Get all hall owners |
| `GET` | `/{id}` | الحصول على صاحب صالة بمعرفه | Get hall owner by ID |
| `POST` | `/` | إنشاء صاحب صالة جديد | Create new hall owner |
| `PUT` | `/{id}` | تحديث صاحب صالة | Update hall owner |
| `DELETE` | `/{id}` | حذف صاحب صالة | Delete hall owner |
| `GET` | `/user/{userId}` | الحصول على صاحب صالة بمعرف المستخدم | Get hall owner by user ID |
| `GET` | `/{id}/halls` | الحصول على صالات المالك | Get owner's halls |
| `PUT` | `/{id}/profile` | تحديث الملف الشخصي | Update owner profile |
| `GET` | `/{id}/statistics` | إحصائيات صاحب الصالة | Get owner statistics |

---

<a name="hall-categories"></a>
### 📂 فئات الصالات | Hall Categories Controller

**Base Path:** `/api/HallCategorie`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الفئات | Get all categories |
| `GET` | `/{id}` | الحصول على فئة بمعرفها | Get category by ID |
| `POST` | `/` | إنشاء فئة جديدة | Create new category |
| `PUT` | `/{id}` | تحديث فئة | Update category |
| `DELETE` | `/{id}` | حذف فئة | Delete category |
| `GET` | `/{id}/with-halls` | الفئة مع صالاتها | Get category with halls |

---

<a name="hall-media"></a>
### 🖼️ وسائط الصالات | Hall Media Controller

**Base Path:** `/api/HallMedia`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الوسائط | Get all media |
| `GET` | `/{id}` | الحصول على وسائط بمعرفها | Get media by ID |
| `POST` | `/` | إنشاء وسائط جديدة | Create new media |
| `PUT` | `/{id}` | تحديث وسائط | Update media |
| `DELETE` | `/{id}` | حذف وسائط | Delete media |
| `GET` | `/hall/{hallId}` | وسائط صالة معينة | Get media by hall |
| `POST` | `/hall/{hallId}/upload` | رفع وسائط لصالة | Upload media to hall |

---

<a name="hall-services"></a>
### ⚙️ خدمات الصالات | Hall Services Controller

**Base Path:** `/api/HallServices`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الخدمات | Get all services |
| `GET` | `/{id}` | الحصول على خدمة بمعرفها | Get service by ID |
| `POST` | `/` | إنشاء خدمة جديدة | Create new service |
| `PUT` | `/{id}` | تحديث خدمة | Update service |
| `DELETE` | `/{id}` | حذف خدمة | Delete service |
| `GET` | `/hall/{hallId}` | خدمات صالة معينة | Get services by hall |
| `POST` | `/hall/{hallId}/add` | إضافة خدمة لصالة | Add service to hall |

---

<a name="hall-ratings"></a>
### ⭐ تقييمات الصالات | Hall Rating Controller

**Base Path:** `/api/HallRating`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع التقييمات | Get all ratings |
| `GET` | `/{id}` | الحصول على تقييم بمعرفه | Get rating by ID |
| `POST` | `/` | إنشاء تقييم جديد | Create new rating |
| `PUT` | `/{id}` | تحديث تقييم | Update rating |
| `DELETE` | `/{id}` | حذف تقييم | Delete rating |
| `GET` | `/hall/{hallId}` | تقييمات صالة معينة | Get ratings by hall |
| `POST` | `/add` | إضافة تقييم جديد | Add new rating |
| `GET` | `/hall/{hallId}/average` | متوسط تقييم الصالة | Get average rating |

---

<a name="hall-payment-methods"></a>
### 💳 طرق دفع الصالات | Hall Payment Methods Controller

**Base Path:** `/api/HallPaymentMethod`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع ربط طرق الدفع | Get all payment method links |
| `GET` | `/{id}` | الحصول على ربط بمعرفه | Get link by ID |
| `POST` | `/` | إنشاء ربط جديد | Create new link |
| `PUT` | `/{id}` | تحديث ربط | Update link |
| `DELETE` | `/{id}` | حذف ربط | Delete link |
| `GET` | `/hall/{hallId}` | طرق دفع صالة معينة | Get payment methods by hall |
| `GET` | `/payment-method/{paymentMethodId}` | الصالات حسب طريقة الدفع | Get halls by payment method |
| `POST` | `/hall/{hallId}/add/{paymentMethodId}` | إضافة طريقة دفع لصالة | Add payment method to hall |
| `DELETE` | `/hall/{hallId}/remove/{paymentMethodId}` | إزالة طريقة دفع من صالة | Remove payment method from hall |

---

<a name="hall-unavailable-dates"></a>
### 📆 التواريخ غير المتاحة | Hall Unavailable Dates Controller

**Base Path:** `/api/HallUnavailableDate`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع التواريخ المحجوبة | Get all unavailable dates |
| `GET` | `/{id}` | الحصول على تاريخ محجوب بمعرفه | Get unavailable date by ID |
| `POST` | `/` | إنشاء تاريخ محجوب جديد | Create new unavailable date |
| `PUT` | `/{id}` | تحديث تاريخ محجوب | Update unavailable date |
| `DELETE` | `/{id}` | حذف تاريخ محجوب | Delete unavailable date |
| `GET` | `/hall/{hallId}` | التواريخ المحجوبة لصالة | Get unavailable dates by hall |
| `POST` | `/hall/{hallId}/add` | إضافة تاريخ محجوب لصالة | Add unavailable date to hall |

---

<a name="invoice"></a>
### 🧾 الفواتير | Invoice Controller

**Base Path:** `/api/Invoice`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الفواتير | Get all invoices |
| `GET` | `/{id}` | الحصول على فاتورة بمعرفها | Get invoice by ID |
| `POST` | `/` | إنشاء فاتورة جديدة | Create new invoice |
| `PUT` | `/{id}` | تحديث فاتورة | Update invoice |
| `DELETE` | `/{id}` | حذف فاتورة | Delete invoice |
| `POST` | `/generate/{bookingId}` | إنشاء فاتورة لحجز | Generate invoice for booking |
| `GET` | `/customer/{customerId}` | فواتير عميل معين | Get customer invoices |
| `GET` | `/{id}/total` | حساب إجمالي الفاتورة | Calculate invoice total |

---

<a name="invoice-items"></a>
### 📝 بنود الفاتورة | Invoice Items Controller

**Base Path:** `/api/InvoiceItems`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع البنود | Get all items |
| `GET` | `/{id}` | الحصول على بند بمعرفه | Get item by ID |
| `POST` | `/` | إنشاء بند جديد | Create new item |
| `PUT` | `/{id}` | تحديث بند | Update item |
| `DELETE` | `/{id}` | حذف بند | Delete item |
| `GET` | `/invoice/{invoiceId}` | بنود فاتورة معينة | Get items by invoice |
| `POST` | `/invoice/{invoiceId}/add` | إضافة بند لفاتورة | Add item to invoice |
| `GET` | `/{id}/total` | حساب إجمالي البند | Calculate item total |
| `GET` | `/invoice/{invoiceId}/total` | إجمالي بنود الفاتورة | Get invoice items total |

---

<a name="payment"></a>
### 💰 المدفوعات | Payment Controller

**Base Path:** `/api/Payment`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع المدفوعات | Get all payments |
| `GET` | `/{id}` | الحصول على مدفوعة بمعرفها | Get payment by ID |
| `POST` | `/` | إنشاء مدفوعة جديدة | Create new payment |
| `PUT` | `/{id}` | تحديث مدفوعة | Update payment |
| `DELETE` | `/{id}` | حذف مدفوعة | Delete payment |
| `POST` | `/process` | معالجة دفعة جديدة | Process new payment |
| `GET` | `/invoice/{invoiceId}` | مدفوعات فاتورة معينة | Get payments by invoice |
| `POST` | `/{id}/refund` | استرداد مبلغ | Refund payment |

---

<a name="payment-methods"></a>
### 💳 طرق الدفع | Payment Methods Controller

**Base Path:** `/api/PaymentMethod`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع طرق الدفع | Get all payment methods |
| `GET` | `/{id}` | الحصول على طريقة دفع بمعرفها | Get payment method by ID |
| `POST` | `/` | إنشاء طريقة دفع جديدة | Create new payment method |
| `PUT` | `/{id}` | تحديث طريقة دفع | Update payment method |
| `DELETE` | `/{id}` | حذف طريقة دفع | Delete payment method |
| `GET` | `/active` | طرق الدفع النشطة | Get active payment methods |

---

<a name="notifications"></a>
### 🔔 الإشعارات | Notifications Controller

**Base Path:** `/api/Notification`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع الإشعارات | Get all notifications |
| `GET` | `/{id}` | الحصول على إشعار بمعرفه | Get notification by ID |
| `POST` | `/` | إنشاء إشعار جديد | Create new notification |
| `PUT` | `/{id}` | تحديث إشعار | Update notification |
| `DELETE` | `/{id}` | حذف إشعار | Delete notification |
| `GET` | `/user/{userId}` | إشعارات مستخدم معين | Get user notifications |
| `POST` | `/{id}/mark-read` | وضع علامة مقروء | Mark as read |
| `POST` | `/booking-confirmation/{bookingId}` | إرسال تأكيد الحجز | Send booking confirmation |
| `POST` | `/payment-confirmation/{paymentId}` | إرسال تأكيد الدفع | Send payment confirmation |
| `POST` | `/booking-reminder/{bookingId}` | إرسال تذكير بالحجز | Send booking reminder |

---

<a name="service-ratings"></a>
### ⭐ تقييمات الخدمات | Service Ratings Controller

**Base Path:** `/api/ServiceRating`

| Method | Endpoint | Description (AR) | Description (EN) |
|--------|----------|------------------|------------------|
| `GET` | `/` | الحصول على جميع التقييمات | Get all ratings |
| `GET` | `/{id}` | الحصول على تقييم بمعرفه | Get rating by ID |
| `POST` | `/` | إنشاء تقييم جديد | Create new rating |
| `PUT` | `/{id}` | تحديث تقييم | Update rating |
| `DELETE` | `/{id}` | حذف تقييم | Delete rating |
| `GET` | `/service/{serviceId}` | تقييمات خدمة معينة | Get ratings by service |
| `POST` | `/add` | إضافة تقييم جديد | Add new rating |
| `GET` | `/service/{serviceId}/average` | متوسط تقييم الخدمة | Get average rating |

---

<a name="data-models"></a>
## 📦 هياكل البيانات | Data Models

### BookingStatus Values | قيم حالة الحجز
| Value | Description (AR) | Description (EN) |
|-------|------------------|------------------|
| `Pending` | قيد الانتظار | Pending |
| `Confirmed` | مؤكد | Confirmed |
| `Cancelled` | ملغى | Cancelled |
| `Completed` | مكتمل | Completed |

### PricingMode Values | قيم نمط التسعير
| Value | Description (AR) | Description (EN) |
|-------|------------------|------------------|
| `PerHour` | بالساعة | Per Hour |
| `PerDay` | باليوم | Per Day |
| `PerGuest` | لكل ضيف | Per Guest |
| `Package` | باقة | Package |

---

<a name="response-codes"></a>
## 📊 رموز الاستجابة | Response Codes

| Code | Description (AR) | Description (EN) |
|------|------------------|------------------|
| `200` | نجاح | OK - Success |
| `201` | تم الإنشاء | Created |
| `204` | لا محتوى (حذف ناجح) | No Content (Successful Delete) |
| `400` | طلب غير صالح | Bad Request |
| `401` | غير مصرح | Unauthorized |
| `403` | ممنوع | Forbidden |
| `404` | غير موجود | Not Found |
| `500` | خطأ في الخادم | Internal Server Error |

### Error Response Format | صيغة رد الخطأ
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "Email": ["البريد الإلكتروني مطلوب"]
  }
}
```

---

## 🔧 أمثلة على الاستخدام | Usage Examples

### 1. تسجيل مستخدم جديد وتسجيل الدخول
```bash
# Register
curl -X POST https://api.example.com/api/Account/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "userName": "newuser",
    "password": "Password123!",
    "confirmPassword": "Password123!",
    "role": "Customer"
  }'

# Login
curl -X POST https://api.example.com/api/Account/login \
  -H "Content-Type: application/json" \
  -d '{
    "emailOrUserName": "user@example.com",
    "password": "Password123!"
  }'
```

### 2. البحث عن الصالات المتاحة
```bash
curl -X POST https://api.example.com/api/Hall/search \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "city": "الرياض",
    "minCapacity": 100,
    "onlyAvailable": true,
    "startDate": "2024-03-15",
    "endDate": "2024-03-16"
  }'
```

### 3. إنشاء حجز جديد
```bash
curl -X POST https://api.example.com/api/Booking \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "hallId": "hall-guid",
    "customerId": "customer-guid",
    "startDate": "2024-03-15",
    "endDate": "2024-03-15",
    "startTime": "18:00:00",
    "endTime": "23:00:00",
    "pricingMode": "PerDay",
    "status": "Pending",
    "eventType": "زفاف",
    "numberOfGuests": 200
  }'
```

---

## 📞 الدعم | Support

للمساعدة أو الاستفسارات، يرجى التواصل مع فريق التطوير.

For help or inquiries, please contact the development team.

---

> **ملاحظة**: هذا التوثيق تم إنشاؤه تلقائياً من كود المصدر ويعكس الحالة الحالية لواجهة برمجة التطبيقات.
> 
> **Note**: This documentation was auto-generated from source code and reflects the current state of the API.
