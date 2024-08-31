using System.Data;
using BetterETLProject.Connection;
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
        actual.State.Should().Be(ConnectionState.Open);
    }
}