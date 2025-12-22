using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public enum PlayerMode
{
    Locomotion = 0,
    Climbing = 1
}
public class PlayerActionGuards
{
    readonly PlayerMotor locomotion;
    readonly HumanoidCombatController combat;
    readonly PlayerStats stats;
    readonly PlayerStatsModifier statsModifier;

    PlayerMode mode;

    public PlayerActionGuards(
        PlayerMotor locomotion,
        HumanoidCombatController combat,
        PlayerStats stats,
        PlayerStatsModifier statsModifier,
        PlayerMode initialMode = PlayerMode.Locomotion)
    {
        this.locomotion = locomotion;
        this.combat = combat;
        this.stats = stats;
        this.statsModifier = statsModifier;
        this.mode = initialMode;
    }

    public PlayerMode Mode => mode;

    public bool CanMove()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if (locomotion.StopMove) return false;

        if(locomotion.isHighSlope) return false;

        if (statsModifier.IsDamaged) return false;

        if (locomotion.IsDodging) return false;

        if (combat.IsAttacking) return false;

        if(combat.isThrowingWeapon) return false;


        return true;
    }

    public bool CanRotate(Vector3 input)
    {
        if (mode != PlayerMode.Locomotion) return false;

        if(input.sqrMagnitude < 0.01f) return false;

        //if (combat.BlockRotation) return false;

        if(statsModifier.IsDamaged) return false;

        return true;
    }

    public bool CanJump()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if (!locomotion.IsGrounded) return false;

        if (locomotion.IsJumping) return false;

        if (locomotion.StopMove) return false;

        if (locomotion.GroundAngle() >= locomotion.slopeLimit) return false;

        if (stats.currentStamina <= 0) return false;


        return true;
    }

    public bool CanDodge()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if (!locomotion.IsGrounded) return false;

        if (locomotion.IsDodging) return false;

        if(combat.IsAttacking) return false;

        if (locomotion.StopMove) return false;

        if (stats.currentStamina <= 0) return false;


        return true;
    }

    public bool CanSprint(bool sprintHeld)
    {
        if (!sprintHeld) return false;
     
        if (mode != PlayerMode.Locomotion) return false;

        if (!locomotion.IsGrounded) return false;

        if (combat.IsAttacking) return false;

        if (statsModifier.IsDamaged) return false;

        if (stats.currentStamina <= 0) return false;

        Vector3 localDir = locomotion.transform.InverseTransformDirection(locomotion.MoveDirection);
        bool isMovingForward = localDir.z > 0.1f;
       
        if (!isMovingForward) return false;

        return true;
    }

    public bool CanAttack()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if (locomotion.IsDodging) return false;

        if (!locomotion.IsGrounded) return false;

        if (stats.currentStamina <= 0) return false;


        return true;
    }

    public bool CanThrowWeapon()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if(statsModifier.IsDamaged) return false;

        if(combat.isAttacking) return false;

        if(!locomotion.IsGrounded) return false;    

        return true;

    }

    public bool CanBlock()
    {
        if (mode != PlayerMode.Locomotion) return false;

        if (statsModifier.IsDamaged) return false;

        if(combat.isAttacking)  return false;

        if (stats.currentStamina <= 0) return false;

        return true;
    }

    public bool CanInteract()
    {
        if(mode != PlayerMode.Locomotion) return false;

        if(combat.isAttacking) return false;

        return true;
    }

}
