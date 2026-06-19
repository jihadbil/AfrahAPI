# وثيقة المتطلبات - شاشات المصادقة والتسجيل

## المقدمة

هذه الوثيقة تحدد متطلبات شاشات المصادقة والتسجيل في تطبيق AfrahApp (.NET MAUI). التطبيق يحتاج إلى ثلاث شاشات رئيسية: شاشة تسجيل الدخول، شاشة تسجيل حساب عميل، وشاشة تسجيل حساب صاحب صالة. جميع الشاشات تتصل بـ API موجود مسبقاً (AfrahAPI) وتستخدم نمط MVVM المعماري.

## المصطلحات (Glossary)

- **Authentication_System**: نظام المصادقة الذي يدير تسجيل الدخول والتسجيل
- **Login_Service**: خدمة الاتصال بـ API لتسجيل الدخول
- **Registration_Service**: خدمة الاتصال بـ API لتسجيل المستخدمين الجدد
- **Validation_Engine**: محرك التحقق من صحة البيانات المدخلة
- **Secure_Storage_Service**: خدمة تخزين آمن للـ Token والبيانات الحساسة
- **Login_ViewModel**: ViewModel لشاشة تسجيل الدخول
- **Customer_Register_ViewModel**: ViewModel لشاشة تسجيل العميل
- **HallOwner_Register_ViewModel**: ViewModel لشاشة تسجيل صاحب الصالة
- **Navigation_Service**: خدمة التنقل بين الشاشات
- **API_Client**: عميل HTTP للاتصال بالـ API
- **JWT_Token**: رمز المصادقة المُرجع من API
- **User_Credentials**: بيانات اعتماد المستخدم (البريد الإلكتروني أو رقم الهاتف وكلمة المرور)
- **Registration_Data**: بيانات التسجيل الكاملة للمستخدم الجديد
- **Error_Handler**: معالج الأخطاء لعرض رسائل مفهومة
- **Loading_Indicator**: مؤشر التحميل المرئي

## المتطلبات

### Requirement 1: تسجيل الدخول

**User Story:** كمستخدم مسجل، أريد تسجيل الدخول إلى التطبيق، حتى أتمكن من الوصول إلى حسابي واستخدام الميزات المتاحة.

#### Acceptance Criteria

1. WHEN a user enters valid credentials and submits, THE Login_Service SHALL send a POST request to /api/account/login with LoginDTO
2. WHEN the API returns a successful response with JWT_Token, THE Secure_Storage_Service SHALL store the token securely
3. WHEN the JWT_Token is stored successfully, THE Navigation_Service SHALL navigate to the main screen
4. IF the API returns an authentication error (401), THEN THE Error_Handler SHALL display "البريد الإلكتروني أو كلمة المرور غير صحيحة"
5. IF the API returns a server error (500), THEN THE Error_Handler SHALL display "حدث خطأ في الخادم، يرجى المحاولة لاحقاً"
6. IF a network error occurs, THEN THE Error_Handler SHALL display "تحقق من اتصالك بالإنترنت"
7. WHILE the login request is in progress, THE Login_ViewModel SHALL display the Loading_Indicator
8. WHEN the login request completes (success or failure), THE Login_ViewModel SHALL hide the Loading_Indicator

### Requirement 2: التحقق من صحة بيانات تسجيل الدخول

**User Story:** كمستخدم، أريد أن يتحقق التطبيق من صحة البيانات المدخلة قبل الإرسال، حتى أتجنب الأخطاء الواضحة وأحصل على تجربة أفضل.

#### Acceptance Criteria

1. WHEN a user attempts to submit with empty email or phone number field, THE Validation_Engine SHALL prevent submission and display "هذا الحقل مطلوب"
2. WHEN a user attempts to submit with empty password field, THE Validation_Engine SHALL prevent submission and display "كلمة المرور مطلوبة"
3. WHEN a user enters an email format in the email field, THE Validation_Engine SHALL validate it matches email pattern
4. IF the email format is invalid, THEN THE Validation_Engine SHALL display "البريد الإلكتروني غير صحيح"
5. THE Login_ViewModel SHALL enable the submit button only when all validation rules pass

### Requirement 3: تسجيل حساب عميل جديد

**User Story:** كعميل جديد، أريد تسجيل حساب في التطبيق، حتى أتمكن من حجز الصالات واستخدام خدمات التطبيق.

#### Acceptance Criteria

