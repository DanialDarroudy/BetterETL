using BetterETLProject.DTO;
using BetterETLProject.Transform;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;

public class AggregationController : Controller
{
    private readonly ILogger<AggregationController> _logger;
    private readonly AbstractValidator<AggregationDto> _validator;
    private readonly IAggregation _aggregation;

    public AggregationController(ILogger<AggregationController> logger, AbstractValidator<AggregationDto> validator
        , IAggregation aggregation)
    {
        _logger = logger;
        _validator = validator;
        _aggregation = aggregation;
    }

    [HttpPost]
    public IActionResult Aggregate([FromBody] AggregationDto dto)
    {
        _logger.LogInformation("Called {MethodName} action method from {ControllerName}"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        _logger.LogInformation("Validating {DTO}", dto);
        _validator.ValidateAndThrow(dto);
        _logger.LogInformation("Validation of {DTO} is successful", dto);
        var resultTable = _aggregation.Aggregate(dto);
        _logger.LogInformation("Aggregation on database {Database} complete", dto.Address.ToString());
        _logger.LogInformation("Start serializing table {Table}", resultTable);
        var resultJson = JsonConvert.SerializeObject(resultTable);
        _logger.LogInformation(
            "Table {Table} is serialized and result of serializing is {Json}" , resultTable , resultJson);
        _logger.LogInformation("{MethodName} action method from {ControllerName} is finished"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        return Ok(resultJson);
    }
}