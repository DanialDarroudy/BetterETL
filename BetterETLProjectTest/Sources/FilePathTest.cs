using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.Sources;

public class FilePathTest
{
    [Theory]
    [InlineData("Student.csv", "Student", "csv")]
    [InlineData("Customer.txt", "Customer", "txt")]
    [InlineData("BigData.word", "BigData", "word")]
    public void ToString_ShouldReturnCorrectFilePath_WhenInvoked(string expected, string tableName, string type)
    {
        // Arrange
        var filePath = new FilePath(tableName, type);
        // Act
        var actual = filePath.ToString();
        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null!, "csv")]
    [InlineData("Students", null!)]
    [InlineData(null!, null!)]
    public void ToString_ShouldThrowArgumentException_WhenTableNameOrTypeAreNull(string tableName,
        string type)
    {
        // Arrange
        var filePath = new FilePath(tableName, type);
        // Act
        var action = () => filePath.ToString();
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}