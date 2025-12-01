using System.Collections.Generic;
using UnityEngine;

public class CardStructure : MonoBehaviour
{
    public List<CardObject> list;

    public virtual int FindIndex(CardObject card)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ObjectID == card.ObjectID) return i;
        }
        return list.Count; // Index out of range exception! :) Seriously this should not happen.
    }

    public virtual int FindIndex(string match)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].cardName == match) return i;
        }
        return list.Count; // IOORE again fuck you! :)
    }

    public virtual List<int> FindIndexes(string match)
    {
        List<int> indexes = new();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].cardName == match) indexes.Add(i);
        }

        return indexes;
    }
}