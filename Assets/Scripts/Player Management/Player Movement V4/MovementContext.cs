using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MovementContext : MonoSingleton<MovementContext>
{
    public PlayerInputActions input;

    public Vector2 MoveInput { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 Right { get; private set; }

    public Vector3 Forward2D { get; private set; }

    public Vector3 Right2D { get; private set; }

    public Vector3 MoveDirection { get; private set; }

    public CapsuleCollider Collider { get; private set; }

    public Rigidbody Rigidbody { get; private set; }

    public Camera Camera { get; private set; }

    [SerializeField]
    private Camera _camera;

    public bool IsGrounded() =>
        Physics.CheckSphere(
            transform.position + Vector3.down * (Collider.height / 2f),
            Collider.radius / 2,
            ~(1 << gameObject.layer)
        );

    public bool PredictNextFixedUpdateCollision(out Vector3 collisionNormal)
    {
        collisionNormal = Vector3.zero;

        ColliderUtils.CapsuleColliderData cd = Collider.ExtractCapsuleInformation();

        Vector3 vel = Rigidbody.linearVelocity;
        Vector3 dir = vel.normalized;
        float dist = vel.magnitude * Time.fixedDeltaTime;

        if (dist < Mathf.Epsilon) 
            return false;

        bool hit = Physics.CapsuleCast(
            cd.point1,
            cd.point2,
            cd.radius,
            dir,
            out RaycastHit rh,
            dist,
            ~(1 << gameObject.layer)
        );

        if (hit)
            collisionNormal = rh.normal;

        return hit;
    }

    private void Awake()
    {
        input = new();

        Collider = GetComponent<CapsuleCollider>();
        Rigidbody = GetComponent<Rigidbody>();
        Camera = /*Set in inspector*/ _camera;
    } 

    private void FixedUpdate()
    {
        MoveInput = input.Movement.Direction.ReadValue<Vector2>().ForceMagnitudeOf1();

        Forward = Camera.transform.forward.normalized;
        Right = Camera.transform.right.normalized;

        Forward2D = new Vector3(Forward.x, 0, Forward.z).normalized;
        Right2D = new Vector3(Right.x, 0, Right.z).normalized;
        
        MoveDirection = (Forward2D * MoveInput.y + Right2D * MoveInput.x).normalized;
    }

    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}