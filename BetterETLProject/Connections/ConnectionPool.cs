using System.Collections.Concurrent;
using System.Data;

namespace BetterETLProject.Connections;

public class ConnectionPool : IConnectionPool
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly ConcurrentQueue<IDbConnection> _pool;
    private int MaxPoolSize = 50;
    private int _currentPoolSize;

    public ConnectionPool(ICreatorConnection creatorConnection)
    {
        _pool = new ConcurrentQueue<IDbConnection>();
        _creatorConnection = creatorConnection;
        _currentPoolSize = 0;
    }

    public IDbConnection GetConnection()
    {
        if (_pool.TryDequeue(out var connection))
            return connection;
        
        lock (this)
        {
            if (_currentPoolSize < MaxPoolSize)
            {
                _currentPoolSize++;
                return _creatorConnection.CreateConnection();
            }
        }
        
        while (!_pool.TryDequeue(out connection))
        {
            Thread.Sleep(1);
        }
        return connection;
    }

    public void ReleaseConnection(IDbConnection connection)
    {
        _pool.Enqueue(connection); 
    }

    public void Dispose()
    {
        while (_pool.TryDequeue(out var connection))
        {
            connection.Close();
        }
    }
}
