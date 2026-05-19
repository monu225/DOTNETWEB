using System.Data;
using Microsoft.Data.SqlClient;

namespace WEBAPI_CRUD.Data;

public sealed class DBConnection
{
    private readonly string _connectionString;
    private readonly ILogger<DBConnection> _logger;

    public DBConnection(IConfiguration configuration, ILogger<DBConnection> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");
        _logger = logger;
    }

    public DataTable ExecuteDataTable(string commandText, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
    {
        var dataSet = ExecuteDataSet(commandText, parameters, commandType);
        return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
    }

    public DataSet ExecuteDataSet(string commandText, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
    {
        using var connection = CreateConnection();
        using var command = CreateCommand(connection, commandText, parameters, commandType);
        using var adapter = new SqlDataAdapter(command);

        var dataSet = new DataSet();
        _logger.LogDebug("Executing dataset command: {CommandText}", commandText);
        adapter.Fill(dataSet);
        return dataSet;
    }

    public int ExecuteNonQuery(string commandText, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
    {
        using var connection = CreateConnection();
        using var command = CreateCommand(connection, commandText, parameters, commandType);

        connection.Open();
        _logger.LogDebug("Executing non-query command: {CommandText}", commandText);
        return command.ExecuteNonQuery();
    }

    public object? ExecuteScalar(string commandText, SqlParameter[]? parameters = null, CommandType commandType = CommandType.Text)
    {
        using var connection = CreateConnection();
        using var command = CreateCommand(connection, commandText, parameters, commandType);

        connection.Open();
        _logger.LogDebug("Executing scalar command: {CommandText}", commandText);
        return command.ExecuteScalar();
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    private static SqlCommand CreateCommand(SqlConnection connection, string commandText, SqlParameter[]? parameters, CommandType commandType)
    {
        var command = new SqlCommand(commandText, connection)
        {
            CommandType = commandType
        };

        if (parameters is { Length: > 0 })
        {
            command.Parameters.AddRange(parameters);
        }

        return command;
    }
}
