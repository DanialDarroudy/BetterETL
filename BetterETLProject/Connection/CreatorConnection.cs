using System.Data;
using BetterETLProject.Sources;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Connection;

public class CreatorConnection : ICreatorConnection
{ 
    private readonly ConnectionSetting _address;

    public CreatorConnection(ConnectionSetting address)
    {
        _address = address;
    }
    public NpgsqlConnection CreateConnection()
    {
        Validator.CheckNull(_address);
        var connection = new NpgsqlConnection(_address.ToString());
        connection.Open();
        return connection;
    }
}