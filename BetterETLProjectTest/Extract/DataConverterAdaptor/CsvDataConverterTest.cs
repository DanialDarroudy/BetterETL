using System.Data;
using BetterETLProject.Connections;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.Sources;
using FluentAssertions;
using NSubstitute;

namespace BetterETLProjectTest.Extract.DataConverterAdaptor;

public class CsvDataConverterTest
{
    [Fact]
    public void Convert_ShouldCallWithSpecificParameter_WhenDtoIsNotNull()
    {
        // Arrange
        var dto = new ImportDataDto(new FilePath("Students", "csv"), new ConnectionSetting());
        var columnNames = new List<string>() { "FirstName", "Average" };
        var creatorConnection = Substitute.For<ICreatorConnection>();
        var creatorTable = Substitute.For<ICreatorTable>();
        var importTable = Substitute.For<IImporterTable>();
        var connection = Substitute.For<IDbConnection>();
        creatorTable.GetColumnNames().Returns(columnNames);
        creatorConnection.CreateConnection(dto.Address).Returns(connection);
        var converter = new CsvDataConverter(creatorConnection, creatorTable, importTable);

        // Act
        converter.Convert(dto);

        // Assert
        creatorTable.Received(1).CreateTable(
            Arg.Is<string>(query => query.Contains("CREATE TABLE")),
            Arg.Is<IDbConnection>(conn => conn == connection)
        );
        importTable.Received(1).ImportDataToTable(
            Arg.Is<string>(query => query.Contains("COPY")),
            Arg.Is<IDbConnection>(conn => conn == connection)
        );
        connection.Received(1).Dispose();
    }

    [Fact]
    public void Convert_ShouldCallWithSpecificParameter_WhenDtoIsNull()
    {
        // Arrange
        ImportDataDto dto = null!;
        var creatorConnection = Substitute.For<ICreatorConnection>();
        var creatorTable = Substitute.For<ICreatorTable>();
        var importTable = Substitute.For<IImporterTable>();
        var converter = new CsvDataConverter(creatorConnection, creatorTable, importTable);
        
        // Act
        var action = () => converter.Convert(dto);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }
}