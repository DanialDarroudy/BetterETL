using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.Sources;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    public void Convert(ImportDataDto dto)
    {
        var columnNames = GetColumnNames(dto.Path);
        using var connection = CreatorConnection.CreateConnection(dto.Address);
        CreateTable(QueryGenerator.GenerateCreateTempTableQuery(dto.Path.TableName, columnNames), connection);

        ImportDataToTable(QueryGenerator.GenerateCopyQuery(dto.Path, columnNames), dto.Path, connection);
    }

    private List<string> GetColumnNames(PathFile path)
    {
        Validator.CheckNull(path);
        using var reader = new StreamReader(path.ToString());
        return reader.ReadLine()!.Split(',').ToList();
    }


    private void CreateTable(string query, NpgsqlConnection connection)
    {
        using var command = new NpgsqlCommand(query, connection);
        command.ExecuteNonQuery();
    }


    private void ImportDataToTable(string query, PathFile path, NpgsqlConnection connection)
    {
        using var reader = new StreamReader(path.ToString());
        reader.ReadLine();
        using var writer = connection.BeginTextImport(query);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!;
            writer.WriteLine(line);
        }
    }
}