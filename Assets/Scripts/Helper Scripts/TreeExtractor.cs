using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранит настройки извлечения деревьев Terrain в объекты сцены через редактор.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Terrain))]
public class TreeExtractor : MonoBehaviour
{
    [SerializeField, InspectorName("Родитель деревьев")]
    [Tooltip("Родительский Transform для извлечённых экземпляров префабов в той же сцене.")]
    private Transform parentTransform;

    [SerializeField, InspectorName("Префабы деревьев для извлечения")]
    [Tooltip("Префабы, отмеченные в списке экземпляров Terrain. Остальные объекты останутся в Terrain.")]
    private List<GameObject> treePrefabs = new List<GameObject>();

    public Transform ParentTransform => parentTransform;
    public IReadOnlyCollection<GameObject> TreePrefabs => treePrefabs;
}
