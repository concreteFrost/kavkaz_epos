
public interface IShield : ICombatItem
{
    bool IsProtectionActive { get; set; }

    public ShieldSO ShieldData();

    public void PerformDefence();


    public void CancelDefence();


}
