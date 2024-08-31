using BetterETLProject.DTO;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Sources;
using FluentAssertions;

namespace BetterETLProjectTest.QueryGeneration;

public class QueryGeneratorTest
{
    [Theory]
    [MemberData(nameof(ProvideCopyQueries))]
    public void GenerateCopyQuery_ShouldReturnCopyQuery_WhenParametersDoesNotNullOrEmpty(string expected
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
        const string firstExpected = "COPY Students(FirstName,Age,Average)" +
                                     " FROM STDIN (FORMAT CSV)";
        yield return [firstExpected, firstFilePath, firstColumnNames];

        var secondFilePath = new FilePath("Customer", "txt");
        var secondColumnNames = new List<string> { "Name" };
        const string secondExpected = "COPY Customer(Name)" +
                                      " FROM STDIN (FORMAT TXT)";
        yield return [secondExpected, secondFilePath, secondColumnNames];
    }

    [Fact]
    public void GenerateCopyQuery_ShouldThrowArgumentException_WhenInputSourceIsNull()
    {
        // Arrange
        FilePath inputSource = null!;
        var columnNames = new List<string> { "FirstName", "Age", "Average" };
        // Act
        var action = () => QueryGenerator.GenerateCopyQuery(inputSource, columnNames);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }

    [Fact]
    public void GenerateCopyQuery_ShouldThrowArgumentException_WhenColumnNamesIsEmpty()
    {
        // Arrange
        var inputSource = new FilePath("Customer", "csv");
        var columnNames = new List<string>();
        // Act
        var action = () => QueryGenerator.GenerateCopyQuery(inputSource, columnNames);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"The input List Of {typeof(string)} cannot be empty.");
    }

    [Theory]
    [MemberData(nameof(ProvideCreateTableQueries))]
    public void GenerateCreateTableQuery_ShouldReturnCreateTableQuery_WhenColumnNamesIsNotEmpty(string expected
        , string tableName, List<string> columnNames)
    {
        // Arrange

        // Act
        var actual = QueryGenerator.GenerateCreateTableQuery(tableName, columnNames);
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> ProvideCreateTableQueries()
    {
        const string firstExpected =
            "CREATE TABLE Students (FirstName VARCHAR(100), Age VARCHAR(100), Average VARCHAR(100));";
        const string firstTableName = "Students";
        var firstColumnNames = new List<string> { "FirstName", "Age", "Average" };
        yield return [firstExpected, firstTableName, firstColumnNames];

        const string secondExpected =
            "CREATE TABLE Customers (Price VARCHAR(100));";
        const string secondTableName = "Customers";
        var secondColumnNames = new List<string> { "Price" };
        yield return [secondExpected, secondTableName, secondColumnNames];
    }

    [Fact]
    public void GenerateCreateTableQuery_ShouldThrowArgumentException_WhenColumnNamesIsEmpty()
    {
        // Arrange
        const string tableName = "Customers";
        var columnNames = new List<string>();
        // Act
        var action = () => QueryGenerator.GenerateCreateTableQuery(tableName, columnNames);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"The input List Of {typeof(string)} cannot be empty.");
    }

    [Theory]
    [MemberData(nameof(ProvideAggregateQueries))]
    public void GenerateAggregateQuery_ShouldReturnAggregateQuery_WhenDtoIsNotNull(string expected
        , AggregationDto dto)
    {
        // Arrange

        // Act
        var actual = QueryGenerator.GenerateAggregateQuery(dto);
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> ProvideAggregateQueries()
    {
        const string firstExpected =
            "SELECT FirstName,Age, Sum(Average::numeric) AS Average_result " +
            "FROM Students " +
            "GROUP BY FirstName,Age";
        var firstDto = new AggregationDto("Students", ["FirstName", "Age"]
            , "Average", "Sum", new ConnectionSetting());
        yield return [firstExpected, firstDto];

        const string secondExpected =
            "SELECT Age, Max(Average::numeric) AS Average_result " +
            "FROM Students " +
            "GROUP BY Age";
        var secondDto = new AggregationDto("Students", ["Age"]
            , "Average", "Max", new ConnectionSetting());
        yield return [secondExpected, secondDto];
    }

    [Fact]
    public void GenerateAggregateQuery_ShouldThrowArgumentException_WhenDtoIsNull()
    {
        // Arrange
        AggregationDto dto = null!;
        // Act
        var action = () => QueryGenerator.GenerateAggregateQuery(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }

    [Fact]
    public void GenerateAggregateQuery_ShouldThrowArgumentException_WhenGroupedByColumnNamesIsEmpty()
    {
        // Arrange
        var dto = new AggregationDto("Customers", [], "Price"
            , "Count", new ConnectionSetting());
        // Act
        var action = () => QueryGenerator.GenerateAggregateQuery(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"The input List Of {typeof(string)} cannot be empty.");
    }

    [Theory]
    [MemberData(nameof(ProvideApplyConditionQueries))]
    public void GenerateApplyConditionQuery_ShouldReturnApplyConditionQuery_WhenDtoIsNotNull(string expected
        , ConditionDto dto)
    {
        // Arrange

        // Act
        var actual = QueryGenerator.GenerateApplyConditionQuery(dto);
        // Assert
        actual.Should().Be(expected);
    }

    public static IEnumerable<object[]> ProvideApplyConditionQueries()
    {
        const string firstExpected = $"SELECT * " +
                                     "FROM Students " +
                                     "WHERE Age = 15";
        var firstDto = new ConditionDto("Age = 15", "Students", new ConnectionSetting());
        yield return [firstExpected, firstDto];

        const string secondExpected = $"SELECT * " +
                                      "FROM Customers " +
                                      "WHERE Price > 500 AND Price < 800";
        var secondDto =
            new ConditionDto("Price > 500 AND Price < 800", "Customers", new ConnectionSetting());
        yield return [secondExpected, secondDto];
    }
    [Fact]
    public void GenerateApplyConditionQuery_ShouldThrowArgumentException_WhenDtoIsNull()
    {
        // Arrange
        ConditionDto dto = null!;
        // Act
        var action = () => QueryGenerator.GenerateApplyConditionQuery(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}