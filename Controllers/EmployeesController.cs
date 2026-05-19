using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Models;
using WEBAPI_CRUD.Repositories;

namespace WEBAPI_CRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        return Ok(_employeeRepository.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Employee> GetById(int id)
    {
        var employee = _employeeRepository.GetById(id);
        return employee is null ? NotFound(new { message = "Employee not found." }) : Ok(employee);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public ActionResult<Employee> Create(EmployeeCreateDto request)
    {
        var employee = _employeeRepository.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Update(int id, EmployeeUpdateDto request)
    {
        return _employeeRepository.Update(id, request)
            ? NoContent()
            : NotFound(new { message = "Employee not found." });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Delete(int id)
    {
        return _employeeRepository.Delete(id)
            ? NoContent()
            : NotFound(new { message = "Employee not found." });
    }
}
