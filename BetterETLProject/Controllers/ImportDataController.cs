using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Validation.DTOValidation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BetterETLProject.Controllers;

public class ImportDataController : Controller
{
    [HttpPost]
    public void Import([FromBody] ImportDataDto dto)
    {
        new ImportDataDtoValidator().ValidateAndThrow(dto);
        var converter = DataConverterFactory.CreateDataConverter(dto);
        converter.Convert(dto);
    }
}