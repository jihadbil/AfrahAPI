# خطة التنفيذ - شاشات المصادقة والتسجيل

## نظرة عامة

هذه الخطة تحول التصميم إلى مهام تنفيذية قابلة للتطبيق. كل مهمة تبني على المهام السابقة وتنتهي بربط جميع المكونات معاً. جميع المهام تركز فقط على كتابة أو تعديل أو اختبار الكود.

## المهام

- [x] 1. إعداد البنية الأساسية والمراجع
  - إضافة مرجع لمشروع `AfrahAPI.Models.DTOs` في مشروع AfrahApp
  - إنشاء هيكل المجلدات: `Services`, `ViewModels`, `Views`, `Helpers`, `Converters`
  - تثبيت الحزم المطلوبة: `CommunityToolkit.Mvvm`, `Microsoft.Extensions.Http`
  - إنشاء ملف `Constants.cs` في مجلد Helpers مع جميع الثوابت (API URLs, رسائل الأخطاء، إلخ)
  - _Requirements: 12.1, 12.6_

- [x] 2. تنفيذ Helpers والأدوات المساعدة
  - [x] 2.1 إنشاء `ValidationHelper.cs`
    - تنفيذ `IsValidEmail()` للتحقق من صحة البريد الإلكتروني
    - تنفيذ `IsValidPhoneNumber()` للتحقق من صحة رقم الهاتف
    - تنفيذ `CalculateAge()` لحساب العمر من تاريخ الميلاد
    - تنفيذ `IsValidPassword()` للتحقق من طول كلمة المرور
    - تنفيذ `IsValidName()` للتحقق من طول الاسم
    - _Requirements: 2.3, 4.1, 4.3, 4.7, 4.9, 4.10_
  
  - [ ]* 2.2 كتابة اختبارات وحدة لـ ValidationHelper
    - اختبار `IsValidEmail` مع بريد إلكتروني صالح وغير صالح
    - اختبار `CalculateAge` مع تواريخ مختلفة
    - اختبار `IsValidPhoneNumber` مع أرقام صالحة وغير صالحة
    - اختبار `IsValidPassword` مع كلمات مرور مختلفة الأطوال
    - _Requirements: 2.3, 4.1, 4.3, 4.9, 4.10_
  
  - [ ]* 2.3 كتابة اختبار خاصية للتحقق من البريد الإلكتروني
    - **Property 6: Email Validation**
    - **Validates: Requirements 2.3, 2.4, 4.1, 4.2**
  
  - [ ]* 2.4 كتابة اختبار خاصية للتحقق من كلمة المرور
    - **Property 10: Password Length Validation**
    - **Validates: Requirements 4.3, 4.4**
  
  - [ ]* 2.5 كتابة اختبار خاصية للتحقق من العمر
    - **Property 14: Age Validation**
    - **Validates: Requirements 4.10, 4.11**

- [x] 3. تنفيذ Result Helper Class
  - إنشاء `Result.cs` في مجلد Helpers
  - تنفيذ `Result<T>` class مع `IsSuccess`, `Data`, `ErrorMessage`
  - تنفيذ `Success()` و `Failure()` static methods
  - _Requirements: 1.4, 1.5, 1.6_

- [ ] 4. تنفيذ Services الأساسية
  - [x] 4.1 إنشاء `ISecureStorageService` و `SecureStorageService`
    - تنفيذ `SaveTokenAsync()` لحفظ JWT Token
    - تنفيذ `GetTokenAsync()` لاسترجاع JWT Token
    - تنفيذ `SaveRefreshTokenAsync()` لحفظ Refresh Token
    - تنفيذ `GetRefreshTokenAsync()` لاسترجاع Refresh Token
    - تنفيذ `SaveUserInfoAsync()` لحفظ معلومات المستخدم
    - تنفيذ `GetUserInfoAsync()` لاسترجاع معلومات المستخدم
    - تنفيذ `RemoveAllAsync()` لحذف جميع البيانات
    - استخدام `SecureStorage` من MAUI
    - _Requirements: 1.2, 8.1, 8.2, 8.3, 8.5, 8.6_
  
  - [ ]* 4.2 كتابة اختبارات وحدة لـ SecureStorageService
    - اختبار حفظ واسترجاع Token
    - اختبار حذف جميع البيانات
    - _Requirements: 8.1, 8.5, 8.6_
  
  - [ ]* 4.3 كتابة اختبار خاصية لـ Token Storage Round Trip
    - **Property 2: Token Storage Round Trip**
    - **Validates: Requirements 1.2, 8.1, 8.5**
  
  - [ ]* 4.4 كتابة اختبار خاصية لعدم تخزين كلمات المرور
    - **Property 16: Password Never Stored**
    - **Validates: Requirements 8.4**

