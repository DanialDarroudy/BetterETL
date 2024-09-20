using System.Data;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Connections;

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