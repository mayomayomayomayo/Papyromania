public interface IDamageable
{
    enum DamageType
    {
        Normal,
        NoKnockback
    }

    public void TakeDamage(float damage, DamageType t = DamageType.NoKnockback);
}