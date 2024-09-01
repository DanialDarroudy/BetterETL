using System.Data;
using BetterETLProject.Extract.ImportTableAdaptor;
using NSubstitute;

namespace BetterETLProjectTest.Extract.ImportTableAdaptor;

public class CsvImporterTableTest
{
    [Fact]
    public void ImportDataToTable_ShouldNotProcess_WhenConnectionIsNotNpgsql()
    {
        // Arrange
        const string query = "COPY";
        var mockConnection = Substitute.For<IDbConnection>();
        var mockStreamReader = Substitute.For<StreamReader>(new MemoryStream());
        var csvImporterTable = new CsvImporterTable(mockStreamReader);
        // Act
        csvImporterTable.ImportDataToTable(query, mockConnection);
        // Assert
        mockStreamReader.Received(1).ReadLine();
        mockStreamReader.DidNotReceive().Dispose();
    }
}