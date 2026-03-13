using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public IDamagable target;
    public IAttackSource source;
    public Vector3 baseDir;

    public float speed;
    public float lifetime;
    public DamageData damageData;
    public ProjectileMoveSO moveSO;


}