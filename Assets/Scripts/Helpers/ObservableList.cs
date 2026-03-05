using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class ObservableList<T> : IEnumerable<T>
{
    [SerializeField]
    private readonly List<T> _list = new();

    public event Action<T> OnAdd;

    public event Action<T> OnRemove;

    public IReadOnlyList<T> Items => _list;

    public void Add(T item)
    {
        _list.Add(item);
        OnAdd?.Invoke(item);
    }

    public bool Remove(T item)
    {
        bool rem = _list.Remove(item);
        
        if (rem)
            OnRemove?.Invoke(item);
        
        return rem;
    }

    public T this[int index] => _list[index];

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}