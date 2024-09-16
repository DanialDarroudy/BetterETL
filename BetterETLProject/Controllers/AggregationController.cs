using BetterETLProject.DTO;
using BetterETLProject.Transform;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;
public class AggregationController : Controller
{
    private readonly ILogger<AggregationController> _logger;
    private readonly IValidator<AggregationDto> _validator;
    private readonly IAggregation _aggregation;

    public AggregationController(ILogger<AggregationController> logger, IValidator<AggregationDto> validator
        , IAggregation aggregation)
    {
        _logger = logger;
        _validator = validator;
        _aggregation = aggregation;
    }

    [HttpPost]
    public IActionResult Aggregate([FromBody] AggregationDto dto)
    {
        _logger.LogInformation("Called {MethodName} action method from {ControllerName} controller"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        _validator.ValidateAndThrow(dto);
        var resultTable = _aggregation.Aggregate(dto);
        var resultJson = JsonConvert.SerializeObject(resultTable);
        _logger.LogInformation("{MethodName} action method from {ControllerName} controller is finished"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        return Ok(resultJson);
    }
}