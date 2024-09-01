using System.Reflection;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public static class DataConverterFactory
{
    public static IDataConverter CreateDataConverter(ImportDataDto dto)
    {
        var converterName = $"{typeof(IDataConverter).Namespace}.{dto.FilePath.Type}DataConverter";
        var converterType = Assembly.GetExecutingAssembly().GetType(converterName, false, true)!;
        Validator.CheckTypeIsNull(converterType, dto.FilePath.Type);
        Validator.CheckTypeCanCastToParent(converterType, typeof(IDataConverter));
        var converterConstructor = converterType.GetConstructors().FirstOrDefault()!;
        Validator.CheckConstructorIsNull(converterConstructor , converterName);
        var converter = (IDataConverter)converterConstructor.Invoke([new CreatorConnection(new NpgsqlConnection())
            , CreateTableFactory.CreateCreatorTable(dto), ImportTableFactory.CreateImporterTable(dto)]);
        return converter;
    }
}