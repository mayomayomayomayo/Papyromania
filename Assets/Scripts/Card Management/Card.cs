using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Divisions")]
    public CardDefinition definition;
    public CardVisuals visuals;
    public CardBehaviour behaviour;

    [Header("References")]
    public Player player;
}


