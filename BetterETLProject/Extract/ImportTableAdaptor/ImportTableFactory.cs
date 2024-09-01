using System.Globalization;
using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Validation;
using CsvHelper;
using CsvHelper.Configuration;

namespace BetterETLProject.Extract.ImportTableAdaptor;

public static class ImportTableFactory
{
    public static IImporterTable CreateImporterTable(ImportDataDto dto)
    {
        var importerName = $"{typeof(IImporterTable).Namespace}.{dto.FilePath.Type}ImporterTable";
        var importerType = Assembly.GetExecutingAssembly().GetType(importerName, false, true)!;
        Validator.CheckTypeIsNull(importerType, dto.FilePath.Type);
        Validator.CheckTypeCanCastToParent(importerType, typeof(IImporterTable));
        var importerConstructor = importerType.GetConstructors().FirstOrDefault()!;
        Validator.CheckConstructorIsNull(importerConstructor, importerName);
        return (IImporterTable)importerConstructor.Invoke(
        [new StreamReader(dto.FilePath.ToString())]);
    }
}