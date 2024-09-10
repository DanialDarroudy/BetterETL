using System.Data;
using BetterETLProject.Connection;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public interface ICreatorTable
{
    public List<string> GetColumnNames();
    public void CreateTable(string query, ICreatorConnection creatorConnection);
}