- [x] 5. Checkpoint - التحقق من الأساسيات
  - تأكد من أن جميع الاختبارات تعمل بنجاح
  - تأكد من أن ValidationHelper و SecureStorageService يعملان بشكل صحيح
  - اسأل المستخدم إذا كانت هناك أي أسئلة

- [ ] 6. تنفيذ NavigationService
  - [x] 6.1 إنشاء `INavigationService` و `NavigationService`
    - تنفيذ `NavigateToAsync(string route)`
    - تنفيذ `NavigateToAsync(string route, IDictionary<string, object> parameters)`
    - تنفيذ `GoBackAsync()`
    - استخدام `Shell.Current.GoToAsync()`
    - _Requirements: 1.3, 3.4, 7.1, 7.2, 7.3, 7.4_
  
  - [ ]* 6.2 كتابة اختبارات وحدة لـ NavigationService
    - اختبار الانتقال إلى route محدد
    - اختبار الانتقال مع parameters
    - _Requirements: 7.1, 7.2, 7.3, 7.4_

- [ ] 7. تنفيذ AuthService
  - [x] 7.1 إنشاء `RegisterResponseDTO` في مجلد Models (محلي)
    - خصائص: `Success`, `Message`, `UserId`
    - _Requirements: 3.4, 5.4_
  
  - [x] 7.2 إنشاء `IAuthService` و `AuthService`
    - تنفيذ constructor مع `HttpClient` و `ISecureStorageService`
    - تكوين HttpClient مع base URL و timeout و headers
    - _Requirements: 12.1, 12.2, 12.6_
  
  - [x] 7.3 تنفيذ `LoginAsync(LoginDTO)`
    - إرسال POST request إلى `/api/account/login`
    - Serialize LoginDTO إلى JSON
    - معالجة استجابة ناجحة (200): حفظ Token و UserInfo
    - معالجة 401: إرجاع رسالة "البريد الإلكتروني أو كلمة المرور غير صحيحة"
    - معالجة 500: إرجاع رسالة "حدث خطأ في الخادم"
    - معالجة HttpRequestException: إرجاع رسالة "تحقق من اتصالك بالإنترنت"
    - معالجة TaskCanceledException: إرجاع رسالة "انتهت مهلة الاتصال"
    - _Requirements: 1.1, 1.2, 1.4, 1.5, 1.6, 9.1, 9.2, 12.3_
  
  - [x] 7.4 تنفيذ `RegisterCustomerAsync(CustomerRegisterDTO)`
    - إرسال POST request إلى `/api/customer/register`
    - Serialize CustomerRegisterDTO إلى JSON
    - معالجة استجابة ناجحة (200/201)
    - معالجة 400: parse أخطاء محددة (Email exists, Phone exists)
    - معالجة أخطاء الشبكة والمهلة
    - _Requirements: 3.1, 3.6, 3.7, 3.8, 12.4_
  
  - [x] 7.5 تنفيذ `RegisterHallOwnerAsync(HallOwnerRegisterDTO)`
    - مشابه لـ RegisterCustomerAsync مع endpoint `/api/hallowner/register`
    - _Requirements: 5.1, 5.6, 5.7, 5.8_
  
  - [ ]* 7.6 كتابة اختبارات وحدة لـ AuthService
    - اختبار LoginAsync مع استجابة ناجحة
    - اختبار LoginAsync مع 401
    - اختبار RegisterCustomerAsync مع استجابة ناجحة
    - استخدام Mock لـ HttpClient
    - _Requirements: 1.1, 1.4, 3.1_
  
  - [ ]* 7.7 كتابة اختبار خاصية لـ Login Service Request Format
    - **Property 1: Login Service Request Format**
    - **Validates: Requirements 1.1**
  
  - [ ]* 7.8 كتابة اختبار خاصية لـ DTO Serialization Round Trip
    - **Property 21: DTO Serialization Round Trip**
    - **Validates: Requirements 12.3, 12.4, 12.5**

- [x] 8. Checkpoint - التحقق من Services
  - تأكد من أن جميع Services تعمل بشكل صحيح
  - تأكد من أن الاختبارات تمر بنجاح
  - اسأل المستخدم إذا كانت هناك أي أسئلة

