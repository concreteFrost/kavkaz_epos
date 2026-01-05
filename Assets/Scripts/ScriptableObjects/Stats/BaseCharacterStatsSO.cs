using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsSO", menuName = "Scriptable Systems/Character/Stats/Base")]
public class BaseCharacterStatsSO : ScriptableObject
{

    [Header("Health")]
    public float health = 1f;

    [Header("Stamina")]
    public float stamina = 1f;  

    [Header ("Speed")]
    public float walkSpeed = 2f;
    public float runningSpeed = 4f;
    public float sprintSpeed = 6f;

    [Header ("Jump")]

    [Tooltip("How much time the character will be jumping")]
    public float jumpTimer = 0.3f;
    [Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
    public float jumpHeight = 4f;

    [Header("Target Lock")]

    public float targetCheckDistance = 5f;
    public float targetResetDistance = 7f;

}
