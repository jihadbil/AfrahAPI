# Requirements Document

## Introduction

هذا المستند يحدد متطلبات CustomerRegisterViewModel في تطبيق MAUI الخاص بنظام Afrah. يوفر هذا الـ ViewModel واجهة لتسجيل عميل جديد في النظام، مع التحقق من صحة البيانات المدخلة وإدارة حالة التسجيل والتفاعل مع API.

## Glossary

- **CustomerRegisterViewModel**: ViewModel مسؤول عن إدارة منطق تسجيل العميل الجديد في تطبيق MAUI
- **IAuthService**: خدمة المصادقة المسؤولة عن الاتصال بـ API لإجراء عمليات التسجيل
- **INavigationService**: خدمة التنقل المسؤولة عن الانتقال بين الصفحات
- **ValidationHelper**: مساعد التحقق من صحة البيانات المدخلة
- **CustomerRegisterDTO**: كائن نقل البيانات الذي يحتوي على معلومات تسجيل العميل
- **BaseViewModel**: الفئة الأساسية التي توفر خصائص مشتركة مثل IsBusy و IsLoading و ErrorMessage
- **Property_Binding**: ربط خصائص ViewModel مع واجهة المستخدم لتحديث تلقائي
- **Command**: أمر قابل للتنفيذ يربط بين إجراءات المستخدم والمنطق في ViewModel
- **Users_Table**: جدول قاعدة البيانات الذي يحتوي على بيانات المصادقة والمعلومات الأساسية لجميع المستخدمين في النظام
- **Customers_Table**: جدول قاعدة البيانات الذي يحتوي على البيانات الخاصة بالعملاء فقط
- **User_Customer_Relationship**: علاقة واحد لواحد بين جدول Users وجدول Customers حيث كل عميل يجب أن يكون له سجل مستخدم مرتبط

## Requirements

### Requirement 1: إدارة خصائص بيانات التسجيل

**User Story:** كمستخدم، أريد إدخال بياناتي الشخصية في نموذج التسجيل، حتى أتمكن من إنشاء حساب جديد في النظام

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL provide bindable properties for Email, Password, ConfirmPassword, FirstName, LastName, PhoneNumber, DateOfBirth, Address, Gender, Country, City, and Nationality
2. WHEN any property value changes, THE CustomerRegisterViewModel SHALL notify the UI through INotifyPropertyChanged
3. WHEN a property value changes, THE CustomerRegisterViewModel SHALL clear any existing error message for that field
4. WHEN a property value changes, THE CustomerRegisterViewModel SHALL update the CanExecute state of RegisterCommand
5. THE CustomerRegisterViewModel SHALL initialize DateOfBirth to a default date that makes the user 18 years old

### Requirement 2: التحقق من صحة البيانات المدخلة

**User Story:** كمستخدم، أريد أن يتم التحقق من صحة البيانات التي أدخلها، حتى أتجنب الأخطاء عند التسجيل

#### Acceptance Criteria

1. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that Email is not empty and has valid email format using ValidationHelper
2. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that Password is not empty and has minimum length of 6 characters using ValidationHelper
3. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that ConfirmPassword matches Password exactly
4. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that FirstName and LastName are not empty and have length between 2 and 50 characters using ValidationHelper
5. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that PhoneNumber is not empty and has valid phone format using ValidationHelper
6. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that DateOfBirth results in age of 18 years or more using ValidationHelper
7. WHEN RegisterCommand is invoked, THE CustomerRegisterViewModel SHALL validate that Gender is selected
8. IF any validation fails, THEN THE CustomerRegisterViewModel SHALL set ErrorMessage with appropriate Arabic error message from Constants and return without calling API

### Requirement 3: تنفيذ عملية التسجيل

**User Story:** كمستخدم، أريد إرسال بيانات التسجيل إلى الخادم، حتى يتم إنشاء حسابي في النظام

#### Acceptance Criteria

