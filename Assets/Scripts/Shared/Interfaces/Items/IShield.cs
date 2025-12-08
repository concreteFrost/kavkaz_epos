
public interface IShield
{
    public ShieldSO ShieldData();

    public void ReduceDurability(float amount);

    public void PerformDefence();

    public void CancelDefence();

    public void ThrowShield();
}
