namespace BetterETLProject.Extract.Sources;

public class PathFile
{
    public string TableName{ get; set; } = null!;
    public string Type{ get; set; } = null!;

    public override string ToString()
    {
        return $"{TableName}.{Type}";
    }
}