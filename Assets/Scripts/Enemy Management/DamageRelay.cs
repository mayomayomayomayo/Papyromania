using UnityEngine;
using static IDamageable;

public class DamageRelay : MonoBehaviour, IDamageable
{
    public HealthManager manager;

    public void TakeDamage(float amount, DamageType type = DamageType.NoKnockback)
    {
        manager.Damage(amount);

        //TODO: IMPLEMENT KNOCKBACK AND STUFF.
    }
}