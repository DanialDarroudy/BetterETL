using System.Data;
using BetterETLProject.DTO;

namespace BetterETLProject.Transform;

public interface IAggregation
{
    public DataTable Aggregate(AggregationDto dto);
}