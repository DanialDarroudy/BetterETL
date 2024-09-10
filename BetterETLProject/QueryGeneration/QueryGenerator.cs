using System.Text;
using BetterETLProject.DTO;
using BetterETLProject.Sources;
using BetterETLProject.Validation;

namespace BetterETLProject.QueryGeneration;

public static class QueryGenerator
{
    public static string GenerateCopyQuery(FilePath inputSource, List<string> columnNames)
    {
        return $"COPY {inputSource.TableName}({string.Join(",", columnNames)})" +
               $" FROM STDIN (FORMAT {inputSource.Type.ToUpper()})";
    }

    public static string GenerateCreateTableQuery(string tableName, List<string> columnNames)
    {
        var createTableQuery = new StringBuilder().Append("CREATE TABLE").Append(' ')
            .Append(tableName).Append(" (");

        for (var i = 0; i < columnNames.Count; i++)
        {
            createTableQuery.Append($"{columnNames[i]} VARCHAR(100)");

            if (i < columnNames.Count - 1)
                createTableQuery.Append(", ");
        }

        createTableQuery.Append(");");
        return createTableQuery.ToString();
    }

    public static string GenerateAggregateQuery(AggregationDto dto)
    {
        var groupedBy = string.Join(",", dto.GroupedByColumnNames);

        return
            $"SELECT {groupedBy}, {dto.AggregateType}({dto.AggregatedColumnName}::numeric) AS " +
            $"{dto.AggregatedColumnName}_result " + $"FROM {dto.TableName} " + $"GROUP BY {groupedBy} " +
            $"LIMIT {dto.Limit} ";
    }

    public static string GenerateApplyConditionQuery(ConditionDto dto)
    {
        return $"SELECT * FROM {dto.TableName} " +
               $"WHERE {dto.Condition} " +
               $"LIMIT {dto.Limit} ";
    }
}