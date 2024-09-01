using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Sources;
using BetterETLProject.Transform;
using FluentAssertions;
using NSubstitute;

namespace BetterETLProjectTest.Transform;

public class AggregationTest
{
    [Theory]
    [MemberData(nameof(ProvideDto))]
    public void Aggregate_ShouldReturnAnswerTable_WhenDtoIsNotNull(DataTable expected, AggregationDto dto)
    {
        // Arrange
        var mockCreator = Substitute.For<ICreatorConnection>();
        var mockCommand = Substitute.For<IDbCommand>();
        var mockAdaptor = Substitute.For<IDbDataAdapter>();
        var mockConnection = Substitute.For<IDbConnection>();
        var aggregation = new Aggregation(mockCreator, mockCommand, mockAdaptor);
        
        mockCreator.CreateConnection(dto.Address).Returns(mockConnection);
        mockAdaptor.Fill(Arg.Any<DataSet>()).Returns(x =>
        {
            var dataSet = (DataSet)x[0];
            dataSet.Tables.Add(expected);
            return expected.Rows.Count;
        });
        
        // Act
        var actual = aggregation.Aggregate(dto);
        
        // Assert
        mockCommand.CommandText.Should().Contain("GROUP BY");
        mockCommand.Connection.Should().Be(mockConnection);
        mockAdaptor.SelectCommand.Should().Be(mockCommand);
        mockCommand.Received(1).Dispose();
        mockConnection.Received(1).Dispose();
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> ProvideDto()
    {
        var firstDto = new AggregationDto("Students", ["Age", "FirstName"],
            "Average", "Max", new ConnectionSetting());
        var firstExpected = new DataTable();
        firstExpected.Columns.Add("Age");
        firstExpected.Columns.Add("FirstName");
        firstExpected.Columns.Add("Average");
        firstExpected.Rows.Add(15 , "Ali" , 16);
        yield return [firstExpected, firstDto];
    }

    [Fact]
    public void Aggregate_ShouldThrowArgumentException_WhenDtoIsNull()
    {
        // Arrange
        var mockCreator = Substitute.For<ICreatorConnection>();
        var mockCommand = Substitute.For<IDbCommand>();
        var mockAdaptor = Substitute.For<IDbDataAdapter>();
        var aggregation = new Aggregation(mockCreator, mockCommand, mockAdaptor);
        AggregationDto dto = null!;
        // Act
        var action = () => aggregation.Aggregate(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}