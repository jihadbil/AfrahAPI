# Bugfix Requirements Document

## Introduction

عملية تسجيل حساب العميل (Customer Registration) لا تعمل بشكل صحيح. عند محاولة تسجيل مستخدم جديد بدور "Customer"، يتم إنشاء حساب المصادقة (IdentityUser) بنجاح، لكن لا يتم إنشاء السجل المقابل في جدول العملاء (Customer table). هذا يؤدي إلى عدم اكتمال عملية التسجيل وعدم قدرة العميل على استخدام النظام بشكل كامل.

المشكلة تؤثر أيضاً على تسجيل أصحاب الصالات (HallOwner) حيث أن نموذج HallOwner يتطلب أيضاً UserID مرتبط بـ IdentityUser.

## Bug Analysis

### Current Behavior (Defect)

1.1 WHEN a user registers with role "Customer" THEN the system creates only an IdentityUser record without creating the corresponding Customer record in the database

1.2 WHEN a user registers with role "HallOwner" THEN the system creates only an IdentityUser record without creating the corresponding HallOwner record in the database

1.3 WHEN the registration completes without creating Customer/HallOwner records THEN any subsequent operations requiring Customer or HallOwner data fail due to missing foreign key relationships

### Expected Behavior (Correct)

2.1 WHEN a user registers with role "Customer" THEN the system SHALL create both an IdentityUser record AND a corresponding Customer record with the UserID foreign key properly linked

2.2 WHEN a user registers with role "HallOwner" THEN the system SHALL create both an IdentityUser record AND a corresponding HallOwner record with the UserID foreign key properly linked

2.3 WHEN the Customer/HallOwner record creation fails THEN the system SHALL rollback the IdentityUser creation and return an appropriate error message

2.4 WHEN a user registers with role "Employee" or "Admin" THEN the system SHALL create only the IdentityUser record as these roles do not require additional entity records

### Unchanged Behavior (Regression Prevention)

3.1 WHEN a user attempts to register with an already existing email THEN the system SHALL CONTINUE TO return an error message "البريد الإلكتروني مستخدم بالفعل"

3.2 WHEN a user attempts to register with an already existing username THEN the system SHALL CONTINUE TO return an error message "اسم المستخدم مستخدم بالفعل"

3.3 WHEN a user registers with invalid password requirements THEN the system SHALL CONTINUE TO return validation errors from Identity framework

3.4 WHEN a user successfully logs in after registration THEN the system SHALL CONTINUE TO generate JWT tokens with correct claims and roles
