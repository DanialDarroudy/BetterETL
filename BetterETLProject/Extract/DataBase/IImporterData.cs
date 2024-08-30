using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Extract.DataBase;

public interface IImporterData
{
    public void ImportDataToTable(string query, FilePath filePath, NpgsqlConnection connection);
}