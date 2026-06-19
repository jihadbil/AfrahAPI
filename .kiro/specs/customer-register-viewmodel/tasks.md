# Implementation Plan: CustomerRegisterViewModel

## Overview

تنفيذ CustomerRegisterViewModel لإدارة منطق تسجيل العميل الجديد في تطبيق Afrah MAUI. يتضمن التنفيذ إنشاء ViewModel مع جميع الخصائص القابلة للربط، التحقق من صحة البيانات، التفاعل مع IAuthService، وإدارة حالة UI والتنقل.

## Tasks

- [x] 1. إعداد البنية الأساسية لـ CustomerRegisterViewModel
  - إنشاء ملف CustomerRegisterViewModel.cs في المجلد المناسب
  - إضافة الوراثة من BaseViewModel
  - تعريف جميع الخصائص القابلة للربط (Email, Password, ConfirmPassword, FirstName, LastName, PhoneNumber, DateOfBirth, Address, Gender, Country, City, Nationality)
  - تعريف خصائص حالة UI (IsPasswordVisible, IsConfirmPasswordVisible, GenderOptions)
  - تعريف جميع الأوامر (RegisterCommand, NavigateToLoginCommand, TogglePasswordVisibilityCommand, ToggleConfirmPasswordVisibilityCommand)
  - _Requirements: 1.1, 5.1, 5.2, 8.1_

- [ ] 2. تنفيذ Constructor والتهيئة الأولية
  - [x] 2.1 إنشاء Constructor مع حقن التبعيات
    - قبول IAuthService, INavigationService, ValidationHelper كمعاملات
    - التحقق من أن جميع المعاملات ليست null ورمي ArgumentNullException عند الحاجة
    - تهيئة جميع الأوامر
    - تهيئة GenderOptions بقيم من Constants (GenderMale و GenderFemale)
    - تعيين DateOfBirth إلى تاريخ يجعل المستخدم 18 سنة
    - _Requirements: 7.1, 7.2, 7.3, 1.5, 8.2_
  
  - [ ]* 2.2 كتابة unit tests للـ Constructor
    - اختبار رمي ArgumentNullException عند null parameters
    - اختبار تهيئة GenderOptions بشكل صحيح
    - اختبار DateOfBirth الافتراضي (18 سنة)
    - اختبار تهيئة Commands بشكل صحيح
    - _Requirements: 7.2, 1.5, 8.2_

- [ ] 3. تنفيذ Property Setters مع INotifyPropertyChanged
  - [x] 3.1 إضافة PropertyChanged notification لجميع الخصائص
    - تنفيذ property setters لجميع الخصائص القابلة للربط
    - استدعاء OnPropertyChanged عند تغيير أي خاصية
    - استدعاء ClearFieldError() عند تغيير أي خاصية
    - استدعاء RegisterCommand.ChangeCanExecute() عند تغيير الخصائص المطلوبة
    - _Requirements: 1.2, 1.3, 1.4_
  
  - [ ]* 3.2 كتابة property test للـ Property Change Notification
    - **Property 1: Property Change Notification**
    - **Validates: Requirements 1.2**
  
  - [ ]* 3.3 كتابة property test لمسح ErrorMessage عند تغيير الخاصية
    - **Property 2: Error Message Clearing on Property Change**
    - **Validates: Requirements 1.3**
  
  - [ ]* 3.4 كتابة property test لتحديث CanExecute
    - **Property 3: CanExecute Update on Property Change**
    - **Validates: Requirements 1.4**

- [ ] 4. تنفيذ منطق التحقق من صحة البيانات
  - [x] 4.1 إنشاء ValidateInput() method
    - التحقق من Email (not empty و valid format)
    - التحقق من Password (not empty و minimum 6 characters)
    - التحقق من تطابق Password و ConfirmPassword
    - التحقق من FirstName و LastName (not empty و length 2-50)
    - التحقق من PhoneNumber (not empty و valid format)
    - التحقق من DateOfBirth (age >= 18)
    - التحقق من Gender (not empty)
    - تعيين ErrorMessage المناسب من Constants عند فشل أي تحقق
    - إرجاع true إذا نجحت جميع التحققات
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8_
  
  - [ ]* 4.2 كتابة unit tests للتحقق من صحة البيانات
    - اختبار كل قاعدة تحقق بشكل منفصل
    - اختبار حالات حدية (empty, null, invalid format)
    - اختبار رسائل الخطأ الصحيحة
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8_
  
  - [ ]* 4.3 كتابة property test للتحقق الشامل من المدخلات
    - **Property 4: Input Validation Completeness**
    - **Validates: Requirements 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8**

