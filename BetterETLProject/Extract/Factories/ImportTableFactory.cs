using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.ImportTableAdaptor;

namespace BetterETLProject.Extract.Factories;

public class ImportTableFactory : IFactory<IImporterTable>
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _provider;

    public ImportTableFactory(ILogger<ImportTableFactory> logger , IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public IImporterTable Create(ImportDataDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        IImporterTable result = null!;
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Create csv importer table");
            result = new CsvImporterTable(new StreamReader(dto.FilePath.ToString())
                , _provider.GetRequiredService<ILogger<CsvImporterTable>>());
        }
        else
        {
            _logger.LogError("Doesnt create importer table");
        }
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        return result;
    }
}