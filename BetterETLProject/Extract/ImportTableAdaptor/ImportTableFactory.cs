using BetterETLProject.DTO;
using BetterETLProject.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace BetterETLProject.Extract.ImportTableAdaptor;

public static class ImportTableFactory
{
    public static IImporterTable CreateImporterTable(ImportDataDto dto)
    {
        if (dto.FilePath.Type.Equals("CSV" , StringComparison.OrdinalIgnoreCase))
        {
            return new CsvImporterTable(new StreamReader(dto.FilePath.ToString()));
        }
        throw new UnsupportedContentTypeException($"Unsupported importer table type: {dto.FilePath.Type}");
    }
}