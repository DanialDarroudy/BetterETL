using System.Data;
using BetterETLProject.DTO;

namespace BetterETLProject.Transform;

public interface ICondition
{
    public DataTable PerformFilter(ConditionDto dto);
}