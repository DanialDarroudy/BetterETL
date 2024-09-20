using System.Data;
using BetterETLProject.Connections;
using BetterETLProject.Sources;
using FluentAssertions;
using NSubstitute;

namespace BetterETLProjectTest.Connection;

public class CreateConnectionTest
{
    [Fact]
    public void CreateConnection_ShouldReturnOpenNpgsqlConnection_WhenAddressIsNotNull()
    {
        // Arrange
        var mockConnection = Substitute.For<IDbConnection>();
        mockConnection.When(x => x.Open())
            .Do(_ => mockConnection.State.Returns(ConnectionState.Open));
        var address =
            new ConnectionSetting("localhost", "postgres", "!@#123qwe", "DIA");
        var creatorConnection = new CreatorConnection(mockConnection);

        // Act
        var actual = creatorConnection.CreateConnection(address);

        // Assert
        actual.Should().NotBeNull();
        actual.Should().BeSameAs(mockConnection);
        actual.ConnectionString.Should().Be(address.ToString());
        actual.State.Should().Be(ConnectionState.Open);
    }

    [Fact]
    public void CreateConnection_ShouldThrowArgumentException_WhenAddressIsNull()
    {
        // Arrange
        var mockConnection = Substitute.For<IDbConnection>();
        var creatorConnection = new CreatorConnection(mockConnection);
        ConnectionSetting address = null!;
        // Act
        var action = () => creatorConnection.CreateConnection(address);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}