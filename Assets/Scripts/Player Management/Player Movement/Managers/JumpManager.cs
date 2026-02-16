using UnityEngine;

public class JumpManager : DepMovementManager
{
    protected override void DelayedAwake()
    {
        AssignKey(input.Jump, DetermineJump);
        cooldown = new(player.stats.jumpCooldownLength);
    }

    private void DetermineJump()
    {
        // if (moveCtx.CanWallJump(out _)) TryWallJump();
        // else TryJump();
    }

    private void TryJump()
    {
        if (!moveCtx.IsGrounded() || !cooldown.Ready) return;

        Rigidbody rb = player.physical.rb;

        rb.linearVelocity = rb.linearVelocity.NeuterY();
        rb.AddForce(Vector3.up * player.stats.jumpForce, ForceMode.VelocityChange);

        cooldown.Start();
    }

    private void TryWallJump()
    {
        // if (moveCtx.CanWallJump(out Vector3 normal) && cooldown.Ready)
        // {
        //     Rigidbody rb = player.physical.rb;
        // 
        //     rb.linearVelocity = Vector3.zero;
        // 
        //     rb.AddForce(normal * player.stats.wallJumpPushForce + Vector3.up * player.stats.wallJumpVerticalForce, ForceMode.VelocityChange);
        // 
        //     cooldown.Start();
        // }
    }
}