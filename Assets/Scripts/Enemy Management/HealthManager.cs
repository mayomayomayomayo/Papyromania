using UnityEngine;

public class HealthManager
{
    public MonoBehaviour mono;

    private float _health;
    public float Health => _health;

    public float resistance;
    public float healEffectiveness;

    public void Damage(float amount)
    {
        _health -= amount * (1 - resistance);
    }

    public void AddHealth(float amount)
    {
        _health += amount;
    }

    public void Die()
    {
        Debug.LogWarning($"{mono.gameObject} has died!");
        //TODO: IMPLEMENT THIS
    }
}