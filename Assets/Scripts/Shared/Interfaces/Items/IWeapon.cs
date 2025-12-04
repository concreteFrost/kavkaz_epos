
public interface IWeapon 
{
    public WeaponSO WeaponData();
    public void PerformAttack();
    public void CancelAttack();
    public void ThrowWeapon();
    public void ReduceDurability(float amount);

    public IAttackSource AttackSource { get; set; }
}
