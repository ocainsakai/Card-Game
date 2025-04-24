using System;
using System.Collections.Generic;
using System.Linq;

public interface IObservableArray<T> {
    event Action<T[]> AnyValueChanged;

    int Count { get; }
    T this[int index] { get; }
    
    void Swap(int index1, int index2);
    void Clear();
    bool TryAdd(T item);
    bool TryAddAt(int index, T item);
    bool TryRemove(T item);
    bool TryRemoveAt(int index);
}

[Serializable]
public class ObservableArray<T> : IObservableArray<T> {
    public T[] items;

    public event Action<T[]> AnyValueChanged = delegate { };
    public int Count => items.Count(i => i != null);
    public int Length => items.Length;
    public T this[int index] => items[index];

    public ObservableArray(int size = 20, IList<T> initialList = null) {
        items = new T[size];
        if (initialList != null) {
            initialList.Take(size).ToArray().CopyTo(items, 0);
            Invoke();
        }
    }
    //int IndexOf(T item)
    //{
    //    return 
    //}
    void Invoke() => AnyValueChanged.Invoke(items);

    public void Swap(int index1, int index2) {
        (items[index1], items[index2]) = (items[index2], items[index1]);
        Invoke();
    }

    public void Clear() {
        items = new T[items.Length];
        Invoke();
    }

    public bool TryAdd(T item) {
        for (var i = 0; i < items.Length; i++) {
            if (TryAddAt(i, item)) return true;
        }
        return false;
    }
    
    public bool TryAddAt(int index, T item) {
        if (index < 0 || index >= items.Length) return false;
        
        if (items[index] != null) return false;

        items[index] = item;
        Invoke();
        return true;
    }

    public bool TryRemove(T item) {
        for (var i = 0; i < items.Length; i++) {
            if (EqualityComparer<T>.Default.Equals(items[i], item) && TryRemoveAt(i)) return true;
        }
        return false;
    }
    
    public bool TryRemoveAt(int index) {
        if (index < 0 || index >= items.Length) return false;
        
        if (items[index] == null) return false;

        items[index] = default;
        Invoke();
        return true;
    }

    //public void Sort(IComparer<T> comparer)
    //{
    //    if (items == null || items.Length == 0)
    //    {
    //        return;
    //    }

    //    // Create a list of non-null items and their original indices
    //    var indexedItems = items
    //        .Select((item, index) => new { Item = item, Index = index })
    //        .Where(x => x.Item != null)
    //        .ToList();

    //    // Sort non-null items using the provided comparer
    //    var sortedItems = indexedItems
    //        .OrderBy(x => x.Item, comparer ?? Comparer<T>.Default)
    //        .ToList();

    //    // Create a new array to store sorted items
    //    T[] newItems = new T[items.Length];

    //    // Place sorted items back into their original positions (or first available slots)
    //    int sortedIndex = 0;
    //    for (int i = 0; i < newItems.Length && sortedIndex < sortedItems.Count; i++)
    //    {
    //        if (items[i] != null) // Only overwrite slots that originally had non-null items
    //        {
    //            newItems[i] = sortedItems[sortedIndex].Item;
    //            sortedIndex++;
    //        }
    //    }

    //    // Update the items array and invoke the event
    //    items = newItems;
    //    Invoke();
    //}

    //public void Sort<TKey1, TKey2>(
    //    Func<T, TKey1> orderByKeySelector,
    //    Func<T, TKey2> thenByKeySelector,
    //    IComparer<TKey1> orderByComparer = null,
    //    IComparer<TKey2> thenByComparer = null)
    //{
    //    if (items == null || items.Length == 0)
    //    {
    //        return;
    //    }

    //    // Create a list of non-null items and their original indices
    //    var indexedItems = items
    //        .Select((item, index) => new { Item = item, Index = index })
    //        .Where(x => x.Item != null)
    //        .ToList();

    //    // Sort non-null items using OrderBy and ThenBy
    //    var sortedItems = indexedItems
    //        .OrderBy(x => orderByKeySelector(x.Item), orderByComparer ?? Comparer<TKey1>.Default)
    //        .ThenBy(x => thenByKeySelector(x.Item), thenByComparer ?? Comparer<TKey2>.Default)
    //        .ToList();

    //    // Create a new array to store sorted items
    //    T[] newItems = new T[items.Length];

    //    // Place sorted items back into their original positions (or first available slots)
    //    int sortedIndex = 0;
    //    for (int i = 0; i < newItems.Length && sortedIndex < sortedItems.Count; i++)
    //    {
    //        if (items[i] != null) // Only overwrite slots that originally had non-null items
    //        {
    //            newItems[i] = sortedItems[sortedIndex].Item;
    //            sortedIndex++;
    //        }
    //    }

    //    // Update the items array and invoke the event
    //    items = newItems;
    //    Invoke();
    //}
}