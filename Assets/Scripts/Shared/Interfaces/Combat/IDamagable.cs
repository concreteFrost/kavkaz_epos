
public interface IDamagable
{
    public string SourceId();
    public float Health();
    public void TakeDamage(float damage);   
    public void Die();
}
