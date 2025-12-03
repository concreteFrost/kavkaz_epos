using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsSO", menuName = "Scriptable Systems/Character/CharacterStatsSO")]
public class CharacterStatsSO : ScriptableObject
{


    [Header ("Speed")]
    public float walkSpeed = 2f;
    public float runningSpeed = 4f;
    public float sprintSpeed = 6f;

    [Header ("Jump")]

    [Tooltip("How much time the character will be jumping")]
    public float jumpTimer = 0.3f;
    [Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
    public float jumpHeight = 4f;


}
