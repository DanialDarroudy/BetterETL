using System.Data;
using Npgsql;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private readonly StreamReader _streamReader;

    public CsvImporterTable(StreamReader streamReader)
    {
        _streamReader = streamReader;
    }

    public void ImportDataToTable(string query, IDbConnection connection)
    {
        _streamReader.ReadLine();
        if (connection is not NpgsqlConnection npgsqlConnection) return;
        
        using var writer = npgsqlConnection.BeginTextImport(query);
        while (!_streamReader.EndOfStream)
            writer.WriteLine(_streamReader.ReadLine()!);
        
        _streamReader.Dispose();
    }
}