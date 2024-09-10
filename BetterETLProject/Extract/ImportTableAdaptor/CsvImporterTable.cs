using BetterETLProject.Connection;
using Npgsql;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private readonly StreamReader _streamReader;
    private const int ChunkSize = 250000;
    private readonly string[] _chunkedRows = new string[ChunkSize];
    private const int NumberOfThreads = 50;

    public CsvImporterTable(StreamReader streamReader)
    {
        _streamReader = new StreamReader(new BufferedStream(streamReader.BaseStream));
    }

    public async Task ImportDataToTable(string query , ICreatorConnection creatorConnection)
    {
        await _streamReader.ReadLineAsync();
        
        var tasks = new List<Task>();
        var counter = 0;
        while (!_streamReader.EndOfStream)
        {
            _chunkedRows[counter] = (await _streamReader.ReadLineAsync())!;
            counter++;
            if (counter < ChunkSize) continue;
            const int rowsPerThread = ChunkSize / NumberOfThreads;
            for (var i = 0; i < NumberOfThreads; i++)
            {
                var start = i * rowsPerThread;
                var end = (i + 1) * rowsPerThread;
                tasks.Add(Task.Run(() => WriteEachThread(query, creatorConnection, start, end)));
            }
            counter = 0;
        }

        if (counter > 0)
        {
            tasks.Add(Task.Run(() => WriteEachThread(query, creatorConnection, 0, counter)));
        }
        await Task.WhenAll(tasks);
        _streamReader.Dispose();
    }

    private async Task WriteEachThread(string query, ICreatorConnection creatorConnection, int start, int end)
    {
        using var connection = creatorConnection.CreateConnection();
        if (connection is not NpgsqlConnection npgsqlConnection) return;
        
        await using var writer = await npgsqlConnection.BeginTextImportAsync(query);
        await writer.WriteAsync(string.Join("\n", _chunkedRows, start, end - start) + "\n");
    }
}