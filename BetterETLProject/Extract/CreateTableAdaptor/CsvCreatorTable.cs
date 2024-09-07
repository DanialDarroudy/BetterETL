using System.Data;
using BetterETLProject.Connection;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public class CsvCreatorTable : ICreatorTable
{
    private readonly StreamReader _streamReader;
    private readonly IDbCommand _command;

    public CsvCreatorTable(StreamReader streamReader, IDbCommand command)
    {
        _streamReader = streamReader;
        _command = command;
    }
    
    public List<string> GetColumnNames()
    {
        var result = _streamReader.ReadLine()!.Split(',').ToList();
        _streamReader.Dispose();
        return result;
    }

    public void CreateTable(string query, ICreatorConnection creatorConnection)
    {
        var connection = creatorConnection.CreateConnection();
        _command.CommandText = query;
        _command.Connection = connection;
        _command.ExecuteNonQuery();
        connection.Dispose();
        _command.Dispose();
    }
}