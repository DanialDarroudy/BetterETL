﻿using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Validation;
using Npgsql;

namespace BetterETLProject.Transform;

public class Aggregation 
{
    public DataTable Aggregate(AggregationDto dto)
    {
        Validator.CheckNull(dto);
        var groupedBy = JoinColumns(dto.GroupedByColumnNames);
        var query = QueryGenerator.GenerateAggregateQuery(dto, groupedBy);

        var dataTable = new DataTable();
        using var connection = CreatorConnection.CreateConnection(dto.Address);
        using var command = new NpgsqlCommand(query, connection);
        using var adapter = new NpgsqlDataAdapter(command);
        adapter.Fill(dataTable);
        return dataTable;
    }

    private string JoinColumns(List<string> columns)
    {
        Validator.CheckListIsEmpty(columns);
        return string.Join(", ", columns);
    }
}