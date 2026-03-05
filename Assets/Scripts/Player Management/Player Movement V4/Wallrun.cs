using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Wallrun : MonoSingleton<Wallrun>
{
    public bool IsWallrunning { get; private set; }

    [SerializeField]
    private Timer _cooldown;

    [SerializeField]
    private float _wallJumpLateralForce;

    [SerializeField]
    private float _wallJumpVerticalForce;

    [SerializeField]
    private float _wallCheckDistance;

    [SerializeField]
    private float _wallrunInwardsPushForce;

    [SerializeField]
    private ControlFactorChangeParameters _cfcp;

    private Vector3 _wallNormal;

    private float _wallSide;

    private MovementContext mctx;

    private Rigidbody rb;

    public event Action OnStartWallrun;

    public event Action OnEndWallrun;

    public event Action OnWallJump;

    private void Start()
    {
        mctx = MonoSingleton<MovementContext>.Instance;
        rb = mctx.Rigidbody;
        mctx.input.Movement.Jump.performed += TryWalljump;
    }

    private void StartWallrun()
    {
        IsWallrunning = true;

        rb.useGravity = false;
        rb.linearVelocity = rb.linearVelocity.NeuterY();

        OnStartWallrun?.Invoke();
    }

    private void StopWallrun()
    {
        IsWallrunning = false;

        rb.useGravity = true;

        OnEndWallrun?.Invoke();
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
            mctx.Camera.transform.right * inputSign,
            out RaycastHit hit,
            _wallCheckDistance
        );
        
        wallNormal = hit.normal;
        side = inputSign;
        
        return isHugging;
    }

    private void AddWallrunForces()
    {
        rb.AddForce(-_wallNormal * _wallrunInwardsPushForce);
    }

    private void TryWalljump(InputAction.CallbackContext _)
    {
        if (!IsWallrunning || !_cooldown.Ready) return;

        StopWallrun();

        rb.linearVelocity = rb.linearVelocity.NeuterY();

        rb.AddForce(Vector3.up * _wallJumpVerticalForce + _wallNormal * _wallJumpLateralForce);
       
        MonoSingleton<Movement>.Instance.ReduceControl(_cfcp);
        
        _cooldown.Start();

        OnWallJump?.Invoke();
    }

    private void FixedUpdate()
    {
        if (IsWallrunning)
        {
            bool hugging = Physics.Raycast(
                transform.position,
                mctx.Camera.transform.right * _wallSide,
                out RaycastHit hit,
                _wallCheckDistance
            );

            _wallNormal = hit.normal;

            if (!hugging 
                || mctx.MoveInput.x * _wallSide < 0f 
                || mctx.MoveInput.y < -0.1f )
            {
                StopWallrun();
            }
                
            AddWallrunForces();
        }

        else
        {
            if (IsHuggingWall(out _wallNormal, out _wallSide)
                && !mctx.IsGrounded())
            {
                StartWallrun();
            }
        }
    }
}