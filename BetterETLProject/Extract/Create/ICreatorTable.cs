using System.Data;
using BetterETLProject.Sources;

namespace BetterETLProject.Extract.Create;

public interface ICreatorTable
{
    public List<string> GetColumnNames(FilePath filePath);
    public void CreateTable(string query, IDbConnection connection);
}