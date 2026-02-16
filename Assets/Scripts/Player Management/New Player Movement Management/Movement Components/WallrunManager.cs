using UnityEngine;

public class WallrunManager : MovementComponent
{
    public bool isWallrunning;

    private Vector3 wallNormal;

    [SerializeField] 
    private ControlFactorChangeParameters walljumpCfcp;

    protected override void DelayedAwake()
    {
        AssignKey(input.Jump, TryWalljump);
    }

    private void StartWallrun()
    {
        isWallrunning = true;

        rb.useGravity = false;
        rb.linearVelocity = new(rb.linearVelocity.x, -player.stats.wallrunDrop, rb.linearVelocity.z);
    }

    private void StopWallrun()
    {
        isWallrunning = false;

        rb.useGravity = true;
    }

    private void AddWallrunForces()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float neededForce = player.stats.minimumWallrunVelocity - forwardSpeed;
        
        rb.AddForce(transform.forward * neededForce + -wallNormal * player.stats.wallrunPushForce);
    }

    private void TryWalljump()
    {
        if (!isWallrunning) return;

        StopWallrun();
        rb.linearVelocity = rb.linearVelocity.NeuterY();

        player.movement.movementManager.TemporaryControlReduction(walljumpCfcp);

        rb.AddForce(Vector3.up * player.stats.wallJumpVerticalForce + wallNormal * player.stats.wallJumpPushForce);

        // TODO ADD TIMER
    }

    private void FixedUpdate() // Kinda gross but okies
    {
        bool huggingWall = mctx.IsHuggingWall(out wallNormal) && !mctx.IsGrounded();

        if (!isWallrunning && huggingWall)
        {
            StartWallrun();
        }

        if (isWallrunning)
        {
            AddWallrunForces();

            if (!huggingWall) StopWallrun();
        }
    }
}