using BetterETLProject.DTO;
using BetterETLProject.Transform;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;

public class ConditionController : Controller
{
    private readonly ILogger<ConditionController> _logger;
    private readonly IValidator<ConditionDto> _validator;
    private readonly ICondition _condition;

    public ConditionController(ILogger<ConditionController> logger, IValidator<ConditionDto> validator
        , ICondition condition)
    {
        _logger = logger;
        _validator = validator;
        _condition = condition;
    }

    [HttpPost]
    public IActionResult ApplyCondition([FromBody] ConditionDto dto)
    {
        _logger.LogInformation("Called {MethodName} method from {ControllerName} controller",
            ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        _validator.ValidateAndThrow(dto);
        var resultTable = _condition.PerformFilter(dto);
        var resultJson = JsonConvert.SerializeObject(resultTable);
        _logger.LogInformation("{MethodName} action method from {ControllerName} controller is finished"
            , ControllerContext.ActionDescriptor.ActionName, ControllerContext.ActionDescriptor.ControllerName);
        return Ok(resultJson);
    }
}