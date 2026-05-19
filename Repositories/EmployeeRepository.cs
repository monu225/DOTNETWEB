using System.Data;
using Microsoft.Data.SqlClient;
using WEBAPI_CRUD.Data;
using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly DBConnection _dbConnection;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(DBConnection dbConnection, ILogger<EmployeeRepository> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public IEnumerable<Employee> GetAll()
    {
        const string sql = """
            SELECT Id, FirstName, LastName, Email, Department, Salary, JoiningDate, IsActive, CreatedAt, UpdatedAt
            FROM Employees
            ORDER BY Id DESC;
            """;

        var table = _dbConnection.ExecuteDataTable(sql);
        return table.AsEnumerable().Select(MapEmployee).ToList();
    }

    public Employee? GetById(int id)
    {
        const string sql = """
            SELECT Id, FirstName, LastName, Email, Department, Salary, JoiningDate, IsActive, CreatedAt, UpdatedAt
            FROM Employees
            WHERE Id = @Id;
            """;

        var table = _dbConnection.ExecuteDataTable(sql, [new SqlParameter("@Id", id)]);
        return table.Rows.Count == 0 ? null : MapEmployee(table.Rows[0]);
    }

    public Employee Create(EmployeeCreateDto employee)
    {
        const string sql = """
            INSERT INTO Employees (FirstName, LastName, Email, Department, Salary, JoiningDate, IsActive, CreatedAt)
            OUTPUT INSERTED.Id
            VALUES (@FirstName, @LastName, @Email, @Department, @Salary, @JoiningDate, 1, SYSUTCDATETIME());
            """;

        var parameters = CreateEmployeeParameters(employee);
        var newId = Convert.ToInt32(_dbConnection.ExecuteScalar(sql, parameters));

        _logger.LogInformation("Employee created with id {EmployeeId}", newId);
        return GetById(newId) ?? throw new InvalidOperationException("Employee was created but could not be loaded.");
    }

    public bool Update(int id, EmployeeUpdateDto employee)
    {
        const string sql = """
            UPDATE Employees
            SET FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                Department = @Department,
                Salary = @Salary,
                JoiningDate = @JoiningDate,
                IsActive = @IsActive,
                UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id;
            """;

        var parameters = new[]
        {
            new SqlParameter("@Id", id),
            new SqlParameter("@FirstName", employee.FirstName),
            new SqlParameter("@LastName", employee.LastName),
            new SqlParameter("@Email", employee.Email),
            new SqlParameter("@Department", employee.Department),
            new SqlParameter("@Salary", employee.Salary),
            new SqlParameter("@JoiningDate", employee.JoiningDate),
            new SqlParameter("@IsActive", employee.IsActive)
        };

        var affectedRows = _dbConnection.ExecuteNonQuery(sql, parameters);
        _logger.LogInformation("Employee update attempted for id {EmployeeId}; affected rows {AffectedRows}", id, affectedRows);
        return affectedRows > 0;
    }

    public bool Delete(int id)
    {
        const string sql = "DELETE FROM Employees WHERE Id = @Id;";
        var affectedRows = _dbConnection.ExecuteNonQuery(sql, [new SqlParameter("@Id", id)]);

        _logger.LogInformation("Employee delete attempted for id {EmployeeId}; affected rows {AffectedRows}", id, affectedRows);
        return affectedRows > 0;
    }

    private static SqlParameter[] CreateEmployeeParameters(EmployeeCreateDto employee) =>
    [
        new SqlParameter("@FirstName", employee.FirstName),
        new SqlParameter("@LastName", employee.LastName),
        new SqlParameter("@Email", employee.Email),
        new SqlParameter("@Department", employee.Department),
        new SqlParameter("@Salary", employee.Salary),
        new SqlParameter("@JoiningDate", employee.JoiningDate)
    ];

    private static Employee MapEmployee(DataRow row) => new()
    {
        Id = row.Field<int>("Id"),
        FirstName = row.Field<string>("FirstName") ?? string.Empty,
        LastName = row.Field<string>("LastName") ?? string.Empty,
        Email = row.Field<string>("Email") ?? string.Empty,
        Department = row.Field<string>("Department") ?? string.Empty,
        Salary = row.Field<decimal>("Salary"),
        JoiningDate = row.Field<DateTime>("JoiningDate"),
        IsActive = row.Field<bool>("IsActive"),
        CreatedAt = row.Field<DateTime>("CreatedAt"),
        UpdatedAt = row.Field<DateTime?>("UpdatedAt")
    };
}
