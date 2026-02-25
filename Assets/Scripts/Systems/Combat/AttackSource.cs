using System.Collections.Generic;
using UnityEngine;

public class AttackSource : MonoBehaviour, IAttackSource
{
    private Transform sourcePosition;

    /// <summary>
    /// Уникальный идентификатор нападающего
    /// </summary>
    private int sourceId;

    /// <summary>
    /// Цели для игнорирования во время атаки
    /// </summary>
    public List<CharacterType> targetsToIgnore = new List<CharacterType>();

    public int SourceId() => sourceId;
    public Transform Source() => sourcePosition;

    public List<CharacterType> TargetsToIgnore { get => targetsToIgnore; set => targetsToIgnore = value; }

    public void Init(Transform sourcePosition, int sourceId)
    {
        
        this.sourcePosition = sourcePosition;
        this.sourceId = sourceId;

     
    }

}
