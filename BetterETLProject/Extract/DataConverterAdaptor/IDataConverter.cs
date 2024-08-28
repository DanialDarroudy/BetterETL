using BetterETLProject.DTO;
using BetterETLProject.Extract.Sources;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public interface IDataConverter
{
    public void Convert(ImportDataDto dto);
}