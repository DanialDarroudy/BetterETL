namespace BetterETLProject.Sources;

public class ConnectionSetting : ISource
{
    public string Host { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PassWord { get; set; } = null!;
    public string DataBase { get; set; } = null!;

    public ConnectionSetting(string host, string userName, string passWord, string dataBase)
    {
        Host = host;
        UserName = userName;
        PassWord = passWord;
        DataBase = dataBase;
    }
    public ConnectionSetting(){}

    public new string ToString()
    {
        return $"Host={Host};Username={UserName};Password={PassWord};Database={DataBase}";
    }
}