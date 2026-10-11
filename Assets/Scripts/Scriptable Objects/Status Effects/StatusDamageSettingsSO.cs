using UnityEngine;

[CreateAssetMenu(fileName = "status_damage_settings", menuName = ScriptablePaths.STATUS_FX_PATH + "/Параметры периодического урона")]
public class StatusDamageSettingsSO : ScriptableObject
{
    [Min(0f), Tooltip("Урон в секунду для горения, скверны и обморожения.")]
    public float damagePerSecond;

    [Min(0f), Tooltip("Параметр длительности эффекта. При накоплении используется существующая механика активации и затухания.")]
    public float duration;
}
