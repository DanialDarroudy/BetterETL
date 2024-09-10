using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Transform;
using BetterETLProject.Validation.DTOValidation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;

namespace BetterETLProject.Controllers;

public class ConditionController : Controller
{
    [HttpPost]
    public IActionResult ApplyCondition([FromBody] ConditionDto dto)
    {
        new ConditionDtoValidator().ValidateAndThrow(dto);
        var resultTable = new Condition(new CreatorConnection(dto.Address)
            , new NpgsqlCommand() , new NpgsqlDataAdapter()).PerformFilter(dto);
        return Ok(JsonConvert.SerializeObject(resultTable));
    }
}