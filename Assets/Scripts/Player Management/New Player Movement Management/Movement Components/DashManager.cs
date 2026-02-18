using System.Collections;
using UnityEngine;

public class DashManager : MovementComponent
{
    // Call it what it is, I'm just a bad programmer

    public bool hasDash;

    [SerializeField]
    private Timer cooldown;

    [SerializeField]
    private float smallDashForce;

    [SerializeField]
    private float longDashForce;

    [SerializeField]
    private float dashChargeRate;

    [SerializeField]
    private float minimumDirectedDashCharge;

    [SerializeField]
    private float maximumDashCharge;

    [SerializeField]
    private float maximumLongDashDistance;

    [SerializeField]
    private float longDashStopDistance;

    [SerializeField]
    private float chargingRigidbodyVelocityMitigation;

    [SerializeField]
    private ControlFactorChangeParameters cfcp;

    private Coroutine dashRoutine;

    protected override void DelayedAwake()
    {
        AssignKey(input.Dash, BeginDash);
    }

    private void BeginDash()
    {
        if (!hasDash) return;

        if (dashRoutine != null) StopCoroutine(dashRoutine);

        dashRoutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        hasDash = false;

        float charge = 0f;

        rb.useGravity = false;

        player.movement.movementManager.ControlFactor = 0f;

        while (input.Dash.IsPressed())
        {
            charge += Time.fixedDeltaTime * dashChargeRate;

            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, chargingRigidbodyVelocityMitigation);

            yield return new WaitForFixedUpdate();
        }

        charge = Mathf.Clamp(charge, 0f, maximumDashCharge);

        if (charge < minimumDirectedDashCharge)
        {
            ShortDash();
        }
        else
        {
            if (Physics.Raycast(
                cam.transform.position,
                cam.transform.forward,
                out RaycastHit hit,
                maximumLongDashDistance,
                ~(1 << gameObject.layer)
            ))
            {
                yield return DirectedDash(hit);
            }
            else ShortDash(); // this feels weird
            
        }
        
        rb.useGravity = true;

        player.movement.movementManager.TemporaryControlReduction(cfcp);
    }

    private void ShortDash()
    {
        Vector3 dashInput = mctx.MoveInput != Vector2.zero ? mctx.MoveInput : new (0, 1);
        Vector3 dashDirection = (mctx.Forward2D * dashInput.y + mctx.Right2D * dashInput.x).normalized;

        rb.AddForce(dashDirection * smallDashForce, ForceMode.VelocityChange);
    }

    private IEnumerator DirectedDash(RaycastHit hit)
    {
        Vector3 direction = (hit.point - transform.position).normalized;

        while (Vector3.Distance(hit.point, transform.position) > longDashStopDistance && !mctx.IsGrounded() && !mctx.PredictNextFixedUpdateCollision(out _))
        {
            rb.AddForce(direction * longDashForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        hasDash |= mctx.IsGrounded();

        if (player.movement.wallrunManager != null) hasDash |= player.movement.wallrunManager.isWallrunning;
    }
}