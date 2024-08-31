using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.Create;
using BetterETLProject.Extract.Import;
using BetterETLProject.QueryGeneration;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    private readonly ICreatorConnection _connection;
    private readonly ICreatorTable _dataBaseHelper;
    private readonly IImporterTable _importerTable;

    public CsvDataConverter(ICreatorConnection connection, ICreatorTable dataBaseHelper , IImporterTable importerTable)
    {
        _connection = connection;
        _dataBaseHelper = dataBaseHelper;
        _importerTable = importerTable;
    }

    public void Convert(ImportDataDto dto)
    {
        var columnNames = _dataBaseHelper.GetColumnNames(dto.FilePath);
        using var connection = _connection.CreateConnection(dto.Address);
        _dataBaseHelper.CreateTable(
            QueryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames),connection);

        _importerTable.ImportDataToTable(
            QueryGenerator.GenerateCopyQuery(dto.FilePath, columnNames), dto.FilePath,connection);
    }
}