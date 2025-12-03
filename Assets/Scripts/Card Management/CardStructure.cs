using System;
using System.Collections.Generic;
using UnityEngine;

public class CardStructure
{
    [Header("Contents")]
    public List<Card> cards = new();

    [Header("Events")]
    public Action<Card> onAddCard;
    
    public virtual void AddCard(Card card)
    {
        cards.Add(card);
        onAddCard.Invoke(card);
    }

    public virtual int FindIndex(Card card)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (card == cards[i]) return i;
        }
        throw new Exception("Index not found");
    }

    public virtual int FindIndex(string match)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].definition.name == match) return i;
        }
        throw new Exception("Index not found");
    }

    public virtual List<int> FindIndexes(string match)
    {
        List<int> indexes = new();

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].definition.name == match) indexes.Add(i);
        }

        return indexes;
    }
}