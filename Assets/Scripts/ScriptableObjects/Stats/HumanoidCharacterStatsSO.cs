using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanoidCharacterStatsSO", menuName = "Scriptable Systems/Character/Stats/Humanoid")]
public class HumanoidCharacterStatsSO : BaseCharacterStatsSO
{
    [Header("Behaviours")]
    [Header("Idle")]

    [RangeAttribute(3, 6)]
    public int minIdleStationary;

    [RangeAttribute(6, 20)]
    public int maxIdleStationary;
}
