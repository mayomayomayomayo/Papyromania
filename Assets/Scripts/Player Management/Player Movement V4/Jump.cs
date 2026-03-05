using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoSingleton<Jump>
{
    [SerializeField]
    private Timer _cooldown;

    [SerializeField]
    private float _jumpForce;

    [SerializeField]
    private ControlFactorChangeParameters _cfcp;

    private MovementContext mctx;

    public event Action OnJump;

    private void Start()
    {
        mctx = MonoSingleton<MovementContext>.Instance;
        mctx.input.Movement.Jump.performed += TryJump;
    }

    private void TryJump(InputAction.CallbackContext _)
    {
        if (!mctx.IsGrounded() || !_cooldown.Ready) return;
        
        Rigidbody rb = mctx.Rigidbody;

        rb.linearVelocity = rb.linearVelocity.NeuterY();

        MonoSingleton<Movement>.Instance.ReduceControl(_cfcp);

        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);

        OnJump?.Invoke();

        _cooldown.Start();
    }
}