using UnityEngine;


public class PlayerMovement : DepMovementManager
{
    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);

        Vector3 target = moveCtx.MoveDirection * player.stats.movementSpeed;
        Vector3 current = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 delta = target - current;

        rb.AddForce(delta * player.stats.acceleration, ForceMode.VelocityChange);
    }
}