using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] 
public abstract class CardBehaviour : MonoBehaviour
{
    [Header("Owner")]
    public Card owner;

    [Header("References")]
    public BoxCollider cardCollider;
    public Player player;

    [Header("State")]
    public CardState state;

    [Header("Events")]
    public Action<Player> onPickup;

    private void Awake()
    {
        GetRefs();
    }

    protected virtual void GetRefs()
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

            player = other.GetComponent<Player>();

            state = CardState.InHand;

            transform.SetParent(player.cardManagement.handAnchor);

            player.cardManagement.hand.handStructure.AddCard(owner);
        }
    }

    public enum CardState
    {
        InHand,
        Dropped,
        DroppedNonInteractable
    }
}