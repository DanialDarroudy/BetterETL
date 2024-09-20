using BetterETLProject.Connections;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.Factories;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.QueryGeneration;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public class CsvDataConverter : IDataConverter
{
    private readonly ICreatorConnection _creatorConnection;
    private readonly ICreatorConnectionPool _creatorConnectionPool;
    private readonly IFactory<ICreatorTable> _creatorFactory;
    private readonly IFactory<IImporterTable> _importerFactory;
    private readonly IQueryGenerator _queryGenerator;
    private readonly ILogger<CsvDataConverter> _logger;

    public CsvDataConverter(ICreatorConnectionPool creatorConnectionPool, IFactory<IImporterTable> importerFactory
        , IFactory<ICreatorTable> creatorFactory, IQueryGenerator queryGenerator, ILogger<CsvDataConverter> logger, ICreatorConnection creatorConnection)
    {
        _creatorConnectionPool = creatorConnectionPool;
        _importerFactory = importerFactory;
        _creatorFactory = creatorFactory;
        _queryGenerator = queryGenerator;
        _logger = logger;
        _creatorConnection = creatorConnection;
    }

    public async Task Convert(ImportDataDto dto)
    {
        var creatorTable = _creatorFactory.Create(dto);
        var importerTable = _importerFactory.Create(dto);
        _creatorConnection.Address = dto.Address;
        var pool = _creatorConnectionPool.Create(_creatorConnection);
        var columnNames = creatorTable.GetColumnNames();
        creatorTable.CreateTable(
            _queryGenerator.GenerateCreateTableQuery(dto.FilePath.TableName, columnNames), _creatorConnection);
        await importerTable.ImportDataToTable(_queryGenerator.GenerateCopyQuery(dto.FilePath, columnNames), pool);
    }
}