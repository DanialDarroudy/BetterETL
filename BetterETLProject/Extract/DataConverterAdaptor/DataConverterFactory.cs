using System.Reflection;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public static class DataConverterFactory
{
    public static IDataConverter CreateDataConverter(ImportDataDto dto)
    {
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            return new CsvDataConverter(new CreatorConnection(new NpgsqlConnection(dto.Address.ToString()))
                , CreateTableFactory.CreateCreatorTable(dto), ImportTableFactory.CreateImporterTable(dto));
        }
        throw new UnsupportedContentTypeException($"Unsupported data converter type: {dto.FilePath.Type}");
    }
}