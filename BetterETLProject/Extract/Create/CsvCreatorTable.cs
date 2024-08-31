using System.Data;
using BetterETLProject.Sources;
using BetterETLProject.Validation;

namespace BetterETLProject.Extract.Create;

public class CsvCreatorTable : ICreatorTable
{
    private readonly StreamReader _streamReader;
    private readonly IDbCommand _command;

    public CsvCreatorTable(StreamReader streamReader, IDbCommand command)
    {
        _streamReader = streamReader;
        _command = command;
    }
    
    public List<string> GetColumnNames(FilePath filePath)
    {
        Validator.CheckNull(filePath);
        var result = _streamReader.ReadLine()!.Split(',').ToList();
        _streamReader.Dispose();
        return result;
    }

    public void CreateTable(string query, IDbConnection connection)
    {
        _command.CommandText = query;
        _command.Connection = connection;
        _command.ExecuteNonQuery();
        _command.Dispose();
    }
}