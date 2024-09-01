using BetterETLProject.DTO;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.Extract.ImportTableAdaptor;

public class ImportTableFactoryTest
{
    [Fact]
    public void CreateDataConverter_ShouldReturnDataConverter_WhenTypeIsValidAndCanBeCastToDataConverter()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Csv");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var actual = ImportTableFactory.CreateImporterTable(dto);
        // Assert
        actual.Should().BeOfType<CsvImporterTable>();
    }
    
    [Fact]
    public void CreateDataConverter_ShouldThrowArgumentException_WhenTypeIsInvalid()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Txt");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var action = () => ImportTableFactory.CreateImporterTable(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"Unsupported data converter type: {dto.FilePath.Type}");
    }
}