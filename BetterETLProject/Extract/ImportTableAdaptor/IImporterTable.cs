using BetterETLProject.Connection;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public interface IImporterTable
{
    public Task ImportDataToTable(string query , ICreatorConnection creatorConnection);
}