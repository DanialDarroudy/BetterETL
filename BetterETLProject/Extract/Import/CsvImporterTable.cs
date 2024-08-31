using System.Data;
using BetterETLProject.Sources;
using Npgsql;

namespace BetterETLProject.Extract.Import;

public class CsvImporterTable
{
    private readonly StreamReader _streamReader;

    public CsvImporterTable(StreamReader streamReader)
    {
        _streamReader = streamReader;
    }
    public void ImportDataToTable(string query, FilePath filePath, IDbConnection connection)
    {
        _streamReader.ReadLine();
        if (connection is not NpgsqlConnection npgsqlConnection) return;
        using var writer = npgsqlConnection.BeginTextImport(query);
        while (!_streamReader.EndOfStream)
        {
            var line = _streamReader.ReadLine()!;
            writer.WriteLine(line);
        }
        _streamReader.Dispose();
    }
}