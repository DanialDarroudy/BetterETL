using System.Data;

namespace BetterETLProject.Connections;

public interface IConnectionPool
{
    public IDbConnection GetConnection();
    public void ReleaseConnection(IDbConnection connection);
    public void Dispose();
}