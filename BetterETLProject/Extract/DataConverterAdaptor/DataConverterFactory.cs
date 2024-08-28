using System.Reflection;
using BetterETLProject.Extract.Sources;
using BetterETLProject.Validation;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public static class DataConverterFactory
{
    public static IDataConverter CreateDataConverter(string type)
    {
        var className = $"{typeof(DataConverterFactory).Namespace}.{type}DataConverter";
        var converterType = Assembly.GetExecutingAssembly().GetType(className, false, true)!;
        Validator.CheckTypeIsNull(converterType , type);
        Validator.CheckTypeCanCastToParent(converterType , typeof(IDataConverter));
        var converter = (IDataConverter)Activator.CreateInstance(converterType)!;
        return converter;
    }
}