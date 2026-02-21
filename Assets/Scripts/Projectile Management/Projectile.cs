using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected bool isEnemyPiercing;

    protected bool isTerrainPiercing;

    public ProjectileDamageData damageData;

    [SerializeField]
    protected LayerMask collisionLayerMask;
    
    /// <returns>The new position.</returns>
    protected abstract Vector3 UpdatePosition();

    protected abstract void OnHit(in HitInfo hit);

    private void FixedUpdate()
    {
        Vector3 oldPosition = transform.position;

        Vector3 newPosition = UpdatePosition();

        if (CheckCollisionLine(oldPosition, newPosition, out RaycastHit rh))
        {
            OnHit((HitInfo)rh);
        }
    }

    protected virtual bool CheckCollisionLine(Vector3 old, Vector3 @new, out RaycastHit rh)
    {
        bool isHit =  Physics.Linecast(
            old,
            @new,
            out rh,
            collisionLayerMask
        );

        return isHit;
    }

    protected void OnCollisionEnter(Collision other)
    {
        OnHit((HitInfo)other);
    }
}

[Serializable]
public struct ProjectileDamageData
{
    public enum Affiliation
    {
        Enemy,
        Player,
        None
    }

    public float Damage { get; private set; }
}

public struct HitInfo
{
    public Collider Collider;

    public Vector3 Point;

    public Vector3 Normal;

    public Rigidbody Rigidbody;

    public GameObject GameObject;

    public static explicit operator HitInfo(Collision col)
    {
        ContactPoint cp = col.GetContact(0);

        return new()
        {
            Collider = col.collider,
            Point = cp.point,
            Normal = cp.normal,
            Rigidbody = col.rigidbody,
            GameObject = col.gameObject
        };
    }

    public static explicit operator HitInfo(RaycastHit rh)
    {
        return new()
        {
            Collider = rh.collider,
            Point = rh.point,
            Normal = rh.normal,
            Rigidbody = rh.rigidbody,
            GameObject = rh.collider.gameObject
        };
    }
}