- [x] 9. تنفيذ BaseViewModel
  - إنشاء `BaseViewModel.cs` في مجلد ViewModels
  - تنفيذ `INotifyPropertyChanged` interface
  - إضافة خصائص: `IsBusy`, `IsLoading`, `ErrorMessage`
  - تنفيذ `OnPropertyChanged()` method
  - تنفيذ `SetProperty<T>()` helper method
  - _Requirements: 13.1, 13.4_

- [ ]* 9.1 كتابة اختبار خاصية لـ Property Change Notification
  - **Property 23: Property Change Notification**
  - **Validates: Requirements 13.4, 13.5, 13.6**

- [ ] 10. تنفيذ LoginViewModel
  - [x] 10.1 إنشاء `LoginViewModel.cs` يرث من BaseViewModel
    - إضافة properties: `EmailOrPhone`, `Password`, `IsPasswordVisible`
    - إضافة commands: `LoginCommand`, `NavigateToRegisterCommand`, `TogglePasswordVisibilityCommand`
    - تنفيذ constructor مع DI لـ IAuthService, INavigationService, ValidationHelper
    - _Requirements: 1.1, 2.1, 2.2, 7.1_
  
  - [x] 10.2 تنفيذ `LoginAsync()` method
    - التحقق من الحقول الفارغة
    - إنشاء LoginDTO
    - استدعاء AuthService.LoginAsync()
    - معالجة النجاح: الانتقال إلى MainPage
    - معالجة الفشل: عرض رسالة الخطأ
    - إدارة IsLoading و IsBusy
    - _Requirements: 1.1, 1.3, 1.7, 1.8, 2.1, 2.2_
  
  - [x] 10.3 تنفيذ `CanLogin()` method
    - التحقق من أن الحقول ليست فارغة
    - التحقق من أن IsBusy = false
    - _Requirements: 2.5_
  
  - [x] 10.4 تنفيذ `NavigateToRegisterAsync()` method
    - عرض ActionSheet لاختيار نوع التسجيل
    - الانتقال إلى CustomerRegisterPage أو HallOwnerRegisterPage
    - _Requirements: 7.1, 7.2, 7.3_
  
  - [ ]* 10.5 كتابة اختبارات وحدة لـ LoginViewModel
    - اختبار CanExecute للـ LoginCommand
    - اختبار LoginAsync مع بيانات صالحة
    - اختبار LoginAsync مع بيانات غير صالحة
    - اختبار PropertyChanged events
    - استخدام Mock لـ IAuthService و INavigationService
    - _Requirements: 1.1, 2.5, 13.4_
  
  - [ ]* 10.6 كتابة اختبار خاصية لـ Loading Indicator Lifecycle
    - **Property 4: Loading Indicator Lifecycle**
    - **Validates: Requirements 1.7, 1.8**
  
  - [ ]* 10.7 كتابة اختبار خاصية لـ Submit Button State
    - **Property 7: Submit Button State**
    - **Validates: Requirements 2.5**

