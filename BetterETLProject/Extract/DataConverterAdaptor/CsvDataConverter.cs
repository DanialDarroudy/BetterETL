using System.Reflection;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.Factories;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.QueryGeneration;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly IFactory<ICreatorTable> _creatorFactory;
    private readonly IFactory<IImporterTable> _importerFactory;
    private readonly IQueryGenerator _queryGenerator;
    private readonly ILogger<CsvDataConverter> _logger;

    public CsvDataConverter(ICreatorConnection creatorConnection, IFactory<IImporterTable> importerFactory
        , IFactory<ICreatorTable> creatorFactory, IQueryGenerator queryGenerator, ILogger<CsvDataConverter> logger)
    {
        _creatorConnection = creatorConnection;
        _importerFactory = importerFactory;
        _creatorFactory = creatorFactory;
        _queryGenerator = queryGenerator;
        _logger = logger;
    }

    public void Convert(ImportDataDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        var creatorTable = _creatorFactory.Create(dto);
        var importerTable = _importerFactory.Create(dto);
        _creatorConnection.Address = dto.Address;
        var columnNames = creatorTable.GetColumnNames();
        creatorTable.CreateTable(
            _queryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames), _creatorConnection);
        importerTable.ImportDataToTable(
            _queryGenerator.GenerateCopyQuery(dto.FilePath, columnNames), _creatorConnection);
    }
}