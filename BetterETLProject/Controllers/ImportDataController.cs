using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using Microsoft.AspNetCore.Mvc;

namespace BetterETLProject.Controllers;

public class ImportDataController : Controller
{
    [HttpPost]
    public void Import([FromBody] ImportDataDto dto)
    {
        var converter = DataConverterFactory.CreateDataConverter(dto.FilePath.Type);
        converter.Convert(dto);
    }
}