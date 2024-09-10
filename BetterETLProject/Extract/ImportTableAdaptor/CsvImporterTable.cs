using System.Data;
using Npgsql;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private readonly StreamReader _streamReader;
    private const int ChunkSize = 5000;

    public CsvImporterTable(StreamReader streamReader)
    {
        _streamReader = streamReader;
    }

    public void ImportDataToTable(string query, IDbConnection connection)
    {
        _streamReader.ReadLine();
        if (connection is not NpgsqlConnection npgsqlConnection) return;

        var chunks = new string[ChunkSize];
        var counter = 0;
        using var writer = npgsqlConnection.BeginTextImport(query);

        while (!_streamReader.EndOfStream)
        {
            chunks[counter] = _streamReader.ReadLine()!;
            counter++;
            if (counter < ChunkSize) continue;
            writer.Write(string.Join("\n", chunks) + "\n");
            counter = 0;
        }

        if (counter > 0)
            writer.Write(string.Join("\n", chunks, 0, counter) + "\n");

        _streamReader.Dispose();
    }
}