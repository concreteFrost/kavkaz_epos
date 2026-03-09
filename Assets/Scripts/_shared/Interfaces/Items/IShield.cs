
public interface IShield : IBreakable
{
    ICollector Owner { get; set; }
    bool IsProtectionActive { get; set; }

    public ShieldSO ShieldData();

    public void ReduceDurability();

    public void PerformDefence();

    public void CancelDefence();

    public void ThrowShield();

    public void AssignToOwner(ICollector owner);
}
