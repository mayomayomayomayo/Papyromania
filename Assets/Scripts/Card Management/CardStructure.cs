using System;
using System.Collections.Generic;
using UnityEngine;

public class CardStructure
{
    private readonly List<Card> _cards = new();

    public IReadOnlyList<Card> Cards => _cards;

    [Header("Events")]
    public Action<Card> onAddCard;
    public Action<Card> onRemoveCard;
    
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