using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Repositories;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetAll();
    Employee? GetById(int id);
    Employee Create(EmployeeCreateDto employee);
    bool Update(int id, EmployeeUpdateDto employee);
    bool Delete(int id);
}
