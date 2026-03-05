using UnityEngine;

public class TestCardEffect : CardEffect
{
    public string test; 

    public override void Execute(CardUseContext ctx)
    {
        Debug.Log($"TestCardEffect has been executed with string: {test}");
    }
}

public class TakeGunpowder : CardEffect
{
    public float amount;

    public override void Execute(CardUseContext ctx)
    {
        Debug.Log($"Took {amount} gunpowder");
    }
}