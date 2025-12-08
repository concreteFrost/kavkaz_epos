
public interface IDamagable
{
    public string SourceId();
    public float Health();
    public abstract void TakeDamage(float damage);   
    public void Die();
}
