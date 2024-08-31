using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Validation;

namespace BetterETLProject.Transform;

public class Condition
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly IDbCommand _command;
    private readonly IDbDataAdapter _dataAdapter;

    public Condition(ICreatorConnection connection , IDbCommand command , IDbDataAdapter dataAdapter)
    {
        _creatorConnection = connection;
        _command = command;
        _dataAdapter = dataAdapter;
    }

    public DataTable PerformFilter(ConditionDto dto)
    {
        Validator.CheckNull(dto);
        var query = QueryGenerator.GenerateApplyConditionQuery(dto);

        var dataTables = new DataSet();
        using var connection = _creatorConnection.CreateConnection(dto.Address);
        _command.CommandText = query;
        _command.Connection = connection;
        _dataAdapter.InsertCommand = _command;
        _dataAdapter.Fill(dataTables);

        return dataTables.Tables[0];
    }
}