1. WHEN a user completes all required fields and submits, THE Registration_Service SHALL send a POST request to /api/customer/register with CustomerRegisterDTO
2. WHEN the API receives the registration request, THE API SHALL create a user Identity account with Customer role
3. WHEN the Identity account is created successfully, THE API SHALL create a customer record in the Customers table linked to the user account
4. WHEN the API returns successful registration (200/201), THE Navigation_Service SHALL navigate to the login screen
5. WHEN navigation to login screen occurs, THE Authentication_System SHALL display a success message "تم التسجيل بنجاح، يمكنك تسجيل الدخول الآن"
6. IF the API returns a validation error (400), THEN THE Error_Handler SHALL parse and display specific field errors
7. IF the API returns "Email already exists" error, THEN THE Error_Handler SHALL display "البريد الإلكتروني مسجل مسبقاً"
8. IF the API returns "Phone number already exists" error, THEN THE Error_Handler SHALL display "رقم الهاتف مسجل مسبقاً"
9. WHILE the registration request is in progress, THE Customer_Register_ViewModel SHALL display the Loading_Indicator

### Requirement 4: التحقق من صحة بيانات تسجيل العميل

**User Story:** كعميل جديد، أريد أن يتحقق التطبيق من صحة بياناتي، حتى أتأكد من إدخال معلومات صحيحة ومقبولة.

#### Acceptance Criteria

1. WHEN a user enters an email, THE Validation_Engine SHALL validate it matches email pattern (contains @ and domain)
2. IF the email format is invalid, THEN THE Validation_Engine SHALL display "البريد الإلكتروني غير صحيح"
3. WHEN a user enters a password, THE Validation_Engine SHALL validate it has minimum 6 characters
4. IF the password is less than 6 characters, THEN THE Validation_Engine SHALL display "كلمة المرور يجب أن تكون 6 أحرف على الأقل"
5. WHEN a user enters password confirmation, THE Validation_Engine SHALL validate it matches the password field
6. IF passwords do not match, THEN THE Validation_Engine SHALL display "كلمات المرور غير متطابقة"
7. WHEN a user enters first name, THE Validation_Engine SHALL validate it has 2-50 characters
8. WHEN a user enters last name, THE Validation_Engine SHALL validate it has 2-50 characters
9. WHEN a user enters phone number, THE Validation_Engine SHALL validate it matches phone pattern
10. WHEN a user selects date of birth, THE Validation_Engine SHALL calculate age and validate it is 18 years or older
11. IF the user is under 18 years old, THEN THE Validation_Engine SHALL display "يجب أن يكون عمرك 18 سنة أو أكثر"
12. THE Customer_Register_ViewModel SHALL enable the submit button only when all required fields are valid

### Requirement 5: تسجيل حساب صاحب صالة جديد

**User Story:** كصاحب صالة جديد، أريد تسجيل حساب في التطبيق، حتى أتمكن من إدارة صالاتي وتلقي الحجوزات.

#### Acceptance Criteria

1. WHEN a user completes all required fields and submits, THE Registration_Service SHALL send a POST request to /api/hallowner/register with HallOwnerRegisterDTO
2. WHEN the API receives the registration request, THE API SHALL create a user Identity account with HallOwner role
3. WHEN the Identity account is created successfully, THE API SHALL create a hall owner record in the HallOwners table linked to the user account
4. WHEN the API returns successful registration (200/201), THE Navigation_Service SHALL navigate to the login screen
5. WHEN navigation to login screen occurs, THE Authentication_System SHALL display a success message "تم التسجيل بنجاح، يمكنك تسجيل الدخول الآن"
6. IF the API returns a validation error (400), THEN THE Error_Handler SHALL parse and display specific field errors
7. IF the API returns "Email already exists" error, THEN THE Error_Handler SHALL display "البريد الإلكتروني مسجل مسبقاً"
8. IF the API returns "Phone number already exists" error, THEN THE Error_Handler SHALL display "رقم الهاتف مسجل مسبقاً"
9. WHILE the registration request is in progress, THE HallOwner_Register_ViewModel SHALL display the Loading_Indicator

### Requirement 6: التحقق من صحة بيانات تسجيل صاحب الصالة

**User Story:** كصاحب صالة جديد، أريد أن يتحقق التطبيق من صحة بياناتي، حتى أتأكد من إدخال معلومات صحيحة ومقبولة.

#### Acceptance Criteria

