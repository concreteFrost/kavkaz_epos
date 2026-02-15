using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class Emitter : MonoBehaviour , IEmitter
{
  
    protected ProjectileSO projectileSO;

    float spread = 0f;

    [SerializeField] protected Transform emitSource;
    protected Transform target;

    protected IAttackSource attackSource;


    #region IEmitter Contract
    public bool IsEmitting { get; set; }
    public Transform Origin() => attackSource != null ? attackSource.Source() : transform;
    public Transform Target() => target;
    public ProjectileSO Projectile() => projectileSO;
    public float Spread { get => spread; set => spread = value; }
    #endregion

    protected void SetTargetData(Transform target)
    {
        this.target = target;
    }

    public virtual void StartEmit()
    {
        IsEmitting = true;  
    }

    public virtual void Emit()
    {

        var attack = projectileSO.attackSO;
        attack.Execute(this);
       
    }

    public void EndEmit()
    {
        IsEmitting = false;
    }

    public Coroutine EmitWithDelay(IEnumerator cor)
    {
        return StartCoroutine(cor);
    }

    public IProjectile NewProjectile(ProjectileDirection direction)
    {
        Vector3 startPos = emitSource.position + Origin().forward * 0.5f;

        GameObject clone = Instantiate(projectileSO.prefab, startPos, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();

        ProjectileData data = new ProjectileData()
        {
           
            target = target,
            attackSource = attackSource,
            projectileSO = projectileSO,
            direction = direction,

        };

        projectile.Init(data);
        return projectile;
    }

   

   

    

}

