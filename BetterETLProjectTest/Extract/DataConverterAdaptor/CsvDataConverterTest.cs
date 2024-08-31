using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.Create;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Sources;
using NSubstitute;

namespace BetterETLProjectTest.Extract.DataConverterAdaptor;

public class CsvDataConverterTest
{
    [Fact]
    public void Convert_ShouldCallWithSpecificParameter_WhenCalled()
    {
        // Arrange
        var dto = new ImportDataDto(new FilePath("Students" , "csv") , new ConnectionSetting());
        var columnNames = new List<string>() { "FirstName", "Average" };
        var creatorConnection = Substitute.For<ICreatorConnection>();
        var dataBaseHelper = Substitute.For<ICreatorTable>();
        var connection = Substitute.For<IDbConnection>();
        dataBaseHelper.GetColumnNames(dto.FilePath).Returns(columnNames);
        creatorConnection.CreateConnection(dto.Address).Returns(connection);
        var converter = new CsvDataConverter(creatorConnection, dataBaseHelper);
        
        // Act
        converter.Convert(dto);
        
        // Assert
        dataBaseHelper.Received(1).CreateTable(
            Arg.Is<string>(query => query.Contains("CREATE TABLE")),
            Arg.Is<IDbConnection>(conn => conn == connection)
        );
        dataBaseHelper.Received(1).ImportDataToTable(
            Arg.Is<string>(query => query.Contains("COPY")),
            dto.FilePath,
            Arg.Is<IDbConnection>(conn => conn == connection)
        );
    }
}