using System;
using UnityEngine;

[DefaultExecutionOrder(-40)]
public sealed class Player : MonoBehaviour
{
    public PlayerStats stats;

    public Movement movement;

    public CardManagement cardManagement;

    public Physical physical;

    [Serializable]
    public class Movement
    {
        public MovementContext ctx;
        public MovementManager movementManager;
        public WallrunManager wallrunManager;
        public JumpManager jumpManager;
        public DashManager dashManager;
    }

    [Serializable]
    public class CardManagement
    {
        public Transform handAnchor;
        public Hand hand;
    }

    [Serializable]
    public class Physical
    {
        public Camera cam;
        public Rigidbody rb;
        public CapsuleCollider collider;
    }

    [Serializable]
    public class PlayerStats
    {
        [Header("Movement")]
        public float movementSpeed;
        public float acceleration;

        public float jumpForce;
        public float jumpCooldownLength;

        public float wallJumpPushForce;
        public float wallJumpVerticalForce;

        public float dashForce;
        public float dashCooldownLength;

        public float wallCheckDistance;
        public float wallrunDrop;
        public float minimumWallrunVelocity;
        public float wallrunPushForce;
    }
}

