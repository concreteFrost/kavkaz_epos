using UnityEngine;

public class HumanoidAttackBehaviour : StateMachineBehaviour
{

    IWeaponSetter inv;
    IHumanoidMeleeCombat combatAnimData;
    IHumanoidMovement motor;
    CharacterStatsController stats;
    IDamagable damageController;
    WeaponAttack attack;
    IWeapon weapon;
    IPushable pushable;
   
    bool hitActive = false;
    bool wasAudioPlayer = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = animator.GetComponentInChildren<IWeaponSetter>();
        combatAnimData = animator.GetComponentInChildren<IHumanoidMeleeCombat>();
        motor = animator.GetComponent<IHumanoidMovement>();
        stats = animator.GetComponentInChildren<CharacterStatsController>();
        damageController = animator.GetComponentInChildren<IDamagable>();
        pushable = animator.GetComponentInChildren<IPushable>();

        if(inv.CurrentWeapon != null)
        {
            
            stats.Stamina.ChangeCurrent(inv.CurrentWeapon.CurrentAttack().staminaPenalty,OperationType.Negative);
        }
        
        animator.applyRootMotion = true;
        hitActive = false;
        wasAudioPlayer = false; 

        // блокируем вращение персонажа во время атаки
        motor.BlockRotation = true;
        motor.StopMove = true;

        weapon = inv.CurrentWeapon;
        attack = weapon.CurrentAttack();
       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!animator.applyRootMotion) animator.applyRootMotion = true;
     
        if (damageController.IsDamaged || pushable.IsPushed) return;

        if (attack == null) return;

        //animator.speed = attack.animationInfo.animationSpeed;

        float t = stateInfo.normalizedTime;

     //   if (t >= attack.invincibleStartFrame &&
     //t <= attack.invincibleEndFrame)
     //   {
     //       damageController.CanPlayDamagedAnimation = false;
     //   }
     //   else
     //   {
     //       damageController.CanPlayDamagedAnimation = true;
     //   }

        if(!wasAudioPlayer && t>= attack.audioStartTime)
        {
            
            weapon.PlaySwing();
            wasAudioPlayer = true;
        }

        if (!hitActive && t >= attack.animationInfo.hitStartFrame)
        {
            weapon.PerformAttack();
            hitActive = true;
        }

        if (hitActive && t >= attack.animationInfo.hitEndFrame)
        {
            weapon.CancelAttack();
            hitActive = false;
        }

       
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;

        wasAudioPlayer = false;
        hitActive = false;
        motor.StopMove = false;

        inv.CurrentWeapon.CancelAttack();

        animator.applyRootMotion = false;
        motor.BlockRotation = false;
        damageController.CanPlayDamagedAnimation = true;    

        // уведомляем контроллер, что атака завершена
        combatAnimData.EndAttack();

    }
}
