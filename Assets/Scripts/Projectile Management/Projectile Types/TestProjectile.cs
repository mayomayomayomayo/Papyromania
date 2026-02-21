using UnityEngine;

public class TestProjectile : Projectile
{
    protected override Vector3 UpdatePosition()
    {
        // Let's just make it go up for now.
        transform.position += transform.forward * 0.01f;

        return transform.position;
    }

    protected override void OnHit(in HitInfo hit)
    {
        Debug.Log($"Hit: {hit.GameObject.name}");
        Destroy(gameObject);
    }
}