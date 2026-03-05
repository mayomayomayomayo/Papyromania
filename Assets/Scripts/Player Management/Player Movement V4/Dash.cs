using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoSingleton<Dash>
{
    public bool HasDash { get; private set; }

    [SerializeField]
    private Timer _cooldown;

    [SerializeField]
    private float _force;

    [SerializeField]
    private float _noGravDistance;

    [SerializeField]
    private float _endOfDashVelocityReduction;

    [SerializeField]
    private ControlFactorChangeParameters _cfcp;

    private Coroutine _dashRoutine;

    private MovementContext mctx;

    public event Action OnStartDash;

    public event Action OnEndDash;

    private void Start()
    {
        mctx = MonoSingleton<MovementContext>.Instance;
        mctx.input.Movement.Dash.performed += TryDash;
    }

    private void TryDash(InputAction.CallbackContext _)
    {
        if (!HasDash || !_cooldown.Ready) return;

        if (_dashRoutine != null) StopCoroutine(_dashRoutine);

        _dashRoutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Rigidbody rb = mctx.Rigidbody;
        
        HasDash = false;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        MonoSingleton<Movement>.Instance.ReduceControl(_cfcp);

        OnStartDash?.Invoke();

        Vector3 startPos = transform.position;

        Vector3 direction = mctx.MoveDirection != Vector3.zero ? 
            mctx.MoveDirection : 
            mctx.Forward2D;

        bool EndDashCheck() => 
            (Vector3.Distance(transform.position, startPos) >= _noGravDistance)
            || mctx.PredictNextFixedUpdateCollision(out _);

        while (!EndDashCheck())
        {
            rb.AddForce(direction * _force, ForceMode.VelocityChange);

            yield return new WaitForFixedUpdate();
        }

        rb.useGravity = true;
        rb.linearVelocity *= _endOfDashVelocityReduction;

        OnEndDash?.Invoke();
    }

    private void FixedUpdate()
    {
        HasDash |= mctx.IsGrounded() || MonoSingleton<Wallrun>.Instance.IsWallrunning;
    }
}