using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public abstract class CardBehaviour : MonoBehaviour
{
    public Card owner;

    [Header("References")]
    public BoxCollider cardCollider;
    public Player player;
    public Hand playerHand;

    [Header("State")]
    public CardState state;

    private void Awake()
    {
        GetRefs();
    }

    private void GetRefs()
    {
        cardCollider = GetComponent<BoxCollider>();
    }

    public abstract void UsePrimary();

    public abstract void UseSecondary();

    public virtual void Discard() {} // TODO IMPLEMENT THIS

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state == CardState.Dropped)
        {
            cardCollider.enabled = false;
            state = CardState.InHand;

            player = other.GetComponent<Player>();
            playerHand = player.hand;

            Pickup();
        }
    }

    protected virtual void Pickup()
    {
        if (player == null) throw new Exception("Player reference is null");

        playerHand.cards.AddCard(owner);

        transform.SetParent(player.playerHandAnchor);
        StartCoroutine(transform.Move(player.playerHandAnchor));
    }

    public enum CardState
    {
        InHand,
        Dropped,
        DroppedNonInteractable
    }
}