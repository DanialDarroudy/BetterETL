using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;
using Npgsql;

namespace BetterETLProject.Transform;

public class Condition
{
    public DataTable PerformFilter(ConditionDto dto)
    {
        var query = QueryGenerator.GenerateApplyConditionQuery(dto);

        var dataTable = new DataTable();
        using var connection = CreatorConnection.CreateConnection(dto.Address);
        using var command = new NpgsqlCommand(query, connection);
        using var adapter = new NpgsqlDataAdapter(command);
        adapter.Fill(dataTable);

        return dataTable;
    }
}