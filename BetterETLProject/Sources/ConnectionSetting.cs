using BetterETLProject.Validation;

namespace BetterETLProject.Sources;

public class ConnectionSetting : ISource
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string DataBase { get; set; }

    public ConnectionSetting(string host, string userName, string passWord, string dataBase)
    {
        Host = host;
        UserName = userName;
        PassWord = passWord;
        DataBase = dataBase;
    }

    public new string ToString()
    {
        Validator.CheckNull(Host);
        Validator.CheckNull(UserName);
        Validator.CheckNull(PassWord);
        Validator.CheckNull(DataBase);
        return $"Host={Host};Username={UserName};Password={PassWord};Database={DataBase}";
    }
}