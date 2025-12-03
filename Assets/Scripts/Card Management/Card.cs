using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Divisions")]
    public CardDefinition data;
    public CardVisuals visuals;
    public CardBehaviour behaviour;

    [Header("References")]
    public Player player;

    public enum CardState
    {
        InHand,
        Dropped,
        DroppedNonInteractable
    }
}

// TODO ADD PICKUP LOGIC

//    private void Pickup(GameObject other)
//    {
//        player = other.GetComponent<Player>();
//        playerHand = player.hand;
//        
//        cardCollider.enabled = false;
//        cardState = CardState.InHand;
//        cardPositionManager.OnPickup(other);
//        playerHand.cards.AddCard(this);
//        playerHand.UpdateHandLayout();
//    }


