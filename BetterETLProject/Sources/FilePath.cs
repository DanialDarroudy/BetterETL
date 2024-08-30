using BetterETLProject.Validation;

namespace BetterETLProject.Sources;

public class FilePath : ISource
{
    public string TableName{ get; set; }
    public string Type{ get; set; }

    public FilePath(string tableName, string type)
    {
        TableName = tableName;
        Type = type;
    }

    public new string ToString()
    {
        Validator.CheckNull(TableName);
        Validator.CheckNull(Type);
        return $"{TableName}.{Type}";
    }
}