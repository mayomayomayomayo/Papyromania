using System;
using UnityEngine;

[Serializable]
public sealed class ProjectileSpawner<T> where T : Projectile
{
    private GameObject _go;

    [SerializeField]
    private T _projectile;

    public T Projectile
    {
        get
        {
            if (!_go)
            {
                _go = new GameObject(typeof(T).Name);
                _go.SetActive(false);
            }

            if (!_projectile)
            {
                _projectile = _go.AddComponent<T>();
            }

            return _projectile;
        }
    }
    
    public T SpawnProjectile(Vector3 position, Vector3 direction)
    {
        T newProjectile = UnityEngine.Object.Instantiate(Projectile);
        newProjectile.gameObject.SetActive(true);

        newProjectile.transform.SetPositionAndRotation(
            position, 
            Quaternion.LookRotation(direction, Vector3.up)
        );

        return newProjectile;
    }

    public T SpawnProjectile(Vector3 position, Quaternion direction)
    {
        T newProjectile = UnityEngine.Object.Instantiate(Projectile);
        newProjectile.gameObject.SetActive(true);

        newProjectile.transform.SetPositionAndRotation(
            position, 
            direction
        );

        return newProjectile;
    }
}