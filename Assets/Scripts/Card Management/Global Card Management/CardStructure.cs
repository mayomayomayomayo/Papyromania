using System;
using System.Collections.Generic;

public class CardStructure
{
    public List<CardObject> cards = new();

    public virtual int FindIndex(CardObject card)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].ObjectID == card.ObjectID) return i;
        }
        throw new Exception("Index not found");
    }

    public virtual int FindIndex(string match)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardName == match) return i;
        }
        throw new Exception("Index not found");
    }

    public virtual List<int> FindIndexes(string match)
    {
        List<int> indexes = new();

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardName == match) indexes.Add(i);
        }

        return indexes;
    }
}