using BetterETLProject.DTO;


namespace BetterETLProject.Extract.ImportTableAdaptor;

public static class ImportTableFactory
{
    public static IImporterTable CreateImporterTable(ImportDataDto dto)
    {
        if (dto.FilePath.Type.Equals("CSV" , StringComparison.OrdinalIgnoreCase))
        {
            return new CsvImporterTable(new StreamReader(dto.FilePath.ToString()));
        }

        return null!;
    }
}