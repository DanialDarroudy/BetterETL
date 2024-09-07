using BetterETLProject.DTO;
using BetterETLProject.Validation;


namespace BetterETLProject.Extract.ImportTableAdaptor;

public static class ImportTableFactory
{
    public static IImporterTable CreateImporterTable(ImportDataDto dto)
    {
        Validator.CheckNull(dto);
        if (dto.FilePath.Type.Equals("CSV" , StringComparison.OrdinalIgnoreCase))
        {
            return new CsvImporterTable(dto);
        }
        return  Validator.InvalidType(dto.FilePath.Type);
    }
}