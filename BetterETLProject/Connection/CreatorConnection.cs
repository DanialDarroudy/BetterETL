using System.Data;
using System.Reflection;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Connection;

public class CreatorConnection : ICreatorConnection
{
    public ConnectionSetting Address { get; set; } = null!;

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(Address.ToString());
        connection.Open();
        return connection;
    }
}