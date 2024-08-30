using BetterETLProject.DTO;

namespace BetterETLProject.Extract.DataConverterAdaptor;

public interface IDataConverter
{
    public void Convert(ImportDataDto dto);
}