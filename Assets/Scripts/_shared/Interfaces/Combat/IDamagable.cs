
public interface IDamagable
{
    public string SourceId();
    public abstract void TakeDamage(float damage, float balanceDamage, IAttackSource source=null);   
    public void Die();
    public bool IsDead();
}