- [ ] 11. تنفيذ CustomerRegisterViewModel
  - [x] 11.1 إنشاء `CustomerRegisterViewModel.cs` يرث من BaseViewModel
    - إضافة properties لجميع الحقول: Email, Password, ConfirmPassword, FirstName, LastName, PhoneNumber, DateOfBirth, Address, Gender, Country, City, Nationality
    - إضافة properties للأخطاء: EmailError, PasswordError, ConfirmPasswordError, FirstNameError, LastNameError, PhoneNumberError, DateOfBirthError
    - إضافة properties: IsPasswordVisible, IsConfirmPasswordVisible
    - إضافة GenderOptions list
    - إضافة commands: RegisterCommand, NavigateToLoginCommand, TogglePasswordVisibilityCommand, ToggleConfirmPasswordVisibilityCommand
    - _Requirements: 3.1, 4.1-4.12_
  
  - [x] 11.2 تنفيذ validation methods
    - `ValidateEmail()`: التحقق من تنسيق البريد الإلكتروني
    - `ValidatePassword()`: التحقق من طول كلمة المرور (6+ أحرف)
    - `ValidateConfirmPassword()`: التحقق من تطابق كلمات المرور
    - `ValidateFirstName()`: التحقق من طول الاسم (2-50 حرف)
    - `ValidateLastName()`: التحقق من طول اسم العائلة (2-50 حرف)
    - `ValidatePhoneNumber()`: التحقق من تنسيق رقم الهاتف
    - `ValidateDateOfBirth()`: التحقق من العمر (18+ سنة)
    - _Requirements: 4.1-4.11_
  
  - [x] 11.3 تنفيذ `RegisterAsync()` method
    - التحقق من جميع الحقول
    - إنشاء CustomerRegisterDTO (مع معالجة الحقول الاختيارية)
    - استدعاء AuthService.RegisterCustomerAsync()
    - معالجة النجاح: عرض رسالة نجاح والانتقال إلى LoginPage
    - معالجة الفشل: عرض رسالة الخطأ
    - إدارة IsLoading و IsBusy
    - _Requirements: 3.1, 3.4, 3.5, 14.1-14.10_
  
  - [x] 11.4 تنفيذ `CanRegister()` method
    - التحقق من أن جميع الحقول المطلوبة مملوءة
    - التحقق من عدم وجود أخطاء validation
    - التحقق من أن IsBusy = false
    - _Requirements: 4.12_
  
  - [ ]* 11.5 كتابة اختبارات وحدة لـ CustomerRegisterViewModel
    - اختبار validation methods
    - اختبار CanExecute للـ RegisterCommand
    - اختبار RegisterAsync مع بيانات صالحة
    - اختبار معالجة الحقول الاختيارية
    - _Requirements: 4.1-4.12, 14.1-14.10_
  
  - [ ]* 11.6 كتابة اختبار خاصية للتحقق من تطابق كلمات المرور
    - **Property 11: Password Confirmation Matching**
    - **Validates: Requirements 4.5, 4.6**
  
  - [ ]* 11.7 كتابة اختبار خاصية للتحقق من طول الأسماء
    - **Property 12: Name Length Validation**
    - **Validates: Requirements 4.7, 4.8**
  
  - [ ]* 11.8 كتابة اختبار خاصية لمعالجة الحقول الاختيارية
    - **Property 25: Optional Fields Handling**
    - **Validates: Requirements 14.1-14.12**

- [ ] 12. تنفيذ HallOwnerRegisterViewModel
  - [x] 12.1 إنشاء `HallOwnerRegisterViewModel.cs` مشابه لـ CustomerRegisterViewModel
    - نفس البنية مع اختلافات: Gender مطلوب، BirthDate بدلاً من DateOfBirth
    - إضافة GenderError property
    - _Requirements: 5.1, 6.1-6.14_
  
  - [x] 12.2 تنفيذ validation methods (مشابه للعميل مع إضافة ValidateGender)
    - `ValidateGender()`: التحقق من أن Gender ليس فارغاً
    - باقي validation methods مشابهة لـ CustomerRegisterViewModel
    - _Requirements: 6.1-6.11_
  
  - [x] 12.3 تنفيذ `RegisterAsync()` method
    - مشابه لـ CustomerRegisterViewModel مع استخدام HallOwnerRegisterDTO
    - استدعاء AuthService.RegisterHallOwnerAsync()
    - _Requirements: 5.1, 5.4, 5.5_
  
  - [ ]* 12.4 كتابة اختبارات وحدة لـ HallOwnerRegisterViewModel
    - اختبار validation methods بما في ذلك Gender
    - اختبار RegisterAsync
    - _Requirements: 6.1-6.14_

- [x] 13. Checkpoint - التحقق من ViewModels
  - تأكد من أن جميع ViewModels تعمل بشكل صحيح
  - تأكد من أن جميع الاختبارات تمر بنجاح
  - اسأل المستخدم إذا كانت هناك أي أسئلة

- [x] 14. إنشاء Value Converters
  - إنشاء مجلد `Converters`
  - إنشاء `InvertedBoolConverter.cs` لعكس قيمة boolean
  - إنشاء `StringNotEmptyConverter.cs` للتحقق من أن string ليس فارغاً
  - إنشاء `PasswordVisibilityIconConverter.cs` لتبديل أيقونة إظهار/إخفاء كلمة المرور
  - _Requirements: 11.1_

- [x] 15. إنشاء Resources (Colors & Styles)
  - إنشاء `Resources/Styles/Colors.xaml`
  - تعريف الألوان: PrimaryColor, PrimaryTextColor, SecondaryTextColor, PlaceholderColor, PageBackgroundColor, InputBackgroundColor, BorderColor, ErrorColor, SuccessColor
  - _Requirements: 10.1, 10.2, 10.3_

