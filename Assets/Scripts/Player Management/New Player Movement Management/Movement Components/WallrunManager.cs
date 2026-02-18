using UnityEngine;

public class WallrunManager : MovementComponent
{
    // DON'T REFACTOR THIS UNTIL YOU'VE MADE LIKE SOME PROGRESS

    [SerializeField] 
    private float wallJumpLateralForce;

    [SerializeField] 
    private float wallJumpVerticalForce;

    [SerializeField]
    private float wallCheckDistance;

    [SerializeField]
    private float wallrunInwardsPushForce;

    [SerializeField]
    private float wallrunVelocity;

    public bool isWallrunning;

    [SerializeField] 
    private ControlFactorChangeParameters walljumpCfcp;

    private Vector3 wallNormal;

    private float wallSide;

    protected override void DelayedAwake()
    {
        AssignKey(input.Jump, TryWalljump);
    }

    private void StartWallrun()
    {
        isWallrunning = true;

        rb.useGravity = false;
        rb.linearVelocity = rb.linearVelocity.NeuterY();
    }

    private void StopWallrun()
    {
        isWallrunning = false;

        rb.useGravity = true;
    }

    private void AddWallrunForces()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float neededForce = wallrunVelocity - forwardSpeed;
        
        rb.AddForce(transform.forward * neededForce + -wallNormal * wallrunInwardsPushForce);
    }

    private void TryWalljump()
    {
        if (!isWallrunning) return;

        StopWallrun();
        rb.linearVelocity = rb.linearVelocity.NeuterY();

        player.movement.movementManager.TemporaryControlReduction(walljumpCfcp);

        rb.AddForce(Vector3.up * wallJumpVerticalForce + wallNormal * wallJumpLateralForce);

        // TODO ADD TIMER
    }

    private void FixedUpdate() // Kinda gross but okies
    {
        if (!isWallrunning)
        {
            if (IsHuggingWall(out wallNormal, out wallSide) && !mctx.IsGrounded()) 
                StartWallrun(); 
        }

        else // if (isWallrunning) (for clarity's sake; i'm stupid)
        {
            bool huggingWall = Physics.Raycast(
                transform.position,
                cam.transform.right * wallSide,
                out RaycastHit hit,
                wallCheckDistance
            );

            wallNormal = hit.normal;

            if (!huggingWall || mctx.MoveInput.x * wallSide < 0f || mctx.MoveInput.y < 0.1f) StopWallrun();

            AddWallrunForces();
        }
    }

    public bool IsHuggingWall(out Vector3 wallNormal, out float side)
    {
        side = 0f;
        
        if (mctx.MoveInput.x == 0f)
        {
            wallNormal = Vector3.zero;
            return false;
        }
        
        float inputSign = Mathf.Sign(mctx.MoveInput.x);
        
        bool isHugging = Physics.Raycast(
            transform.position,
            cam.transform.right * inputSign,
            out RaycastHit hit,
            wallCheckDistance
        );
        
        wallNormal = hit.normal;
        side = inputSign;
        
        return isHugging;
    }
}