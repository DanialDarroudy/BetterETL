using BetterETLProject.Sources;

namespace BetterETLProject.DTO;

public class ConditionDto
{
    public string Condition { get; set; } = null!;
    public string TableName { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;

    public ConditionDto(string condition, string tableName, ConnectionSetting address)
    {
        Condition = condition;
        TableName = tableName;
        Address = address;  
    }

    public ConditionDto() {}
}