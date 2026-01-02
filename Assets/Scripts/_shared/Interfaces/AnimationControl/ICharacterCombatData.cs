
public interface ICharacterCombatData
{
    //bool IsAttacking { get; set; }
    bool IsWeaponed { get; set; }
    bool IsShieldRaised { get; }
    void EndAttack();
    void TryStartNextAttackFromQueue();
}
