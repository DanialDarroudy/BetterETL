using BetterETLProject.Validation;

namespace BetterETLProject.Sources;

public class FilePath : ISource
{
    public string TableName{ get; set; } = null!;
    public string Type{ get; set; } = null!;

    public FilePath(string tableName, string type)
    {
        TableName = tableName;
        Type = type;
    }
    public FilePath(){}

    public new string ToString()
    {
        Validator.CheckNull(TableName);
        Validator.CheckNull(Type);
        return $"{TableName}.{Type}";
    }
}