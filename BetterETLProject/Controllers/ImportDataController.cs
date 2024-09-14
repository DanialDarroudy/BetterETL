using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Extract.Factories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BetterETLProject.Controllers;

public class ImportDataController : Controller
{
    private readonly ILogger<ImportDataController> _logger;
    private readonly AbstractValidator<ImportDataDto> _validator;
    private readonly IFactory<IDataConverter> _converterFactory;

    public ImportDataController(ILogger<ImportDataController> logger , AbstractValidator<ImportDataDto> validator
        , IFactory<IDataConverter> converterFactory)
    {
        _logger = logger;
        _validator = validator;
        _converterFactory = converterFactory;
    }

    [HttpPost]
    public void Import([FromBody] ImportDataDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ControllerName}",
            ControllerContext.ActionDescriptor.ActionName , ControllerContext.ActionDescriptor.ControllerName);
        _logger.LogInformation("Validating {DTO}", dto);
        _validator.ValidateAndThrow(dto);
        _logger.LogInformation("Validation of {DTO} is successful", dto);
        var converter = _converterFactory.Create(dto);
        converter.Convert(dto);
        _logger.LogInformation("Data of {File} imported to {DataBase} successfully"
            , dto.FilePath.ToString(), dto.Address.ToString());
    }
}