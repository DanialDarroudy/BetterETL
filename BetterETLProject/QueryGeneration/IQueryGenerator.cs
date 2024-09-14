using BetterETLProject.DTO;
using BetterETLProject.Sources;

namespace BetterETLProject.QueryGeneration;

public interface IQueryGenerator
{
    public string GenerateCopyQuery(FilePath inputSource, List<string> columnNames);
    public string GenerateCreateTableQuery(string tableName, List<string> columnNames);
    public string GenerateAggregateQuery(AggregationDto dto);
    public string GenerateApplyConditionQuery(ConditionDto dto);
}