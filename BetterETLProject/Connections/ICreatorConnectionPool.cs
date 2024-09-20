using System.Data.Common;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Connections;

public interface ICreatorConnectionPool
{
    public IConnectionPool Create(ICreatorConnection creatorConnection);
}