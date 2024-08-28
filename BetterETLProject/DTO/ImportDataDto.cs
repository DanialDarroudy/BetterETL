using BetterETLProject.Extract.Sources;

namespace BetterETLProject.DTO;

public class ImportDataDto
{
    public PathFile Path { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
}