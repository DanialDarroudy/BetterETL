using System.Data;
using BetterETLProject.Extract.CreateTableAdaptor;
using FluentAssertions;
using NSubstitute;

namespace BetterETLProjectTest.Extract.CreateTableAdaptor;

public class CsvCreatorTableTest
{
    [Theory]
    [MemberData(nameof(ProvideHeaders))]
    public void GetColumnNames_ShouldReturnColumnNames_WhenFilePathIsNotNull(List<string> expected, string csvHeader)
    {
        // Arrange
        var mockStreamReader = Substitute.For<StreamReader>(new MemoryStream());
        var mockCommand = Substitute.For<IDbCommand>();
        mockStreamReader.ReadLine().Returns(csvHeader);
        var csvCreatorTable = new CsvCreatorTable(mockStreamReader, mockCommand);
        // Act
        var actual = csvCreatorTable.GetColumnNames();
        // Assert
        actual.Should().Equal(expected);
        mockStreamReader.Received(1).ReadLine();
        mockStreamReader.Received(1).Dispose();
    }

    public static IEnumerable<object[]> ProvideHeaders()
    {
        const string firstHeader = "Age,FirstName,Average";
        var firstExpected = new List<string> { "Age", "FirstName", "Average" };
        yield return [firstExpected , firstHeader];

        const string secondHeader = "Age";
        var secondExpected = new List<string> { "Age" };
        yield return [secondExpected, secondHeader];
    }

    [Fact]
    public void CreateTable_ShouldCreateTableInPostgresSql_WhenParametersAreProvided()
    {
        // Arrange
        var mockStreamReader = Substitute.For<StreamReader>(new MemoryStream());
        var mockCommand = Substitute.For<IDbCommand>();
        var mockConnection = Substitute.For<IDbConnection>();
        var csvCreatorTable = new CsvCreatorTable(mockStreamReader, mockCommand);
        const string query = "CREATE TABLE";
        // Act
        csvCreatorTable.CreateTable(query , mockConnection);
        // Assert
        mockCommand.CommandText.Should().Be(query);
        mockCommand.Connection.Should().Be(mockConnection);
        mockCommand.Received(1).ExecuteNonQuery();
        mockCommand.Received(1).Dispose();
    }
}