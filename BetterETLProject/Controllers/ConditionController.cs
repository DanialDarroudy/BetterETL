using BetterETLProject.DTO;
using BetterETLProject.Transform;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;

public class ConditionController : Controller
{
    private readonly ILogger<ConditionController> _logger;
    private readonly AbstractValidator<ConditionDto> _validator;
    private readonly ICondition _condition;

    public ConditionController(ILogger<ConditionController> logger, AbstractValidator<ConditionDto> validator
        , ICondition condition)
    {
        _logger = logger;
        _validator = validator;
        _condition = condition;
    }

    [HttpPost]
    public IActionResult ApplyCondition([FromBody] ConditionDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ControllerName}",
            ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        _logger.LogInformation("Validating {DTO}", dto);
        _validator.ValidateAndThrow(dto);
        _logger.LogInformation("Validation of {DTO} is successful", dto);
        var resultTable = _condition.PerformFilter(dto);
        _logger.LogInformation("Applied conditions on database {Database}", dto.Address.ToString());
        _logger.LogInformation("Start serializing table {Table}", resultTable);
        var resultJson = JsonConvert.SerializeObject(resultTable);
        _logger.LogInformation(
            "Table {Table} is serialized and result of serializing is {Json}" , resultTable , resultJson);
        _logger.LogInformation("{MethodName} action method from {ControllerName} is finished"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        return Ok(resultJson);
    }
}