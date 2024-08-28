using BetterETLProject.Extract.Sources;

namespace BetterETLProject.DTO;

public class ConditionDto
{
    public string Condition { get; set; } = null!;
    public string TableName { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
}