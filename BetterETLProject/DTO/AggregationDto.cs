using BetterETLProject.Sources;

namespace BetterETLProject.DTO;

public class AggregationDto
{
    public string TableName { get; set; } = null!;
    public List<string> GroupedByColumnNames { get; set; } = null!;
    public string AggregatedColumnName { get; set; } = null!;
    public string AggregateType { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
    public string Limit{ get; set; } = null!;
    public AggregationDto(){}

    public AggregationDto(string tableName, List<string> groupedByColumnNames, string aggregatedColumnName,
        string aggregateType , ConnectionSetting address , string limit)
    {
        TableName = tableName;
        GroupedByColumnNames = groupedByColumnNames;
        AggregatedColumnName = aggregatedColumnName;
        AggregateType = aggregateType;
        Address = address;
        Limit = limit;
    }
}