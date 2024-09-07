using BetterETLProject.Connection;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public interface IImporterTable
{
    public void ImportDataToTable(string query, ICreatorConnection creatorConnection);
}