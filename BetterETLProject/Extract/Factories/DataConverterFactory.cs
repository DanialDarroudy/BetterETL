using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;

namespace BetterETLProject.Extract.Factories;

public class DataConverterFactory : IFactory<IDataConverter>
{
    private readonly ILogger<DataConverterFactory> _logger;
    private readonly IServiceProvider _provider;

    public DataConverterFactory(ILogger<DataConverterFactory> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public IDataConverter Create(ImportDataDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        IDataConverter result = null!;
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Create csv data converter");
            result = _provider.GetRequiredService<CsvDataConverter>();
        }
        else
        {
            _logger.LogError("Doesnt create data converter");
        }
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        return result;
    }
}