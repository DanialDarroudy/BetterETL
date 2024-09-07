using System.Data;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Connection;

public interface ICreatorConnection
{
    public NpgsqlConnection CreateConnection();
}