using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.DataBase;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Sources;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter ,  ICreatorTable , IGetterColumnNames , IImporterData
{
    public void Convert(ImportDataDto dto)
    {
        var columnNames = GetColumnNames(dto.FilePath);
        using var connection = CreatorConnection.CreateConnection(dto.Address);
        CreateTable(QueryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames), connection);

        ImportDataToTable(QueryGenerator.GenerateCopyQuery(dto.FilePath, columnNames), dto.FilePath, connection);
    }

    public List<string> GetColumnNames(FilePath filePath)
    {
        Validator.CheckNull(filePath);
        using var reader = new StreamReader(filePath.ToString());
        return reader.ReadLine()!.Split(',').ToList();
    }


    public void CreateTable(string query, NpgsqlConnection connection)
    {
        using var command = new NpgsqlCommand(query, connection);
        command.ExecuteNonQuery();
    }


    public void ImportDataToTable(string query, FilePath filePath, NpgsqlConnection connection)
    {
        using var reader = new StreamReader(filePath.ToString());
        reader.ReadLine();
        using var writer = connection.BeginTextImport(query);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine()!;
            writer.WriteLine(line);
        }
    }
}