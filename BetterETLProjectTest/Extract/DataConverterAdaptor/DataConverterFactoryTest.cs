using BetterETLProject.Extract.DataConverterAdaptor;
using FluentAssertions;

namespace BetterETLProjectTest.Extract.DataConverterAdaptor;

public class DataConverterFactoryTest
{
    [Fact]
    public void CreateDataConverter_ShouldReturnDataConverter_WhenTypeIsValidAndCanBeCastToDataConverter()
    {
        // Arrange
        const string type = "Csv";
        // Act
        var actual = DataConverterFactory.CreateDataConverter(type);
        // Assert
        actual.Should().BeOfType<CsvDataConverter>();
    }

    [Fact]
    public void CreateDataConverter_ShouldThrowArgumentException_WhenTypeIsInvalid()
    {
        // Arrange
        const string type = "Txt";
        // Act
        var action = () => DataConverterFactory.CreateDataConverter(type);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"Unsupported data converter type: {type}");
    }
}