- [x] 5. Checkpoint - التحقق من البنية الأساسية
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 6. تنفيذ RegisterCommand
  - [x] 6.1 إنشاء CanRegister() method
    - التحقق من أن جميع الحقول المطلوبة ليست فارغة
    - التحقق من أن IsBusy = false
    - _Requirements: 4.2, 4.4_
  
  - [x] 6.2 إنشاء RegisterAsync() method
    - التحقق من IsBusy لتجنب التنفيذ المتعدد
    - تعيين IsBusy و IsLoading إلى true
    - مسح ErrorMessage
    - استدعاء ValidateInput() والعودة إذا فشل
    - إنشاء CustomerRegisterDTO من خصائص ViewModel
    - استدعاء IAuthService.RegisterCustomerAsync(dto)
    - عند النجاح: عرض رسالة نجاح والانتقال إلى LoginRoute
    - عند الفشل: تعيين ErrorMessage من result
    - في finally block: تعيين IsBusy و IsLoading إلى false
    - معالجة الاستثناءات (HttpRequestException, TaskCanceledException, Exception)
    - _Requirements: 3.1, 3.2, 3.3, 3.7, 3.8, 3.9, 3.10_
  
  - [x] 6.3 تهيئة RegisterCommand في Constructor
    - إنشاء Command مع RegisterAsync و CanRegister
    - _Requirements: 4.1_
  
  - [ ]* 6.4 كتابة unit tests لـ RegisterCommand
    - اختبار RegisterCommand مع بيانات صحيحة
    - اختبار RegisterCommand مع بيانات غير صحيحة
    - اختبار معالجة الأخطاء من API
    - اختبار IsBusy state management
    - _Requirements: 3.1, 3.2, 3.3, 3.7, 3.8, 3.9, 3.10_
  
  - [ ]* 6.5 كتابة property test لتدفق التسجيل الناجح
    - **Property 5: Successful Registration Flow**
    - **Validates: Requirements 3.1, 3.2, 3.3, 3.7, 3.8**
  
  - [ ]* 6.6 كتابة property test لمعالجة أخطاء التسجيل
    - **Property 6: Failed Registration Error Handling**
    - **Validates: Requirements 3.9**
  
  - [ ]* 6.7 كتابة property test لتنظيف IsBusy state
    - **Property 7: IsBusy State Cleanup**
    - **Validates: Requirements 3.10**
  
  - [ ]* 6.8 كتابة property test لمنطق CanExecute
    - **Property 8: RegisterCommand CanExecute Logic**
    - **Validates: Requirements 4.2, 4.3, 4.4**

- [ ] 7. تنفيذ أوامر رؤية كلمة المرور
  - [x] 7.1 تنفيذ TogglePasswordVisibilityCommand
    - تبديل قيمة IsPasswordVisible
    - _Requirements: 5.3_
  
  - [x] 7.2 تنفيذ ToggleConfirmPasswordVisibilityCommand
    - تبديل قيمة IsConfirmPasswordVisible
    - _Requirements: 5.4_
  
  - [ ]* 7.3 كتابة unit tests لأوامر Toggle
    - اختبار TogglePasswordVisibilityCommand
    - اختبار ToggleConfirmPasswordVisibilityCommand
    - _Requirements: 5.3, 5.4_
  
  - [ ]* 7.4 كتابة property test لـ Toggle Idempotence
    - **Property 9: Password Visibility Toggle Idempotence**
    - **Validates: Requirements 5.3, 5.4**

- [ ] 8. تنفيذ NavigateToLoginCommand
  - [x] 8.1 إنشاء NavigateToLoginCommand
    - استدعاء INavigationService.NavigateToAsync(Constants.LoginRoute)
    - _Requirements: 6.1, 6.2_
  
  - [ ]* 8.2 كتابة unit tests لـ NavigateToLoginCommand
    - اختبار استدعاء NavigateToAsync مع LoginRoute
    - _Requirements: 6.2_
  
  - [ ]* 8.3 كتابة property test للتنقل إلى Login
    - **Property 10: Navigation to Login**
    - **Validates: Requirements 6.2**

- [ ] 9. تنفيذ اختبارات Constructor Validation
  - [ ]* 9.1 كتابة property test للتحقق من null parameters
    - **Property 11: Constructor Null Parameter Validation**
    - **Validates: Requirements 7.2**

- [x] 10. Checkpoint النهائي - التحقق من جميع الاختبارات
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 11. التكامل والمراجعة النهائية
  - [x] 11.1 التحقق من تغطية جميع المتطلبات
    - مراجعة أن جميع المتطلبات تم تنفيذها
    - التحقق من أن جميع الخصائص تعمل بشكل صحيح
    - التحقق من أن جميع الأوامر تعمل بشكل صحيح
    - _Requirements: All_
  
  - [ ]* 11.2 تشغيل جميع الاختبارات
    - تشغيل جميع unit tests
    - تشغيل جميع property-based tests
    - التحقق من تغطية الكود (80%+)
    - _Requirements: All_

## Notes

- المهام المميزة بـ `*` اختيارية ويمكن تخطيها للحصول على MVP أسرع
- كل مهمة تشير إلى المتطلبات المحددة لتتبع الارتباط
- Checkpoints تضمن التحقق التدريجي
- Property tests تتحقق من الخصائص العامة للصحة
- Unit tests تتحقق من أمثلة محددة وحالات حدية
- يجب استخدام FsCheck لـ property-based testing مع 100+ iterations
- جميع رسائل الخطأ يجب أن تكون باللغة العربية من Constants
- CustomerRegisterDTO يتم إرساله إلى API الذي ينشئ سجلين (Users و Customers) مع علاقة 1:1
