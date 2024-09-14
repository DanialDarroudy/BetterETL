using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Extract.Factories;
using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.Extract.DataConverterAdaptor;

public class DataConverterFactoryTest
{
    [Fact]
    public void CreateDataConverter_ShouldReturnDataConverter_WhenTypeIsValidAndCanBeCastToDataConverter()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Csv");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var actual = DataConverterFactory.CreateDataConverter(dto);
        // Assert
        actual.Should().BeOfType<CsvDataConverter>();
    }
    
    [Fact]
    public void CreateDataConverter_ShouldThrowArgumentException_WhenTypeIsInvalid()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Txt");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var action = () => DataConverterFactory.CreateDataConverter(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"Unsupported data converter type: {dto.FilePath.Type}");
    }
}