using Npgsql;

namespace BetterETLProject.Extract.DataBase;

public interface ICreatorTable
{
    public void CreateTable(string query, NpgsqlConnection connection);
}