using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{

    public Transform target;
    public Transform source;

    public ProjectileDirection direction;
    public IAttackSource attackSource;

    public ProjectileSO projectileSO;

}
