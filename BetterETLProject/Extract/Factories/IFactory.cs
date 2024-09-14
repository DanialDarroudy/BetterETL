using BetterETLProject.DTO;

namespace BetterETLProject.Extract.Factories;

public interface IFactory<out T>
{
    public T Create(ImportDataDto dto);
}