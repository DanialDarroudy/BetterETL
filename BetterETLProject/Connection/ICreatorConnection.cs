using System.Data;
using BetterETLProject.Sources;

namespace BetterETLProject.Connection;

public interface ICreatorConnection
{
    public IDbConnection CreateConnection();
}