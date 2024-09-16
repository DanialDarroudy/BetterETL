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
        IDataConverter result = null!;
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            result = _provider.GetRequiredService<CsvDataConverter>();
        }
        else
        {
            _logger.LogError("Doesnt create data converter");
        }
        return result;
    }
}