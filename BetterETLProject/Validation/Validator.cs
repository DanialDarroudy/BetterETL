namespace BetterETLProject.Validation;

public static class Validator
{
    public static void CheckTypeIsNull(Type type , string typeName)
    {
        if (type == null)
        {
            throw new ArgumentException($"Unsupported data converter type: {typeName}");
        }
    }

    public static void CheckTypeCanCastToParent(Type child , Type parent)
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
            throw new AggregateException("parameter of the method is null");
        }
    }
}