using System.Collections.Generic;
using UnityEngine;

public class AttackSource : MonoBehaviour, IAttackSource
{
    /// <summary>
    /// Позиция источника атаки
    /// </summary>
    private Transform sourcePosition;

    /// <summary>
    /// Уникальный идентификатор нападающего
    /// </summary>
    private int sourceId;

    [SerializeField] AttackSourceSO initialTargetsToIgnore;
    /// <summary>
    /// Цели для игнорирования во время атаки
    /// </summary>
    private List<CharacterType> targetsToIgnore = new List<CharacterType>();

    public int SourceId() => sourceId;
    public Transform Source() => sourcePosition;

    public List<CharacterType> TargetsToIgnore { get => targetsToIgnore; set => targetsToIgnore = value; }

    public void Init(Transform sourcePosition, int sourceId)
    {

        targetsToIgnore = new List<CharacterType>(initialTargetsToIgnore.characterTypes);
        this.sourcePosition = sourcePosition;
        this.sourceId = sourceId;
     
    }

}
