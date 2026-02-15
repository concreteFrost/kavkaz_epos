
public interface IShield : IBreakable
{
    ICollector Owner { get; set; }

    public ShieldSO ShieldData();

    public void ReduceDurability(float amount);

    public void PerformDefence();

    public void CancelDefence();

    public void ThrowShield();

    public void AssignToOwner(ICollector owner);
}
