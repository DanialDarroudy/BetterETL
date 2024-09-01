using System.Data;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public interface IImporterTable
{
    public void ImportDataToTable(string query, IDbConnection connection);
}