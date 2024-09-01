using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Transform;
using FluentAssertions;
using NSubstitute;

namespace BetterETLProjectTest.Transform;

public class AggregationTest
{
    [Fact]
    public void Aggregate_ShouldReturnAnswerTable_WhenDtoIsNotNull()
    {
        // Arrange
        var mockCreator = Substitute.For<ICreatorConnection>();
        var mockCommand = Substitute.For<IDbCommand>();
        var mockAdaptor = Substitute.For<IDbDataAdapter>();
        var aggregation = new Aggregation(mockCreator , mockCommand , mockAdaptor);
        
        // Act

        // Assert

    }
    [Fact]
    public void Aggregate_ShouldThrowArgumentException_WhenDtoIsNull()
    {
        // Arrange
        var mockCreator = Substitute.For<ICreatorConnection>();
        var mockCommand = Substitute.For<IDbCommand>();
        var mockAdaptor = Substitute.For<IDbDataAdapter>();
        var aggregation = new Aggregation(mockCreator , mockCommand , mockAdaptor);
        AggregationDto dto = null!;
        // Act
        var action = () => aggregation.Aggregate(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}