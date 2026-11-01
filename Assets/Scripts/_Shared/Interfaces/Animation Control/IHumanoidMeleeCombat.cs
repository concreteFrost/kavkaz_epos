
using System;

public interface IHumanoidMeleeCombat
{
    bool IsAttacking { get; set; }
    bool IsWeaponed { get; set; }
    bool IsShieldRaised { get; set; }
    void EndAttack();
    void ForceAttackCancel();
    void PerformAttack(bool loopCombo = false, Func<WeaponAttack, bool> canStartComboAttack = null);
    void PerformPowerAttack();
    void PerformBlock();
    void CancelBlock();
    void ResetCombo();

    float AttackBufferTime { get; set; }

    event Action OnAttackEnd;
}
