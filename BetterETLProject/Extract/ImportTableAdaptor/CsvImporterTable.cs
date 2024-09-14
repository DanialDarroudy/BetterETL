using System.Reflection;
using BetterETLProject.Connection;
using Npgsql;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private readonly ILogger<CsvImporterTable> _logger;
    private readonly StreamReader _streamReader;
    private const int ChunkSize = 250000;
    private const int NumberOfThreads = 50;

    public CsvImporterTable(StreamReader streamReader, ILogger<CsvImporterTable> logger)
    {
        _logger = logger;
        _streamReader = new StreamReader(new BufferedStream(streamReader.BaseStream));
    }
    public async Task ImportDataToTable(string query, ICreatorConnection creatorConnection)
    {
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        await _streamReader.ReadLineAsync();
        var tasks = new List<Task>();
        var chunkedRows = new string[ChunkSize];
        var indexInChunk = 0;
        while (!_streamReader.EndOfStream)
        {
            chunkedRows[indexInChunk] = (await _streamReader.ReadLineAsync())!;
            indexInChunk++;
            if (indexInChunk < ChunkSize) continue;
            _logger.LogInformation("Chunk is read");
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

        if (indexInChunk > 0)
        {
            _logger.LogInformation("Remain is read");
            tasks.Add(Task.Run(() => WriteEachThread(query, creatorConnection, chunkedRows[0..indexInChunk])));
        }

        await Task.WhenAll(tasks);
        _streamReader.Dispose();
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
    }

    private async Task WriteEachThread(string query, ICreatorConnection creatorConnection, string[] rows)
    {
        _logger.LogInformation("Thread {Thread} start writing" , Thread.CurrentThread);
        using var connection = creatorConnection.CreateConnection();
        if (connection is not NpgsqlConnection npgsqlConnection) return;
        await using var writer = await npgsqlConnection.BeginTextImportAsync(query);
        await writer.WriteAsync(string.Join("\n", rows) + "\n");
        _logger.LogInformation("Thread {Thread} finished writing" , Thread.CurrentThread);
    }
}