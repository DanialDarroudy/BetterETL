using BetterETLProject.Extract.Sources;

namespace BetterETLProject.DTO;

public class AggregationDto
{
    public string TableName { get; set; } = null!;
    public List<string> GroupedByColumnNames { get; set; } = null!;
    public string AggregatedColumnName { get; set; } = null!;
    public string AggregateType { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
}