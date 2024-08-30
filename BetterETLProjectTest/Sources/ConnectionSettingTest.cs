using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.Sources;

public class ConnectionSettingTest
{
    [Theory]
    [InlineData("Host=localhost;Username=postgres;Password=!@#123qwe;Database=ETL"
        , "localhost", "postgres", "!@#123qwe", "ETL")]
    [InlineData("Host=localhost;Username=postgres;Password=!@#123qwe;Database=DIA"
        , "localhost", "postgres", "!@#123qwe", "DIA")]
    public void ToString_ShouldReturnCorrectFilePath_WhenInvoked(string expected, string host, string userName
        , string password, string dataBase)
    {
        // Arrange
        var address = new ConnectionSetting(host, userName, password, dataBase);
        // Act
        var actual = address.ToString();
        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null!, "postgres" , "!@#123qwe" , "DIA")]
    [InlineData("localhost", null! , "!@#123qwe" , "DIA")]
    [InlineData("localhost", "postgres" , null! , "DIA")]
    [InlineData("localhost", "postgres" , "!@#123qwe" , null!)]
    [InlineData(null!, null! , null! , null!)]
    public void ToString_ShouldThrowArgumentException_WhenTableNameOrTypeAreNull(string host, string userName
        , string password, string dataBase)
    {
        // Arrange
        var address = new ConnectionSetting(host, userName, password, dataBase);
        // Act
        var action = () => address.ToString();
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}