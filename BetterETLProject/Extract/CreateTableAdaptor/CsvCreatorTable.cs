using System.Data;
using System.Reflection;
using BetterETLProject.Connection;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public class CsvCreatorTable : ICreatorTable
{
    private readonly StreamReader _streamReader;
    private readonly IDbCommand _command;
    private readonly ILogger<CsvCreatorTable> _logger;

    public CsvCreatorTable(StreamReader streamReader, IDbCommand command, ILogger<CsvCreatorTable> logger)
    {
        _streamReader = streamReader;
        _command = command;
        _logger = logger;
    }
    
    public List<string> GetColumnNames()
    {
        _logger.LogInformation("Reading header of csv file");
        var result = _streamReader.ReadLine()!.Split(',').ToList();
        _logger.LogInformation("Header of csv file is read");
        _streamReader.Dispose();
        return result;
    }

    public void CreateTable(string query, ICreatorConnection creatorConnection)
    {
        _command.CommandText = query;
        _command.Connection = creatorConnection.CreateConnection();
        _logger.LogInformation("Table is creating");
        _command.ExecuteNonQuery();
        _logger.LogInformation("Table is created");
        _command.Connection.Dispose();
        _command.Dispose();
    }
}