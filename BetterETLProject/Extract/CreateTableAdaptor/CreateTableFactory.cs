using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public static class CreateTableFactory
{
    public static ICreatorTable CreateCreatorTable(ImportDataDto dto)
    {
        Validator.CheckNull(dto);
        var creatorName = $"{typeof(ICreatorTable).Namespace}.{dto.FilePath.Type}CreatorTable";
        var creatorType = Assembly.GetExecutingAssembly().GetType(creatorName, false, true)!;
        Validator.CheckTypeIsNull(creatorType, dto.FilePath.Type);
        Validator.CheckTypeCanCastToParent(creatorType, typeof(ICreatorTable));
        var creatorConstructor = creatorType.GetConstructors().FirstOrDefault()!;
        Validator.CheckConstructorIsNull(creatorConstructor, creatorName);
        return (ICreatorTable)creatorConstructor.Invoke(
            [new StreamReader(dto.FilePath.ToString()) , new NpgsqlCommand()]);
    }
}