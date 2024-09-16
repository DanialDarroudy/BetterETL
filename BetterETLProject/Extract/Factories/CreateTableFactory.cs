using System.Data;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;

namespace BetterETLProject.Extract.Factories;

public class CreateTableFactory : IFactory<ICreatorTable>
{
    private readonly ILogger<CreateTableFactory> _logger;
    private readonly IServiceProvider _provider;


    public CreateTableFactory(ILogger<CreateTableFactory> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public ICreatorTable Create(ImportDataDto dto)
    {
        ICreatorTable result = null!;
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            result = new CsvCreatorTable(new StreamReader(dto.FilePath.ToString())
                , _provider.GetRequiredService<IDbCommand>()
                , _provider.GetRequiredService<ILogger<CsvCreatorTable>>());
        }
        else
        {
            _logger.LogError("Doesnt create creator table");
        }
        return result;
    }
}