1. WHEN RegisterCommand is invoked and all validations pass, THE CustomerRegisterViewModel SHALL set IsBusy to true and IsLoading to true
2. WHEN RegisterCommand is invoked and all validations pass, THE CustomerRegisterViewModel SHALL create CustomerRegisterDTO with all property values
3. WHEN RegisterCommand is invoked and all validations pass, THE CustomerRegisterViewModel SHALL call IAuthService.RegisterCustomerAsync with the DTO
4. WHEN IAuthService.RegisterCustomerAsync is called, THE IAuthService SHALL create a record in Users_Table with authentication credentials (Email and Password)
5. WHEN IAuthService.RegisterCustomerAsync is called, THE IAuthService SHALL create a record in Customers_Table with customer-specific information (FirstName, LastName, PhoneNumber, DateOfBirth, Address, Gender, Country, City, Nationality)
6. WHEN IAuthService.RegisterCustomerAsync creates records, THE IAuthService SHALL establish User_Customer_Relationship by linking the Customers_Table record to the Users_Table record
7. WHEN IAuthService.RegisterCustomerAsync returns success, THE CustomerRegisterViewModel SHALL display success message using Application.Current.MainPage.DisplayAlert
8. WHEN IAuthService.RegisterCustomerAsync returns success, THE CustomerRegisterViewModel SHALL navigate to LoginRoute using INavigationService
9. WHEN IAuthService.RegisterCustomerAsync returns failure, THE CustomerRegisterViewModel SHALL set ErrorMessage with the error message from the result
10. WHEN RegisterCommand execution completes, THE CustomerRegisterViewModel SHALL set IsBusy to false and IsLoading to false in a finally block

### Requirement 4: إدارة حالة الأوامر

**User Story:** كمستخدم، أريد أن يتم تعطيل زر التسجيل عندما تكون البيانات غير مكتملة، حتى لا أحاول التسجيل بدون بيانات كافية

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL provide RegisterCommand that implements Command interface
2. THE CustomerRegisterViewModel SHALL enable RegisterCommand only when Email, Password, ConfirmPassword, FirstName, LastName, PhoneNumber, DateOfBirth, and Gender are not empty and IsBusy is false
3. WHEN any required property changes, THE CustomerRegisterViewModel SHALL call ChangeCanExecute on RegisterCommand
4. WHILE IsBusy is true, THE CustomerRegisterViewModel SHALL disable RegisterCommand

### Requirement 5: إدارة رؤية كلمة المرور

**User Story:** كمستخدم، أريد إظهار أو إخفاء كلمة المرور، حتى أتمكن من التحقق من صحة ما أدخلته

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL provide IsPasswordVisible property with default value of false
2. THE CustomerRegisterViewModel SHALL provide IsConfirmPasswordVisible property with default value of false
3. THE CustomerRegisterViewModel SHALL provide TogglePasswordVisibilityCommand that toggles IsPasswordVisible
4. THE CustomerRegisterViewModel SHALL provide ToggleConfirmPasswordVisibilityCommand that toggles IsConfirmPasswordVisible

### Requirement 6: التنقل إلى صفحة تسجيل الدخول

**User Story:** كمستخدم لديه حساب بالفعل، أريد الانتقال إلى صفحة تسجيل الدخول، حتى أتمكن من الدخول بدلاً من إنشاء حساب جديد

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL provide NavigateToLoginCommand
2. WHEN NavigateToLoginCommand is invoked, THE CustomerRegisterViewModel SHALL navigate to LoginRoute using INavigationService

### Requirement 7: حقن التبعيات والتهيئة

**User Story:** كمطور، أريد أن يتم حقن التبعيات بشكل صحيح، حتى يعمل ViewModel بشكل سليم

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL accept IAuthService, INavigationService, and ValidationHelper as constructor parameters
2. WHEN any constructor parameter is null, THE CustomerRegisterViewModel SHALL throw ArgumentNullException
3. WHEN CustomerRegisterViewModel is constructed, THE CustomerRegisterViewModel SHALL initialize all Command properties
4. THE CustomerRegisterViewModel SHALL inherit from BaseViewModel to get IsBusy, IsLoading, and ErrorMessage properties

### Requirement 8: إدارة قائمة خيارات الجنس

**User Story:** كمستخدم، أريد اختيار الجنس من قائمة محددة، حتى أتمكن من إدخال هذه البيانات بسهولة

#### Acceptance Criteria

1. THE CustomerRegisterViewModel SHALL provide GenderOptions property as ObservableCollection containing Constants.GenderMale and Constants.GenderFemale
2. THE CustomerRegisterViewModel SHALL initialize GenderOptions in the constructor
