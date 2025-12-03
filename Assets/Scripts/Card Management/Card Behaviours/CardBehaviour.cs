using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class CardBehaviour : MonoBehaviour
{
    public Card card; // Figure out how to set this

    [Header("References")]
    public BoxCollider cardCollider;

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
}