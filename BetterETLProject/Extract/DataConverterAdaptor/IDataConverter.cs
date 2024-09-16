using BetterETLProject.DTO;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public interface IDataConverter
{
    public Task Convert(ImportDataDto dto);
}