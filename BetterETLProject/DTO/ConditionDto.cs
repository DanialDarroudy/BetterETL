using BetterETLProject.Connection;
using BetterETLProject.Sources;

namespace BetterETLProject.DTO;

public class ConditionDto
{
    public string Condition { get; set; } = null!;
    public string TableName { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
    public string Limit{ get; set; } = null!;

    public ConditionDto(string condition, string tableName, ConnectionSetting address , string limit)
    {
        Condition = condition;
        TableName = tableName;
        Address = address;
        Limit = limit;
    }

    public ConditionDto() {}
}