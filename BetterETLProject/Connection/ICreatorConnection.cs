using System.Data;
using BetterETLProject.Sources;

namespace BetterETLProject.Connection;

public interface ICreatorConnection
{
    public ConnectionSetting Address { get; set; }
    public IDbConnection CreateConnection();
}