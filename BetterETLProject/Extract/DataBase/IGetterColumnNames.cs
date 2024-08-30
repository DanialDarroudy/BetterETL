using BetterETLProject.Sources;

namespace BetterETLProject.Extract.DataBase;

public interface IGetterColumnNames
{
    public List<string> GetColumnNames(FilePath filePath);
}