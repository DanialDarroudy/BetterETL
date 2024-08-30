using BetterETLProject.Sources;

namespace BetterETLProject.DTO;

public class ImportDataDto
{
    public FilePath FilePath { get; set; } = null!;
    public ConnectionSetting Address{ get; set; } = null!;
}