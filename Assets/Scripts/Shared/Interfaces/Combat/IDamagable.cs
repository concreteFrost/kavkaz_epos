
public interface IDamagable
{
    public string SourceId();
    public float Health();
    public abstract void TakeDamage(float damage, float balanceDamage);   
    public void Die();

    public bool IsDead();
}
