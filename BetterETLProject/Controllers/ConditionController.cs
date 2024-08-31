﻿using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Transform;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;

namespace BetterETLProject.Controllers;

public class ConditionController : Controller
{
    [HttpGet]
    public IActionResult ApplyCondition([FromBody] ConditionDto dto)
    {
        var resultTable = new Condition(new CreatorConnection(new NpgsqlConnection())).PerformFilter(dto);
        return Ok(JsonConvert.SerializeObject(resultTable));
    }
}