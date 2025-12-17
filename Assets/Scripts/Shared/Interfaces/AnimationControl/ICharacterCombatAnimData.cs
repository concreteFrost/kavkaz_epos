
public interface ICharacterCombatAnimData
{
    bool IsAttacking { get; set; }
    bool IsWeaponed { get; }
    int AttackIndex { get; }
    int WeaponIndex { get; }
    bool IsShieldRaised { get; }
    bool IsDodging { get; set; }

    float DodgeX { get; set; }

    float DodgeY { get; set; }
}
