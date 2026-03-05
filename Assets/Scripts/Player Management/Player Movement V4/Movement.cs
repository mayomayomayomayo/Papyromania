using Unity.Cinemachine;
using UnityEngine;
using System;
using System.Collections;

public class Movement : MonoSingleton<Movement>
{
    [SerializeField]
    private float _speed;

    public float MovementSpeed => _speed;

    [SerializeField]
    private float _accel;

    public float Acceleration => _accel;

    private float _controlFactor = 1f;

    private Coroutine _controlFactorChangeCoroutine;

    public float ControlFactor
    {
        get => _controlFactor;

        set
        {
            if (_controlFactorChangeCoroutine != null)
                StopCoroutine(_controlFactorChangeCoroutine);
        }
    }

    private void FixedUpdate()
    {
        MovementContext mctx = MonoSingleton<MovementContext>.Instance;
        Rigidbody rb = mctx.Rigidbody;

        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            mctx.Camera.transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        Vector3 targetVel = mctx.MoveDirection * _speed;
        Vector3 currentVel = new(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        rb.AddForce((targetVel - currentVel) * _accel * _controlFactor, ForceMode.VelocityChange);

        if (!mctx.IsGrounded()
            && mctx.PredictNextFixedUpdateCollision(out Vector3 collisionNormal)
            && !MonoSingleton<Wallrun>.Instance.IsWallrunning)
        {
            rb.linearVelocity = rb.linearVelocity.ProjectOntoPlane(collisionNormal);
        }
    }

    public Coroutine ReduceControl(ControlFactorChangeParameters cfcp)
    {
        if (_controlFactorChangeCoroutine != null) StopCoroutine(_controlFactorChangeCoroutine);

        return _controlFactorChangeCoroutine = StartCoroutine(ReduceControlCoroutine(cfcp));
    }

    private IEnumerator ReduceControlCoroutine(ControlFactorChangeParameters cfcp)
    {
        _controlFactor = cfcp.initialValue;

        yield return new WaitForSeconds(cfcp.plateau);

        float startTime = Time.time;

        while (_controlFactor < 1f)
        {
            float t = Mathf.InverseLerp(startTime, startTime + cfcp.recovery, Time.time);
            _controlFactor = Mathf.Lerp(cfcp.initialValue, 1f, t);
            yield return new WaitForFixedUpdate();
        }

        _controlFactor = 1f;
    }
}

[Serializable]
public struct ControlFactorChangeParameters
{
    public float initialValue;
    public float plateau;
    public float recovery;

    public ControlFactorChangeParameters(float val, float initial, float recovery)
    {
        initialValue = val;
        plateau = initial;
        this.recovery = recovery;
    }
}