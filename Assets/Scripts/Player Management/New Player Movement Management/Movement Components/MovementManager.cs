using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MovementManager : MovementComponent
{
    private float _controlFactor = 1f;
    public float ControlFactor
    {
        get => _controlFactor;

        set
        {
            if (controlFactorChangeCoroutine != null) StopCoroutine(controlFactorChangeCoroutine);
            _controlFactor = value;
        }
    }

    [SerializeField]
    private float predictiveCollisionVelocityReduction;
    
    private Coroutine controlFactorChangeCoroutine;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            cam.transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        Vector3 target = mctx.MoveDirection * player.stats.movementSpeed;
        Vector3 current = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        rb.AddForce((target - current) * player.stats.acceleration * _controlFactor, ForceMode.VelocityChange);

        if (!mctx.IsGrounded() && !(player.movement.wallrunManager && player.movement.wallrunManager.isWallrunning))
        {
            if (mctx.PredictNextFixedUpdateCollision(out Vector3 collisionNormal))
            {
                rb.linearVelocity = rb.linearVelocity.ProjectOntoPlane(collisionNormal);
            }
        }
    }

    private IEnumerator TemporaryControlReductionCoroutine(ControlFactorChangeParameters cfcp)
    {
        _controlFactor = cfcp.initialValue;

        yield return new WaitForSeconds(cfcp.initialReducedValuePleateau);

        float startTime = Time.time;

        while (_controlFactor < cfcp.maximumValue)
        {
            float t = Mathf.InverseLerp(startTime, startTime + cfcp.recoveryPeriodLength, Time.time);
            _controlFactor = Mathf.Lerp(cfcp.initialValue, cfcp.maximumValue, t);
            yield return new WaitForFixedUpdate();
        }

        _controlFactor = cfcp.maximumValue;
    }

    public Coroutine TemporaryControlReduction(ControlFactorChangeParameters cfcp)
    {
        if (controlFactorChangeCoroutine != null) StopCoroutine(controlFactorChangeCoroutine);
        
        return controlFactorChangeCoroutine = StartCoroutine(TemporaryControlReductionCoroutine(cfcp));
    } 
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