1. WHEN a user enters an email, THE Validation_Engine SHALL validate it matches email pattern (contains @ and domain)
2. IF the email format is invalid, THEN THE Validation_Engine SHALL display "البريد الإلكتروني غير صحيح"
3. WHEN a user enters a password, THE Validation_Engine SHALL validate it has minimum 6 characters
4. IF the password is less than 6 characters, THEN THE Validation_Engine SHALL display "كلمة المرور يجب أن تكون 6 أحرف على الأقل"
5. WHEN a user enters password confirmation, THE Validation_Engine SHALL validate it matches the password field
6. IF passwords do not match, THEN THE Validation_Engine SHALL display "كلمات المرور غير متطابقة"
7. WHEN a user enters first name, THE Validation_Engine SHALL validate it has 2-50 characters
8. WHEN a user enters last name, THE Validation_Engine SHALL validate it has 2-50 characters
9. WHEN a user enters phone number, THE Validation_Engine SHALL validate it matches phone pattern
10. WHEN a user selects gender, THE Validation_Engine SHALL validate a value is selected (required field)
11. IF gender is not selected, THEN THE Validation_Engine SHALL display "الجنس مطلوب"
12. WHEN a user selects date of birth, THE Validation_Engine SHALL calculate age and validate it is 18 years or older
13. IF the user is under 18 years old, THEN THE Validation_Engine SHALL display "يجب أن يكون عمرك 18 سنة أو أكثر"
14. THE HallOwner_Register_ViewModel SHALL enable the submit button only when all required fields are valid

### Requirement 7: التنقل بين الشاشات

**User Story:** كمستخدم، أريد التنقل بسهولة بين شاشات المصادقة، حتى أتمكن من الوصول إلى الشاشة المناسبة لحالتي.

#### Acceptance Criteria

1. WHEN a user is on the login screen and clicks "ليس لديك حساب؟ سجل الآن", THE Navigation_Service SHALL display options to choose between customer or hall owner registration
2. WHEN a user selects customer registration, THE Navigation_Service SHALL navigate to CustomerRegisterPage
3. WHEN a user selects hall owner registration, THE Navigation_Service SHALL navigate to HallOwnerRegisterPage
4. WHEN a user is on any registration screen and clicks "لديك حساب؟ سجل الدخول", THE Navigation_Service SHALL navigate to LoginPage
5. WHEN navigation occurs, THE Navigation_Service SHALL use smooth transitions

### Requirement 8: تخزين آمن للبيانات الحساسة

**User Story:** كمستخدم، أريد أن يتم تخزين بيانات المصادقة الخاصة بي بشكل آمن، حتى أحافظ على خصوصية حسابي.

#### Acceptance Criteria

1. WHEN a JWT_Token is received from the API, THE Secure_Storage_Service SHALL store it using platform-specific secure storage (Keychain on iOS, KeyStore on Android)
2. WHEN a refresh token is received, THE Secure_Storage_Service SHALL store it securely alongside the JWT_Token
3. WHEN user info is received, THE Secure_Storage_Service SHALL store it securely
4. THE Secure_Storage_Service SHALL NOT store passwords in any form
5. WHEN the app needs to retrieve the JWT_Token, THE Secure_Storage_Service SHALL return it from secure storage
6. WHEN a user logs out, THE Secure_Storage_Service SHALL remove all stored authentication data

### Requirement 9: معالجة الأخطاء والاستثناءات

**User Story:** كمستخدم، أريد أن أرى رسائل خطأ واضحة ومفهومة عند حدوث مشاكل، حتى أعرف كيفية حل المشكلة.

#### Acceptance Criteria

1. WHEN a network timeout occurs (>30 seconds), THE Error_Handler SHALL display "انتهت مهلة الاتصال، يرجى المحاولة مرة أخرى"
2. WHEN the device has no internet connection, THE Error_Handler SHALL display "لا يوجد اتصال بالإنترنت"
3. WHEN the API returns 400 with validation errors, THE Error_Handler SHALL parse the error response and display field-specific errors
4. WHEN the API returns 500, THE Error_Handler SHALL display "حدث خطأ في الخادم، يرجى المحاولة لاحقاً"
5. WHEN an unexpected exception occurs, THE Error_Handler SHALL log the error and display "حدث خطأ غير متوقع"
6. THE Error_Handler SHALL display errors in Arabic language
7. THE Error_Handler SHALL clear previous error messages when a new request starts

### Requirement 10: دعم اللغة العربية والتخطيط RTL

**User Story:** كمستخدم عربي، أريد أن يدعم التطبيق اللغة العربية والتخطيط من اليمين لليسار، حتى أحصل على تجربة استخدام مريحة.

#### Acceptance Criteria

1. THE Authentication_System SHALL display all UI text in Arabic language
2. THE Authentication_System SHALL use RTL (Right-to-Left) layout for all screens
3. WHEN text is displayed, THE Authentication_System SHALL use Arabic-friendly fonts
4. WHEN input fields are displayed, THE Authentication_System SHALL align text to the right
5. WHEN buttons are displayed, THE Authentication_System SHALL position them according to RTL conventions
6. THE Authentication_System SHALL display validation messages in Arabic

### Requirement 11: تجربة المستخدم والاستجابة

**User Story:** كمستخدم، أريد أن يكون التطبيق سريع الاستجابة ويوفر تجربة سلسة، حتى أستمتع باستخدامه.

#### Acceptance Criteria

