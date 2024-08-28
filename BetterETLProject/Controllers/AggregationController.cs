using BetterETLProject.DTO;
using BetterETLProject.Transform;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BetterETLProject.Controllers;

public class AggregationController : Controller
{
    [HttpGet]
    public IActionResult Aggregate([FromBody] AggregationDto dto)
    {
        var resultTable = new Aggregation().Aggregate(dto);
        return Ok(JsonConvert.SerializeObject(resultTable));
    }
}