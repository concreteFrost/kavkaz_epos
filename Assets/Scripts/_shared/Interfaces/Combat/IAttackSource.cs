using System.Collections.Generic;
using UnityEngine;

public interface IAttackSource
{
    /// <summary>
    /// Ќазначает источник атаки дл€ предотвращени€ попадани€ по самому себе
    /// </summary>
    /// <returns></returns>
    public int SourceId();

    /// <summary>
    /// ”казывает на источник атаки
    /// </summary>
    /// <returns></returns>
    public Transform Source();

    /// <summary>
    /// ÷ели которые стоит игнорировать. Ќапример дружественные NPC или же цели того же класса
    /// </summary>
    public List<CharacterType> TargetsToIgnore { get; set; }
}
