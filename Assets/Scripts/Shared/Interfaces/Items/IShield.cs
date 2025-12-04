
public interface IShield
{
    public ShieldSO ShieldData();

    public void PerformDefence();

    public void CancelDefence();

    public void ThrowShield();
    public IAttackSource AttackSource { get; set; }
}
