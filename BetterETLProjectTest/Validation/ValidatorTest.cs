using BetterETLProject.DTO;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Sources;
using BetterETLProject.Transform;
using BetterETLProject.Validation;
using FluentAssertions;

namespace BetterETLProjectTest.Validation;

public class ValidatorTest
{
    [Fact]
    public void CheckTypeIsNull_ShouldThrowArgumentException_WhenTypeIsNull()
    {
        // Arrange
        Type type = null!;
        const string typeName = "TxtDataConverter";
        // Act
        var action = () => Validator.CheckTypeIsNull(type, typeName);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"Unsupported data converter type: {typeName}");
    }

    [Fact]
    public void CheckTypeIsNull_ShouldNotThrowArgumentException_WhenTypeIsNotNull()
    {
        // Arrange
        var type = typeof(CsvDataConverter);
        const string typeName = "CsvDataConverter";
        // Act
        var action = () => Validator.CheckTypeIsNull(type, typeName);
        // Assert
        action.Should().NotThrow<ArgumentException>();
    }

    [Fact]
    public void CheckTypeCanCastToParent_ShouldThrowArgumentException_WhenTypeCantBeCastToParent()
    {
        // Arrange
        var child = typeof(Aggregation);
        var parent = typeof(IDataConverter);
        // Act
        var action = () => Validator.CheckTypeCanCastToParent(child, parent);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"{child} does not implement {parent} interface.");
    }

    [Fact]
    public void CheckTypeCanCastToParent_ShouldNotThrowArgumentException_WhenTypeCanBeCastToParent()
    {
        // Arrange
        var child = typeof(CsvDataConverter);
        var parent = typeof(IDataConverter);
        // Act
        var action = () => Validator.CheckTypeCanCastToParent(child, parent);
        // Assert
        action.Should().NotThrow<ArgumentException>();
    }

    [Theory]
    [MemberData(nameof(ProvideEmptyList))]
    public void CheckListIsEmpty_ShouldThrowArgumentException_WhenListIsEmpty<T>(List<T> elements)
    {
        // Arrange
        
        // Act
        var action = () => Validator.CheckListIsEmpty(elements);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            $"The input List Of {typeof(T)} cannot be empty.");
    }

    public static IEnumerable<object[]> ProvideEmptyList()
    {
        yield return [new List<IDataConverter>()];
        yield return [new List<string>()];
        yield return [new List<ISource>()];
    }

    [Theory]
    [MemberData(nameof(ProvideNotEmptyList))]
    public void CheckListIsEmpty_ShouldNotThrowArgumentException_WhenListNotEmpty<T>(List<T> elements)
    {
        // Arrange
        
        // Act
        var action = () => Validator.CheckListIsEmpty(elements);
        // Assert
        action.Should().NotThrow<ArgumentException>();
    }
    public static IEnumerable<object[]> ProvideNotEmptyList()
    {
        yield return [new List<IDataConverter>(){new CsvDataConverter()}];
        yield return [new List<string>(){"Age" , "Name"}];
        yield return [new List<ISource>(){null!}];
    }

    [Theory]
    [MemberData(nameof(ProvideNullObject))]
    public void CheckNull_ShouldThrowArgumentException_WhenParameterIsNull<T>(T input)
    {
        // Arrange
        
        // Act
        var action = () => Validator.CheckNull(input);
        // Assert
        action.Should().Throw<ArgumentException>().WithMessage(
            "The parameter of the method cannot be null.");
    }

    public static IEnumerable<object[]> ProvideNullObject()
    {
        ISource source = null!;
        yield return [source];
        AggregationDto aggDto = null!;
        yield return [aggDto];
        ConditionDto conDto = null!;
        yield return [conDto];
        string userName = null!;
        yield return [userName];
    }

    [Theory]
    [MemberData(nameof(ProvideNotNullObject))]
    public void CheckNull_ShouldNotThrowArgumentException_WhenParameterIsNotNull<T>(T input)
    {
        // Arrange
        
        // Act
        var action = () => Validator.CheckNull(input);
        // Assert
        action.Should().NotThrow<ArgumentException>();
    }
    public static IEnumerable<object[]> ProvideNotNullObject()
    {
        ISource address = new ConnectionSetting(null! , null! , null! , null!);
        yield return [address];
        ISource path = new FilePath(null! , null!);
        yield return [path];
        var aggDto = new AggregationDto();
        yield return [aggDto];
        var conDto = new ConditionDto();
        yield return [conDto];
        const string userName = "postgres";
        yield return [userName];
    }
}