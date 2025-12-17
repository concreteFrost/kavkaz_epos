
public interface ICharacterCombatAnimData
{
    bool IsAttacking { get; }
    bool IsWeaponed { get; }
    int AttackIndex { get; }
    int WeaponIndex { get; }
    bool IsShieldRaised { get; }
    bool IsDodging { get; }
}
