using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Validation;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly ICreatorTable _creatorTable;
    private readonly IImporterTable _importerTable;

    public CsvDataConverter(ICreatorConnection creatorConnection, ICreatorTable creatorTable,
        IImporterTable importerTable)
    {
        _creatorConnection = creatorConnection;
        _creatorTable = creatorTable;
        _importerTable = importerTable;
    }

    public void Convert(ImportDataDto dto)
    {
        Validator.CheckNull(dto);
        var columnNames = _creatorTable.GetColumnNames();
        _creatorTable.CreateTable(
            QueryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames), _creatorConnection);

        _importerTable.ImportDataToTable(
            QueryGenerator.GenerateCopyQuery(dto.FilePath, columnNames), _creatorConnection);
    }
}