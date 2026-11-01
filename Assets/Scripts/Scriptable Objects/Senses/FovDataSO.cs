using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FOV Data", menuName = ScriptablePaths.SENSES_PATH + "/FOV/FOV Data")]
public class FovDataSO : ScriptableObject
{
    public float viewRadius = 20f;
    public float viewAngle = 70f;

    public LayerMask obstacleMask;
    public List<CharacterType> objectsToScan = new List<CharacterType>();

    public LayerMask layerToIgnore;

    [Min(0f)]
    [Tooltip("Интервал поиска новой цели в секундах. Ноль включает поиск каждый кадр; проверка видимости текущей цели выполняется отдельно.")]
    public float targetCheckInterval = 0.2f;

    [Min(1)]
    [Tooltip("Начальная вместимость буфера поиска. При заполнении буфер автоматически увеличивается, чтобы не пропускать цели.")]
    public int initialScanBufferSize = 64;
}
