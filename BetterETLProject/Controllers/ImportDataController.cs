using BetterETLProject.Connections;
using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Extract.Factories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BetterETLProject.Controllers;

public class ImportDataController : Controller
{
    private readonly ILogger<ImportDataController> _logger;
    private readonly IValidator<ImportDataDto> _validator;
    private readonly IFactory<IDataConverter> _converterFactory;

    public ImportDataController(ILogger<ImportDataController> logger , IValidator<ImportDataDto> validator
        , IFactory<IDataConverter> converterFactory)
    {
        _logger = logger;
        _validator = validator;
        _converterFactory = converterFactory;
    }

    [HttpPost]
    public async Task Import([FromBody] ImportDataDto dto)
    {
        // _logger.LogInformation("Called {MethodName} action method from {ControllerName} controller",
        //     ControllerContext.ActionDescriptor.ActionName , ControllerContext.ActionDescriptor.ControllerName);
        await _validator.ValidateAndThrowAsync(dto);
        var converter = _converterFactory.Create(dto);
        await converter.Convert(dto);
        // _logger.LogInformation("{MethodName} action method from {ControllerName} controller is finished"
        //     , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        // for delete table from postgres
        // using var creator = new CreatorConnection
        // {
        //     Address = dto.Address
        // };
        // var connection = creator.CreateConnection();
        // var command = connection.CreateCommand();
        // $"DROP TABLE {dto.FilePath.TableName}";
        // command.ExecuteNonQuery();
    }
}