
public interface ICharacterCombatAnimData
{
    bool IsAttacking { get; set; }
    bool IsWeaponed { get; set; }
    int AttackIndex { get; }
    int WeaponIndex { get; }
    bool IsShieldRaised { get; }
    bool IsThrowingWeapon { get; set; }
}
