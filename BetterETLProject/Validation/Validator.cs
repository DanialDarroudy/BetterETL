using System.Reflection;

namespace BetterETLProject.Validation;

public static class Validator
{
    public static dynamic InvalidType(string typeName)
    {
        throw new ArgumentException($"Unsupported data converter type: {typeName}");
    }

    public static void CheckTypeCanCastToParent(Type child, Type parent)
    {
        if (!parent.IsAssignableFrom(child))
        {
            throw new ArgumentException($"{child} does not implement {parent} interface.");
        }
    }

    public static void CheckListIsEmpty<T>(List<T> elements)
    {
        if (elements.Count == 0)
        {
            throw new ArgumentException($"The input List Of {typeof(T)} cannot be empty.");
        }
    }

    public static void CheckNull<T>(T input)
    {
        if (input == null)
        {
            throw new ArgumentException("The parameter of the method cannot be null.");
        }
    }

    public static void CheckConstructorIsNull(ConstructorInfo constructor, string className)
    {
        if (constructor == null)
        {
            throw new ArgumentException($"No public constructor found for {className}");
        }
    }
}