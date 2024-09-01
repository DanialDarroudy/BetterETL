using System.Data;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public interface ICreatorTable
{
    public List<string> GetColumnNames();
    public void CreateTable(string query, IDbConnection connection);
}