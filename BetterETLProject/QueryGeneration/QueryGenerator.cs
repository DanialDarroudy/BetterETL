using System.Text;
using BetterETLProject.DTO;
using BetterETLProject.Sources;

namespace BetterETLProject.QueryGeneration;

public class QueryGenerator : IQueryGenerator
{
    private readonly ILogger<QueryGenerator> _logger;

    public QueryGenerator(ILogger<QueryGenerator> logger)
    {
        _logger = logger;
    }

    public string GenerateCopyQuery(FilePath inputSource, List<string> columnNames)
    {
        var result =  $"COPY {inputSource.TableName}({string.Join(",", columnNames)})" +
               $" FROM STDIN (FORMAT {inputSource.Type.ToUpper()})";
        _logger.LogInformation("be generated this query : {Query}", result);
        return result;
    }

    public string GenerateCreateTableQuery(string tableName, List<string> columnNames)
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
        var result = createTableQuery.ToString();
        _logger.LogInformation("be generated this query : {Query}", result);
        return result;
    }

    public string GenerateAggregateQuery(AggregationDto dto)
    {
        var groupedBy = string.Join(",", dto.GroupedByColumnNames);
        var result =
            $"SELECT {groupedBy}, {dto.AggregateType}({dto.AggregatedColumnName}::numeric) AS " +
            $"{dto.AggregatedColumnName}_result " + $"FROM {dto.TableName} " + $"GROUP BY {groupedBy} " +
            $"LIMIT {dto.Limit} ";
        _logger.LogInformation("be generated this query : {Query}", result);
        return result;
    }

    public string GenerateApplyConditionQuery(ConditionDto dto)
    {
        var result =
            $"SELECT * FROM {dto.TableName} " +
            $"WHERE {dto.Condition} " +
            $"LIMIT {dto.Limit} ";
        _logger.LogInformation("be generated this query : {Query}", result);
        return result;
    }
}