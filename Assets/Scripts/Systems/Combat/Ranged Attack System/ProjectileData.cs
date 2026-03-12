using System.Collections.Generic;
using UnityEngine;

public struct ProjectileData
{

    public IDamagable target;
    public Transform source;

    public ProjectileDirection direction;
    public IAttackSource attackSource;

    public ProjectileSO projectileSO;

}
