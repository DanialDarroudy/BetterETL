using System.Reflection;
using BetterETLProject.DTO;
using BetterETLProject.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;

namespace BetterETLProject.Extract.CreateTableAdaptor;

public static class CreateTableFactory
{
    public static ICreatorTable CreateCreatorTable(ImportDataDto dto)
    {
        if (dto.FilePath.Type.Equals("CSV" , StringComparison.OrdinalIgnoreCase))
        {
            return new CsvCreatorTable(new StreamReader(dto.FilePath.ToString()) , new NpgsqlCommand());
        }

        return null!;
    }
}