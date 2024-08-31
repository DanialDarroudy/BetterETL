using System.Reflection;
using BetterETLProject.Connection;
using BetterETLProject.Extract.Create;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public static class DataConverterFactory
{
    public static IDataConverter CreateDataConverter(string type)
    {
        var converterName = $"{typeof(DataConverterFactory).Namespace}.{type}DataConverter";
        var helperName = $"{typeof(ICreatorTable).Namespace}.{type}DataConverterHelper";
        
        var converterType = Assembly.GetExecutingAssembly().GetType(converterName, false, true)!;
        var helperType = Assembly.GetExecutingAssembly().GetType(helperName, false, true)!;
        
        Validator.CheckTypeIsNull(converterType, type);
        Validator.CheckTypeIsNull(helperType, type);
        
        Validator.CheckTypeCanCastToParent(converterType, typeof(IDataConverter));
        Validator.CheckTypeCanCastToParent(helperType, typeof(ICreatorTable));

        var constructor = converterType.GetConstructors().FirstOrDefault()!;
        Validator.CheckConstructorIsNull(constructor, converterName);

        var converter = (IDataConverter)constructor.Invoke(
            [new CreatorConnection(new NpgsqlConnection()), Activator.CreateInstance(helperType)]);
        return converter;
    }
}