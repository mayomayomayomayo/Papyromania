using UnityEngine;
using System;

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
}