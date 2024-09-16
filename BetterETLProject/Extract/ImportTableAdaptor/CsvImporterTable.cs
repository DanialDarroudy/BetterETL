using BetterETLProject.Connection;
using Npgsql;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private readonly ILogger<CsvImporterTable> _logger;
    private readonly StreamReader _streamReader;
    private const int ChunkSize = 75000;
    private const int NumberOfThreads = 25;

    public CsvImporterTable(StreamReader streamReader, ILogger<CsvImporterTable> logger)
    {
        _logger = logger;
        _streamReader = new StreamReader(new BufferedStream(streamReader.BaseStream));
    }
    public async Task ImportDataToTable(string query, ICreatorConnection creatorConnection)
    {
        await _streamReader.ReadLineAsync();
        var tasks = new List<Task>();
        var chunkedRows = new string[ChunkSize];
        var indexInChunk = 0;
        while (!_streamReader.EndOfStream)
        {
            chunkedRows[indexInChunk] = (await _streamReader.ReadLineAsync())!;
            indexInChunk++;
            if (indexInChunk < ChunkSize) continue;
            const int rowsPerThread = ChunkSize / NumberOfThreads;
            for (var i = 0; i < NumberOfThreads; i++)
            {
                var start = i * rowsPerThread;
                var end = (i + 1) * rowsPerThread;
                tasks.Add(Task.Run(() => WriteEachThread(query, creatorConnection, chunkedRows[start..end])));
            }

            await Task.WhenAll(tasks);
            indexInChunk = 0;
        }
        _logger.LogInformation("All chunk is read");
        if (indexInChunk > 0)
        {
            tasks.Add(Task.Run(() => WriteEachThread(query, creatorConnection, chunkedRows[0..indexInChunk])));
            _logger.LogInformation("Remain is read");
        }
        _logger.LogInformation("All thread is finished");
        _streamReader.Dispose();
    }

    private async Task WriteEachThread(string query, ICreatorConnection creatorConnection, string[] rows)
    {
        using var connection = creatorConnection.CreateConnection();
        if (connection is not NpgsqlConnection npgsqlConnection) return;
        await using var writer = await npgsqlConnection.BeginTextImportAsync(query);
        await writer.WriteAsync(string.Join("\n", rows) + "\n");
    }
}