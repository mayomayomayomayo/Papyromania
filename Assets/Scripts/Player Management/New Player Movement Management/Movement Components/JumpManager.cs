using UnityEngine;

public class JumpManager : MovementComponent
{
    [SerializeField]
    private Timer cooldown;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private ControlFactorChangeParameters cfcp;

    protected override void DelayedAwake()
    {
        AssignKey(input.Jump, TryJump);
    }

    private void TryJump()
    {
        if (!mctx.IsGrounded() || !cooldown.Ready) return;

        rb.linearVelocity = rb.linearVelocity.NeuterY();
        
        player.movement.movementManager.TemporaryControlReduction(cfcp);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);

        cooldown.Start();
    }
}