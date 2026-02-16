using UnityEngine;

public class DashManager : DepMovementManager
{
    public bool hasDash;

    protected override void DelayedAwake()
    {
        AssignKey(input.Dash, TryDash);
        cooldown = new(player.stats.dashCooldownLength);
    }

    // Ew, per-frame checking
    private void FixedUpdate() => hasDash |= moveCtx.IsGrounded();

    private void TryDash()
    {
        if (!hasDash || !cooldown.Ready) return;

        Rigidbody rb = player.physical.rb;

        Vector2 input = moveCtx.MoveInput != Vector2.zero ? moveCtx.MoveInput : new (0, 1);

        Vector3 dashDirection = (moveCtx.Forward2D * input.y + moveCtx.Right2D * input.x).normalized;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dashDirection * player.stats.dashForce, ForceMode.VelocityChange);

        hasDash = false;
        cooldown.Start();
    }
}