namespace Helpers;

public static class EnumerableHelper<T>
{
    public static T? GetMostRepeatedItem(IEnumerable<T> enumerable)
    {
        return enumerable.GroupBy(card => card).MaxBy(card => card.Count())!.FirstOrDefault();
    }

    /// <summary>
    /// Clones a list of ICloneable items. Your items should implement ICloneable.
    /// </summary>
    /// <param name="list">List of cloneable items.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A cloned list.</returns>
    public static List<ICloneable> CloneList(List<ICloneable> list)
    {
        if (list == null)
        {
            throw new ArgumentNullException();
        }
        
        var newList = new List<ICloneable>(list.Count);

        list.ForEach((item) =>
        {
            newList.Add((ICloneable)item.Clone());
        });

        return newList;
    }
}