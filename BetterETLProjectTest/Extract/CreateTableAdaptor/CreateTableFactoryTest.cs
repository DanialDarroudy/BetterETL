using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.Factories;
using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.Extract.CreateTableAdaptor;

public class CreateTableFactoryTest
{
    [Fact]
    public void CreateCreatorTable_ShouldReturnCreatorTable_WhenTypeIsValidAndCanBeCastToDataConverter()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Csv");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var actual = CreateTableFactory.CreateCreatorTable(dto);
        // Assert
        actual.Should().BeOfType<CsvCreatorTable>();
    }
    [Fact]
    public void CreateCreatorTable_ShouldThrowArgumentException_WhenTypeIsInvalid()
    {
        // Arrange
        var filePath = new FilePath("Students" , "Txt");
        var dto = new ImportDataDto(filePath , new ConnectionSetting());
        // Act
        var action = () => CreateTableFactory.CreateCreatorTable(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"Unsupported data converter type: {dto.FilePath.Type}");
    }
}