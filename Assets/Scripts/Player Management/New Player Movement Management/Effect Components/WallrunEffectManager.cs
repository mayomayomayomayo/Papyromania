using UnityEngine;

public class WallrunEffectManager : EffectManagerComponent<WallrunManager>
{
    protected override void Register()
    {
        RegisterSubscription(
            () => target.OnStartWallrun += TestingTesting,
            () => target.OnStartWallrun -= TestingTesting
        );
    }

    private void TestingTesting()
    {
        Debug.Log("Works wow");
    }
}