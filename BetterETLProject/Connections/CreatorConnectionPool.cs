
using BetterETLProject.Sources;

namespace BetterETLProject.Connections;

public class CreatorConnectionPool : ICreatorConnectionPool
{
    public IConnectionPool Create(ICreatorConnection creatorConnection)
    {
        return new ConnectionPool(creatorConnection);
    }
}