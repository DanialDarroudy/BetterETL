using System.Data;
using BetterETLProject.Sources;

namespace BetterETLProject.Extract.Import;

public interface IImporterTable
{
    public void ImportDataToTable(string query, FilePath filePath, IDbConnection connection);
}