1. WHEN a user interacts with any input field, THE Authentication_System SHALL provide immediate visual feedback
2. WHEN a user submits a form, THE Authentication_System SHALL disable the submit button to prevent double submission
3. WHEN a loading operation is in progress, THE Loading_Indicator SHALL be visible and animated
4. WHEN a loading operation completes, THE Loading_Indicator SHALL be hidden within 100ms
5. WHEN validation errors occur, THE Authentication_System SHALL display them immediately below the relevant field
6. WHEN a user corrects an invalid field, THE Authentication_System SHALL clear the error message for that field
7. THE Authentication_System SHALL support scrolling on registration screens to accommodate all fields on small screens
8. WHEN the keyboard appears, THE Authentication_System SHALL scroll to keep the active input field visible

### Requirement 12: الأمان والاتصال بالـ API

**User Story:** كمستخدم، أريد أن تكون اتصالاتي مع الخادم آمنة، حتى أحمي بياناتي الشخصية.

#### Acceptance Criteria

1. THE API_Client SHALL use HTTPS protocol exclusively for all API requests
2. THE API_Client SHALL include proper headers (Content-Type: application/json, Accept: application/json)
3. WHEN sending login request, THE API_Client SHALL serialize User_Credentials to JSON format matching LoginDTO
4. WHEN sending registration request, THE API_Client SHALL serialize Registration_Data to JSON format matching CustomerRegisterDTO or HallOwnerRegisterDTO
5. WHEN receiving API response, THE API_Client SHALL deserialize JSON to appropriate response models
6. THE API_Client SHALL set request timeout to 30 seconds
7. THE API_Client SHALL validate SSL certificates
8. THE API_Client SHALL NOT log sensitive data (passwords, tokens) in debug or production builds

### Requirement 13: إدارة الحالة في ViewModels

**User Story:** كمطور، أريد أن تدير ViewModels الحالة بشكل صحيح، حتى تعكس الواجهة التغييرات بدقة.

#### Acceptance Criteria

1. THE Login_ViewModel SHALL implement INotifyPropertyChanged interface
2. THE Customer_Register_ViewModel SHALL implement INotifyPropertyChanged interface
3. THE HallOwner_Register_ViewModel SHALL implement INotifyPropertyChanged interface
4. WHEN a property value changes in any ViewModel, THE ViewModel SHALL raise PropertyChanged event
5. WHEN IsLoading property changes, THE ViewModel SHALL notify the view to update Loading_Indicator visibility
6. WHEN ErrorMessage property changes, THE ViewModel SHALL notify the view to display the error
7. WHEN validation state changes, THE ViewModel SHALL update CanExecute state of submit command
8. THE ViewModels SHALL use Command pattern (ICommand) for user actions

### Requirement 14: حقول اختيارية في التسجيل

**User Story:** كمستخدم جديد، أريد أن أتمكن من تخطي الحقول الاختيارية، حتى أسرع عملية التسجيل.

#### Acceptance Criteria

1. WHERE the Address field is provided, THE Registration_Service SHALL include it in the registration request
2. WHERE the Address field is empty, THE Registration_Service SHALL send null or omit it from the request
3. WHERE the City field is provided, THE Registration_Service SHALL include it in the registration request
4. WHERE the City field is empty, THE Registration_Service SHALL send null or omit it from the request
5. WHERE the Country field is provided, THE Registration_Service SHALL include it in the registration request
6. WHERE the Country field is empty, THE Registration_Service SHALL send null or omit it from the request
7. WHERE the Nationality field is provided, THE Registration_Service SHALL include it in the registration request
8. WHERE the Nationality field is empty, THE Registration_Service SHALL send null or omit it from the request
9. WHERE the Gender field is provided in customer registration, THE Registration_Service SHALL include it in the request
10. WHERE the Gender field is empty in customer registration, THE Registration_Service SHALL send null or omit it from the request
11. THE Validation_Engine SHALL NOT validate optional fields when they are empty
12. THE Validation_Engine SHALL validate optional fields when they contain data

### Requirement 15: التوافق مع المنصات المختلفة

**User Story:** كمستخدم، أريد أن يعمل التطبيق بشكل صحيح على جهازي، بغض النظر عن نظام التشغيل.

#### Acceptance Criteria

1. THE Authentication_System SHALL function correctly on Android 21 (Lollipop) and above
2. THE Authentication_System SHALL function correctly on iOS 15 and above
3. THE Authentication_System SHALL function correctly on Windows 10 and above
4. THE Authentication_System SHALL function correctly on MacCatalyst
5. WHEN running on different platforms, THE Authentication_System SHALL use platform-specific secure storage implementations
6. WHEN running on different platforms, THE Authentication_System SHALL adapt UI elements to platform conventions while maintaining consistent functionality
7. THE Authentication_System SHALL handle platform-specific keyboard behaviors appropriately
