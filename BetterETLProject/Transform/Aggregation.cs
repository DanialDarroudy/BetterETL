using System.Data;
using System.Reflection;
using BetterETLProject.Connections;
using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;

namespace BetterETLProject.Transform;

public class Aggregation : IAggregation
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly IDbCommand _command;
    private readonly IDbDataAdapter _dataAdapter;
    private readonly IQueryGenerator _queryGenerator;
    private readonly ILogger<Aggregation> _logger;

    public Aggregation(ICreatorConnection connection, IDbCommand command, IDbDataAdapter dataAdapter
        , IQueryGenerator queryGenerator, ILogger<Aggregation> logger)
    {
        _creatorConnection = connection;
        _command = command;
        _dataAdapter = dataAdapter;
        _queryGenerator = queryGenerator;
        _logger = logger;
    }

    public DataTable Aggregate(AggregationDto dto)
    {
        var query = _queryGenerator.GenerateAggregateQuery(dto);
        _creatorConnection.Address = dto.Address;
        var dataTables = new DataSet();
        var connection = _creatorConnection.CreateConnection();
        _command.CommandText = query;
        _command.Connection = connection;
        _dataAdapter.SelectCommand = _command;
        _logger.LogInformation("Start filling table");
        _dataAdapter.Fill(dataTables);
        _logger.LogInformation("Table is filled");
        _command.Dispose();
        connection.Dispose();
        return dataTables.Tables[0];
    }
}