- [ ] 16. تنفيذ LoginPage
  - [x] 16.1 إنشاء `Views/LoginPage.xaml` و `LoginPage.xaml.cs`
    - تعيين FlowDirection="RightToLeft"
    - إضافة Logo
    - إضافة Entry للبريد الإلكتروني/رقم الهاتف
    - إضافة Entry لكلمة المرور مع زر إظهار/إخفاء
    - إضافة Label لرسالة الخطأ
    - إضافة Button لتسجيل الدخول
    - إضافة ActivityIndicator للتحميل
    - إضافة Link للتسجيل
    - Binding إلى LoginViewModel
    - _Requirements: 1.1, 1.7, 2.1, 2.2, 7.1, 10.1, 10.2_
  
  - [x] 16.2 تكوين Code-behind
    - تعيين BindingContext إلى LoginViewModel (من DI)
    - _Requirements: 13.1_

- [ ] 17. تنفيذ CustomerRegisterPage
  - [x] 17.1 إنشاء `Views/CustomerRegisterPage.xaml` و `CustomerRegisterPage.xaml.cs`
    - استخدام ScrollView لاستيعاب جميع الحقول
    - تعيين FlowDirection="RightToLeft"
    - إضافة Entry لكل حقل مطلوب: Email, Password, ConfirmPassword, FirstName, LastName, PhoneNumber
    - إضافة DatePicker لتاريخ الميلاد
    - إضافة Picker للجنس (اختياري)
    - إضافة Entry للحقول الاختيارية: Address, City, Country, Nationality
    - إضافة Label لكل خطأ validation تحت الحقل المقابل
    - إضافة Button للتسجيل
    - إضافة ActivityIndicator للتحميل
    - إضافة Link لتسجيل الدخول
    - Binding إلى CustomerRegisterViewModel
    - _Requirements: 3.1, 4.1-4.12, 10.1, 10.2, 11.1_
  
  - [x] 17.2 تكوين Code-behind
    - تعيين BindingContext إلى CustomerRegisterViewModel (من DI)
    - _Requirements: 13.2_

- [ ] 18. تنفيذ HallOwnerRegisterPage
  - [x] 18.1 إنشاء `Views/HallOwnerRegisterPage.xaml` و `HallOwnerRegisterPage.xaml.cs`
    - مشابه لـ CustomerRegisterPage مع اختلافات:
    - Gender مطلوب (ليس اختياري)
    - BirthDate بدلاً من DateOfBirth
    - Binding إلى HallOwnerRegisterViewModel
    - _Requirements: 5.1, 6.1-6.14, 10.1, 10.2, 11.1_
  
  - [x] 18.2 تكوين Code-behind
    - تعيين BindingContext إلى HallOwnerRegisterViewModel (من DI)
    - _Requirements: 13.3_

- [x] 19. تكوين Dependency Injection في MauiProgram.cs
  - تسجيل HttpClient مع configuration (BaseAddress, Timeout, Headers)
  - تسجيل Services: IAuthService, ISecureStorageService, INavigationService, ValidationHelper
  - تسجيل ViewModels: LoginViewModel, CustomerRegisterViewModel, HallOwnerRegisterViewModel
  - تسجيل Pages: LoginPage, CustomerRegisterPage, HallOwnerRegisterPage
  - _Requirements: 12.1, 12.2, 12.6_

- [ ] 20. تكوين Navigation في AppShell
  - [x] 20.1 تحديث `AppShell.xaml`
    - إضافة ShellContent لـ LoginPage كصفحة افتراضية
    - تعيين FlowDirection="RightToLeft"
    - _Requirements: 7.1, 10.1_
  
  - [x] 20.2 تحديث `AppShell.xaml.cs`
    - تسجيل Routes: LoginPage, CustomerRegisterPage, HallOwnerRegisterPage
    - _Requirements: 7.2, 7.3, 7.4_

- [x] 21. Checkpoint النهائي - اختبار التكامل
  - تشغيل التطبيق والتحقق من تسجيل الدخول
  - التحقق من التنقل بين الشاشات
  - التحقق من validation على جميع الحقول
  - التحقق من معالجة الأخطاء
  - التحقق من دعم RTL واللغة العربية
  - تأكد من أن جميع الاختبارات تمر بنجاح
  - اسأل المستخدم إذا كانت هناك أي أسئلة أو تعديلات مطلوبة

## ملاحظات

- المهام المميزة بـ `*` هي اختيارية ويمكن تخطيها للحصول على MVP أسرع
- كل مهمة تشير إلى المتطلبات التي تغطيها
- اختبارات الخصائص تستخدم FsCheck مع 100 تكرار كحد أدنى
- جميع رسائل الأخطاء يجب أن تكون باللغة العربية
- جميع الشاشات يجب أن تدعم RTL
