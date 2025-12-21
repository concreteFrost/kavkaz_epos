
public interface ICharacterCombatAnimData
{
    bool IsAttacking { get; set; }
    bool IsWeaponed { get; }
    int AttackIndex { get; }
    int WeaponIndex { get; }
    bool IsShieldRaised { get; }
    bool BlockRotation { get; set; }
}
