using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Validation;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    private readonly ICreatorConnection _connection;
    private readonly ICreatorTable _creatorTable;
    private readonly IImporterTable _importerTable;

    public CsvDataConverter(ICreatorConnection connection, ICreatorTable creatorTable , IImporterTable importerTable)
    {
        _connection = connection;
        _creatorTable = creatorTable;
        _importerTable = importerTable;
    }

    public void Convert(ImportDataDto dto)
    {
        var columnNames = _creatorTable.GetColumnNames();
        var connection = _connection.CreateConnection(dto.Address);
        _creatorTable.CreateTable(
            QueryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames),connection);

        _importerTable.ImportDataToTable(
            QueryGenerator.GenerateCopyQuery(dto.FilePath, columnNames),connection);
        connection.Dispose();
    }
}