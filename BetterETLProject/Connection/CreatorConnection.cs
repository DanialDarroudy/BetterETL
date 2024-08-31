using System.Data;
using BetterETLProject.Sources;
using BetterETLProject.Validation;

namespace BetterETLProject.Connection;

public class CreatorConnection : ICreatorConnection
{
    private readonly IDbConnection _connection;

    public CreatorConnection(IDbConnection connection)
    {
        _connection = connection;
    }
    public IDbConnection CreateConnection(ConnectionSetting address)
    {
        Validator.CheckNull(address);
        _connection.ConnectionString = address.ToString();
        _connection.Open();
        return _connection;
    }
}