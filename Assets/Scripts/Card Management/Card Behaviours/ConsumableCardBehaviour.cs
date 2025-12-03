using UnityEngine;

public class ConsumableCardBehaviour : CardBehaviour
{
    public override void UsePrimary()
    {
        Debug.Log($"{owner.name} (Consumable) primary use message");
    }

    public override void UseSecondary()
    {
        Debug.Log($"{owner.name} (Consumable) secondary use message");
    }
}