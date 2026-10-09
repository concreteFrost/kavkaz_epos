using UnityEngine;

[CreateAssetMenu(fileName = "Base Character Stats", menuName = ScriptablePaths.CHARACTER_STATS_PATH + "/Base")]
public class BaseCharacterStatsSO : ScriptableObject
{
    public string characterName;

    public CharacterType characterType;

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
