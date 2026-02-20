using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class Emitter : MonoBehaviour , IEmitter
{

    [SerializeField] protected Transform emitSource;
    [SerializeField] protected float skyOffset = 2.5f;

    protected ProjectileSO projectileSO;

    protected IDamagable target;
    protected IAttackSource attackSource;

    #region IEmitter Contract
    public bool IsEmitting { get; set; }
    public Transform Origin() => attackSource != null ? attackSource.Source() : transform;
    public IDamagable Target() => target;
    public ProjectileSO Projectile() => projectileSO;
    #endregion

    protected void SetTargetData(IDamagable target)
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
        Vector3 startPos = StartingPosition();
        GameObject clone = Instantiate(projectileSO.prefab, startPos, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();
        projectile.Init(new ProjectileData
        {
            target = target,
            attackSource = attackSource,
            projectileSO = projectileSO,
            direction = direction
        });
        return projectile;
    }

    private Vector3 StartingPosition()
    {
      
        switch (projectileSO.emitStartingPosition)
        {
            case EmitStartingPosition.Self:
                return EmitFromSource();
            case EmitStartingPosition.Ground:
                return EmitFromGround();
            case EmitStartingPosition.Sky:
                return EmitFromSky();
            default: return EmitFromSource();

        }

    }

    private Vector3 EmitFromSource() => emitSource.position + Origin().forward * 0.5f;

    private Vector3 EmitFromGround()
    {
        
        Vector3 startPos = emitSource.position + Origin().forward * 0.5f;
        startPos.y = skyOffset;

        Ray ray = new Ray(startPos, Vector3.down);   

        if(Physics.Raycast(ray,out RaycastHit hitInfo))
        {
           
            Vector3 ground = hitInfo.point;
            ground.y += 0.3f;
            return ground;
        }

        return emitSource.position;
    }

    private Vector3 EmitFromSky()
    {
        return emitSource.position +  Origin().up * skyOffset;
    }




}

