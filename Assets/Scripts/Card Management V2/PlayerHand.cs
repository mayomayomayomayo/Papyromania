using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoSingleton<PlayerHand>
{
    [SerializeField]
    private List<Card> _cards;

    private void Awake()
    {
        _cards = new();
    }
}

