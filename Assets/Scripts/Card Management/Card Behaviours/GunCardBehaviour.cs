using UnityEngine;

public class GunCardBehaviour : CardBehaviour
{
    public override void UsePrimary()
    {
        Debug.Log($"{owner.name} (Gun) primary use message");
    }

    public override void UseSecondary()
    {
        Debug.Log($"{owner.name} (Gun) secondary use message");
    }
}