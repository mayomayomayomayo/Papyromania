using System;
using System.Collections;
using UnityEngine;

public class MovementManager : MovementComponent
{
    public float controlFactor = 1f;
    public AnimationCurve defaultControlReturnCurve;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            cam.transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        Vector3 target = mctx.MoveDirection * player.stats.movementSpeed;
        Vector3 current = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        rb.AddForce((target - current) * player.stats.acceleration * controlFactor, ForceMode.VelocityChange);
    }

    private IEnumerator TemporaryControlReductionCoroutine(ControlFactorChangeParameters cfcp)
    {
        controlFactor = cfcp.initialValue;

        yield return new WaitForSeconds(cfcp.initialReducedValuePleateau);

        float startTime = Time.time;

        while (controlFactor < cfcp.maximumValue)
        {
            float t = Mathf.InverseLerp(startTime, startTime + cfcp.recoveryPeriodLength, Time.time);
            controlFactor = Mathf.Lerp(cfcp.initialValue, cfcp.maximumValue, t);
            yield return new WaitForFixedUpdate();
        }

        controlFactor = cfcp.maximumValue;
    }

    public Coroutine TemporaryControlReduction(ControlFactorChangeParameters cfcp) => StartCoroutine(TemporaryControlReductionCoroutine(cfcp));
}

[Serializable]
public struct ControlFactorChangeParameters
{
    public float initialValue;
    public float initialReducedValuePleateau;
    public float recoveryPeriodLength;
    public float maximumValue;

    public ControlFactorChangeParameters(float val, float initial, float recovery, float max = 1f)
    {
        initialValue = val;
        initialReducedValuePleateau = initial;
        recoveryPeriodLength = recovery;
        maximumValue = max;
    }
}