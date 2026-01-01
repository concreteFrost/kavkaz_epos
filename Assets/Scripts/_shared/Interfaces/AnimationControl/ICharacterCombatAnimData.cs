
public interface ICharacterCombatAnimData
{
    bool IsAttacking { get; set; }
    bool IsWeaponed { get; set; }
    bool IsShieldRaised { get; }
    bool IsThrowingWeapon { get; set; }
    Attack CurrentAttack();
    void EndAttack();
}
