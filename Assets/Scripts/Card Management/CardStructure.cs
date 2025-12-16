using System;
using System.Collections.Generic;
using System.Collections;


public class CardStructure : IEnumerable<Card>
{
    private readonly List<Card> _cards = new();

    public Action<Card> onAddCard;
    public Action<Card> onRemoveCard;

    public Card this[int index] => _cards[index];

    public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual int Count => _cards.Count;
    
    public virtual void AddCard(Card card)
    {
        _cards.Add(card);
        onAddCard?.Invoke(card);
    }

    public virtual void RemoveCard(Card card)
    {
        _cards.RemoveAt(FindIndex(card));
        onRemoveCard?.Invoke(card);
    }

    public virtual void Move(Card card, int pos)
    {
        _cards.Insert(pos, card);
        _cards.RemoveAt(FindIndex(card));
        onAddCard?.Invoke(card);
    }

    public virtual int FindIndex(Card card)
    {
        int index = _cards.IndexOf(card);
        return index >= 0 ? index : throw new Exception("Index not found");
    }

    public virtual int FindIndex(string match)
    {
        int index = _cards.FindIndex(c => c.definition.name == match);
        return index >= 0 ? index : throw new Exception("Index not found");
    }
}