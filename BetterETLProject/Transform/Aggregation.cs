using System.Data;
using System.Reflection;
using BetterETLProject.Connection;
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
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        var query = _queryGenerator.GenerateAggregateQuery(dto);
        _creatorConnection.Address = dto.Address;
        var dataTables = new DataSet();
        var connection = _creatorConnection.CreateConnection();
        _command.CommandText = query;
        _command.Connection = connection;
        _dataAdapter.SelectCommand = _command;
        _logger.LogInformation("Start filling {Tables} tables with this adaptor {Adaptor}", dataTables, _dataAdapter);
        _dataAdapter.Fill(dataTables);
        _logger.LogInformation("{Tables} tables is filled", dataTables);
        _command.Dispose();
        connection.Dispose();
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        return dataTables.Tables[0];
    }
}