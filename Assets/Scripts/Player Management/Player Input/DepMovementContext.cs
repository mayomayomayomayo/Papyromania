using UnityEngine;

// [RequireComponent(typeof(Player))]
// public class DepMovementContext : MonoBehaviour
// {
//     public Player player;
// 
//     public Vector2 MoveInput { get; private set; }
//     public Vector3 Forward { get; private set; }
//     public Vector3 Right { get; private set; }
//     public Vector3 Forward2D { get; private set; }
//     public Vector3 Right2D { get; private set; }
//     public Vector3 MoveDirection { get; private set; }
//     
//     public bool CheckGrounded()
//     {
//         return Physics.CheckSphere(
//             transform.position + Vector3.down * (player.playerCollider.height / 2), 
//             player.playerCollider.radius / 2,
//             ~(1 << gameObject.layer)
//         );
//     }
// 
//     public bool IsBumpingWall(out Vector3 wallNormal)
//     {
//         wallNormal = Vector3.zero;
// 
//         if (Physics.Raycast(transform.position, MoveDirection, out RaycastHit hit, player.stats.wallCheckDistance) && hit.normal.y < GameConstants.MIN_WALL_NORMAL_Y)
//         {
//             wallNormal = hit.normal;
//             return true;
//         }
//         else return false;
//     }
// 
//     // Move to JumpManager
//     public bool CanWallJump(out Vector3 wallNormal)
//     {
//         wallNormal = Vector3.zero;
//         
//         if (IsBumpingWall(out Vector3 n) && !CheckGrounded())
//         {
//             wallNormal = n;
//             return true;
//         }
//         else return false;
//     }
// 
//     private void Awake()
//     {
//         player = GetComponent<Player>();
//     }
// 
//     private void FixedUpdate()
//     {
//         MoveInput = player.playerInputCtx.input.Movement.Direction.ReadValue<Vector2>();
//         Forward = player.playerCamera.transform.forward.normalized;
//         Right = player.playerCamera.transform.right.normalized;
//         Forward2D = Vector3.Scale(Forward, new(1, 0, 1)).normalized;
//         Right2D = Vector3.Scale(Right, new(1, 0, 1)).normalized;
//         MoveDirection = (Forward2D * MoveInput.y + Right2D * MoveInput.x).normalized;
//     }
// }