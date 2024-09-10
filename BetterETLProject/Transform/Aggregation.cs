using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;

namespace BetterETLProject.Transform;

public class Aggregation
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly IDbCommand _command;
    private readonly IDbDataAdapter _dataAdapter;

    public Aggregation(ICreatorConnection connection , IDbCommand command , IDbDataAdapter dataAdapter)
    {
        _creatorConnection = connection;
        _command = command;
        _dataAdapter = dataAdapter;
    }

    public DataTable Aggregate(AggregationDto dto)
    {
        var query = QueryGenerator.GenerateAggregateQuery(dto);

        var dataTables = new DataSet();
        var connection = _creatorConnection.CreateConnection();
        _command.CommandText = query;
        _command.Connection = connection;
        _dataAdapter.SelectCommand = _command;

        _dataAdapter.Fill(dataTables);
        _command.Dispose();
        connection.Dispose();
        return dataTables.Tables[0];
    }
}