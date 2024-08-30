using BetterETLProject.QueryGeneration;
using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.QueryGeneration;

public class QueryGeneratorTest
{
    [Theory]
    [MemberData(nameof(ProvideCopyQueries))]
    public void GenerateCopyQuery_ShouldReturnCopyQuery_WhenParametersDoesNotNull(string expected
        , FilePath inputSource, List<string> columnNames)
    {
        // Arrange

        // Act
        var actual = QueryGenerator.GenerateCopyQuery(inputSource, columnNames);
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> ProvideCopyQueries()
    {
        var firstFilePath = new FilePath("Students", "csv");
        var firstColumnNames = new List<string> { "FirstName", "Age", "Average" };
        const string firstExpected = "COPY Students(FirstName,Age,Average) FROM STDIN (FORMAT CSV)";
        yield return [firstExpected, firstFilePath, firstColumnNames];

        var secondFilePath = new FilePath("Customer", "txt");
        var secondColumnNames = new List<string> { "Name" };
        const string secondExpected = "COPY Customer(Name) FROM STDIN (FORMAT TXT)";
        yield return [secondExpected, secondFilePath, secondColumnNames];
    }
}