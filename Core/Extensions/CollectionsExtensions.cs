using System.Collections.ObjectModel;

namespace Core.Extensions;

public static class CollectionsExtensions
{
    public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        var readOnlyCollection = Array.AsReadOnly(array);
        return readOnlyCollection;
    }
}