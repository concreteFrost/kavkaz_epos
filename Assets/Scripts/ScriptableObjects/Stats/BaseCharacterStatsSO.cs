using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsSO", menuName = "Scriptable Systems/Character/Stats/Base")]
public class BaseCharacterStatsSO : ScriptableObject
{

    public CharacterType characterType;

    [Header("Health")]
    public float health = 100f;

    [Header("Stamina")]
    public float stamina = 100f;  

    [Header ("Speed")]
    public float walkSpeed = 1f;
    public float runningSpeed = 3f;
    public float strafeSpeed = 1f;  

    [Header("Jump")]
    [Tooltip("How much time the character will be jumping")]
    public float jumpTimer = 0.3f;
    [Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
    public float jumpHeight = 4f;



}
