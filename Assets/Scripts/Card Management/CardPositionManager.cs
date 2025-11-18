using UnityEngine;
using System.Collections.Generic;
using static CardManager;


public static class HandLayoutManager
{
    public static float radius = 2.5f;
    public static int centerIndex = 0;

    public static void UpdateHandLayout()
    {
        // centerIndex = FindCardIndexByName(CardObject.currentCard.name, hand);
        
        // for (int i = 0; i < hand.Count; i++)
        {
            // int deltaIndex = i - centerIndex;
            // float x = Mathf.Sign(deltaIndex) * Mathf.Pow(1.5f, Mathf.Abs(deltaIndex - 1)); 
            // hand[i].transform.localPosition = new Vector3(x, 0f, 0);
        }
    }
}