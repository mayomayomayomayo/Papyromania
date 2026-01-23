using UnityEngine;

public class JumpManager : MovementManager
{
    protected override void DelayedAwake()
    {
        AssignKey(input.Jump, DetermineJumpType);
        cooldown = new(player.stats.jumpCooldownLength);
    }

    private void DetermineJumpType()
    {
        if (moveCtx.CanWallJump(out _)) TryWallJump();
        else TryJump();
    }

    private void TryJump()
    {
        if (!moveCtx.CheckGrounded() || !cooldown.Ready) return;

        Rigidbody rb = player.playerRigidbody;

        rb.linearVelocity = new (rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * player.stats.jumpForce, ForceMode.VelocityChange);

        cooldown.Start();
    }

    private void TryWallJump()
    {
        if (moveCtx.CanWallJump(out Vector3 normal) && cooldown.Ready)
        {
            Rigidbody rb = player.playerRigidbody;

            rb.linearVelocity = Vector3.zero;

            rb.AddForce(normal * player.stats.wallJumpPushForce + Vector3.up * player.stats.wallJumpVerticalForce, ForceMode.VelocityChange);

            cooldown.Start();
        }
    }
}