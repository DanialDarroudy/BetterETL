using BetterETLProject.DTO;
using BetterETLProject.Transform;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;

public class ConditionController : Controller
{
    [HttpGet]
    public IActionResult ApplyCondition([FromBody] ConditionDto dto)
    {
        var resultTable = new Condition().PerformFilter(dto);
        return Ok(JsonConvert.SerializeObject(resultTable));
    }
}