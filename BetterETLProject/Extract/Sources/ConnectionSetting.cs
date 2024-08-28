namespace BetterETLProject.Extract.Sources;

public class ConnectionSetting
{
    public string Host { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PassWord { get; set; } = null!;
    public string DataBase { get; set; } = null!;

    public override string ToString()
    {
        return $"Host={Host};Username={UserName};Password={PassWord};Database={DataBase}";
    }
}