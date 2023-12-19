// Controllers/CustomerController.cs
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.Enums;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Models;
using UserManagement.Service.Email;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Users")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
        //_emailService = emailService;
       // _backgroundJobClient = backgroundJobClient;
    }

    [HttpGet]
    [Route("Customers")]    
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _customerService.Customers();
        return Ok(customers);
    }

    [HttpGet]
    [Route("Orders")]
    public async Task<ActionResult<Customer>> GetOrders()
    {
        return Ok(await _customerService.Orders());
    }


    [HttpPost]
    [Route("Customers/Add")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
    {
        var newCustomer = await _customerService.AddNewCustomer(customer);

        return StatusCode(201, new { Customer = newCustomer, message = "Customer created successfully", StatusCode = "201" });
    }
}
