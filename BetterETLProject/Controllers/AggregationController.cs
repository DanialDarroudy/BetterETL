using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Transform;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;

namespace BetterETLProject.Controllers;

public class AggregationController : Controller
{
    [HttpPost]
    public IActionResult Aggregate([FromBody] AggregationDto dto)
    {
        var resultTable = new Aggregation(new CreatorConnection(new NpgsqlConnection())
            , new NpgsqlCommand() , new NpgsqlDataAdapter()).Aggregate(dto);
        return Ok(JsonConvert.SerializeObject(resultTable));
    }
}