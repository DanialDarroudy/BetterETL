using System.Data.Common;
using BetterETLProject.Connections;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public interface IImporterTable
{
    public Task ImportDataToTable(string query , IConnectionPool pool);
}