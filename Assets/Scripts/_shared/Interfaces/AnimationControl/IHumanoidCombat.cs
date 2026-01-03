
public interface IHumanoidCombat
{
    //bool IsAttacking { get; set; }
    bool IsWeaponed { get; set; }
    bool IsShieldRaised { get; set; }
    void EndAttack();
    void PerformAttack();
    void PerformBlock();
    void CancelBlock();
    void ThrowWeapon();
    void ThrowShield();

}
