using System.Data;
using System.Reflection;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Connection;

public class CreatorConnection : ICreatorConnection
{
    public ConnectionSetting Address { get; set; } = null!;
    private readonly ILogger<CreatorConnection> _logger;

    public CreatorConnection(ILogger<CreatorConnection> logger)
    {
        _logger = logger;
    }


    public IDbConnection CreateConnection()
    {
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name , GetType().Name);
        var connection = new NpgsqlConnection(Address.ToString());
        connection.Open();
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        return connection;
    }
}