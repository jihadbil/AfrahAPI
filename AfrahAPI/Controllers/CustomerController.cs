using AfrahAPI.Models.DTOs.Customer;
using AfrahAPI.Models.DTOs.Auth;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة العملاء
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
     private readonly IAuthService _authService;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomerController(ICustomerService customerService, IAuthService authService)
    {
        _customerService = customerService;
        _authService = authService;
    }

    /// <summary>
    /// تسجيل عميل جديد (إنشاء مستخدم + عميل)
    /// </summary>
    /// <param name="registerDto">بيانات التسجيل الكاملة</param>
    /// <returns>نتيجة عملية التسجيل</returns>
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] CustomerCreateDTO registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);



       // var userResult = await _authService.RegisterAsync(userRegisterDto);

   

        // 2. إنشاء العميل وربطه بالمستخدم
        var customerCreateDto = new CustomerCreateDTO
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            DateOfBirth = registerDto.DateOfBirth,
            Address = registerDto.Address,
            Gender = registerDto.Gender,
            Country = registerDto.Country,
            City = registerDto.City,
            Nationality = registerDto.Nationality,
            UserID =registerDto.UserID
        };

        try
        {
            var customer = await _customerService.CreateAsync(customerCreateDto);
            return Ok(customer);
        }
        catch (Exception)
        {
            // في حالة فشل إنشاء العميل، يجب حذف المستخدم (rollback)
            // TODO: إضافة آلية rollback
            return StatusCode(500, new { message = "فشل إنشاء العميل بعد إنشاء المستخدم" });
        }
    }

    /// <summary>
    /// الحصول على جميع العملاء
    /// </summary>
    /// <returns>قائمة بجميع العملاء</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerReadDTO>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    /// <summary>
    /// الحصول على عميل بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف العميل</param>
    /// <returns>بيانات العميل</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerReadDTO>> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound($"لم يتم العثور على عميل بالمعرف: {id}");

        return Ok(customer);
    }

    /// <summary>
    /// إنشاء عميل جديد
    /// </summary>
    /// <param name="createDto">بيانات العميل الجديد</param>
    /// <returns>بيانات العميل المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<CustomerReadDTO>> Create([FromBody] CustomerCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = customer.CustomerID }, customer);
    }

    /// <summary>
    /// تحديث بيانات عميل موجود
    /// </summary>
    /// <param name="id">معرف العميل</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات العميل المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerReadDTO>> Update(Guid id, [FromBody] CustomerUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.UpdateAsync(id, updateDto);
        if (customer == null)
            return NotFound($"لم يتم العثور على عميل بالمعرف: {id}");

        return Ok(customer);
    }

    /// <summary>
    /// حذف عميل
    /// </summary>
    /// <param name="id">معرف العميل</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _customerService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على عميل بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على عميل بواسطة معرف المستخدم المرتبط
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>بيانات العميل</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<CustomerReadDTO>> GetByUserId(Guid userId)
    {
        var customer = await _customerService.GetCustomerByUserIdAsync(userId);
        if (customer == null)
            return NotFound($"لم يتم العثور على عميل لمعرف المستخدم: {userId}");

        return Ok(customer);
    }

    /// <summary>
    /// الحصول على جميع حجوزات العميل
    /// </summary>
    /// <param name="id">معرف العميل</param>
    /// <returns>قائمة بحجوزات العميل</returns>
    [HttpGet("{id}/bookings")]
    public async Task<ActionResult<IEnumerable<object>>> GetBookings(Guid id)
    {
        var bookings = await _customerService.GetCustomerBookingsAsync(id);
        return Ok(bookings);
    }

    /// <summary>
    /// تحديث ملف العميل الشخصي
    /// </summary>
    /// <param name="id">معرف العميل</param>
    /// <param name="updateDto">بيانات التحديث</param>
    /// <returns>بيانات العميل المُحدث</returns>
    [HttpPut("{id}/profile")]
    public async Task<ActionResult<CustomerReadDTO>> UpdateProfile(Guid id, [FromBody] CustomerUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.UpdateProfileAsync(id, updateDto);
        if (customer == null)
            return NotFound($"لم يتم العثور على عميل بالمعرف: {id}");

        return Ok(customer);
    }
}

