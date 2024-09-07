﻿using BetterETLProject.Connection;
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
        Validator.CheckNull(dto);
        if (dto.FilePath.Type.Equals("CSV", StringComparison.OrdinalIgnoreCase))
        {
            return new CsvDataConverter(new CreatorConnection(dto.Address)
                , CreateTableFactory.CreateCreatorTable(dto), ImportTableFactory.CreateImporterTable(dto));
        }
        return Validator.InvalidType(dto.FilePath.Type);
    }
}