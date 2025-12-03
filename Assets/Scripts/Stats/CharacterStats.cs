using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public CharacterStatsSO statsSO;

    [Header("movement speed")]
    public float walkSpeed;
    public float runningSpeed;

    [Header("jumping")]
    public float sprintSpeed;
    public float jumpHeight;
    public float jumpTimer;


    public virtual void Init()
    {
        walkSpeed = statsSO.walkSpeed;
        runningSpeed = statsSO.runningSpeed;
        sprintSpeed = statsSO.sprintSpeed;
        jumpHeight = statsSO.jumpHeight;
        jumpTimer = statsSO.jumpTimer;
    }
}
