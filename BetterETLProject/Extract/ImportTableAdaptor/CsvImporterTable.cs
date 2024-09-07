using BetterETLProject.Connection;
using BetterETLProject.DTO;
using nietras.SeparatedValues;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public class CsvImporterTable : IImporterTable
{
    private const int NumberOfThread = 20;
    private readonly ImportDataDto _dto;
    private string _query;
    private ICreatorConnection _creatorConnection;
    private readonly Dictionary<string, Tuple<int, int>> _indexes;

    public CsvImporterTable(ImportDataDto dto)
    {
        _dto = dto;
        _indexes = new Dictionary<string, Tuple<int, int>>();
    }

    public void ImportDataToTable(string query, ICreatorConnection creatorConnection)
    {
        _query = query;
        _creatorConnection = creatorConnection;
        var numberOfRows = GetNumberOfRows();
        var threads = new Thread[NumberOfThread];
        var chunkSize = numberOfRows / NumberOfThread;
        for (var i = 0; i < NumberOfThread; i++)
        {
            var start = i * chunkSize;
            var end = GetEndIndex(i, numberOfRows, chunkSize);
            threads[i] = new Thread(ThreadImportInTable) { Name = i.ToString() };
            _indexes[i.ToString()] = new Tuple<int, int>(start, end);
            threads[i].Start();
        }
    }

    private int GetEndIndex(int i, int numberOfRows, int chunkSize)
    {
        return (i == NumberOfThread - 1) ? numberOfRows : (i + 1) * chunkSize;
    }

    private int GetNumberOfRows()
    {
        return File.ReadLines(_dto.FilePath.ToString()).Count();
    }

    private void ThreadImportInTable()
    {
        var connection = _creatorConnection.CreateConnection();

        using var writer = connection.BeginTextImport(_query);
        using var reader = Sep.Reader().From(new StreamReader(_dto.FilePath.ToString()));
        var start = _indexes[Thread.CurrentThread.Name!].Item1;
        var end = _indexes[Thread.CurrentThread.Name!].Item2;
        var offset = start;
        while (offset < end)
        {
            writer.WriteLine(reader.);
            offset++;
        }

        connection.Dispose();
    }
}