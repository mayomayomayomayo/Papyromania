using UnityEngine;

public abstract class CustomCardBehaviour : MonoBehaviour
{
    public abstract void OnUse();
    public abstract void OnSecondaryUse();
    public abstract void OnDiscard();
}