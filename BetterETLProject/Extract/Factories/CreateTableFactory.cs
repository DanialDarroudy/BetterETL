using System.Data;
using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using Npgsql;

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
        _logger.LogInformation("Called {MethodName} method from {ClassName} class"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        ICreatorTable result = null!;
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Create csv creator table");
            result = new CsvCreatorTable(new StreamReader(dto.FilePath.ToString())
                , _provider.GetRequiredService<IDbCommand>()
                , _provider.GetRequiredService<ILogger<CsvCreatorTable>>());
        }
        else
        {
            _logger.LogError("Doesnt create creator table");
        }
        _logger.LogInformation("{MethodName} method from {ClassName} class is finished"
            , MethodBase.GetCurrentMethod()!.Name, GetType().Name);
        return result;
    }
}