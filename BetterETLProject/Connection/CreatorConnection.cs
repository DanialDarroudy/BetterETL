using BetterETLProject.Sources;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Connection;

public static class CreatorConnection
{
    public static NpgsqlConnection CreateConnection(ConnectionSetting address)
    {
        Validator.CheckNull(address);
        var connection = new NpgsqlConnection(address.ToString());
        connection.Open();
        return connection;
    }
}