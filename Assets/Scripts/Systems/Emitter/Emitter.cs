using System.Collections;
using UnityEngine;

public abstract class Emitter : MonoBehaviour , IEmitter
{

    protected Transform emitSource;
    [SerializeField] protected float skyOffset = 4f;

    protected ProjectileSO projectileSO;

    protected IDamagable target;
    protected IAttackSource attackSource;

    protected float damageMultiplier = 100f;

    #region IEmitter Contract
    public bool IsEmitting { get; set; }
    public Transform Origin() => attackSource != null ? attackSource.Source() : transform;
    public IDamagable Target() => target;
    public IAttackSource AttackSource() => attackSource;
    public ProjectileSO Projectile() => projectileSO;
    public float DamageMultiplier() => damageMultiplier;

    public Vector3 StartingPosition()
    {

        Vector3 selfPosition = emitSource.position + Origin().forward * 0.5f;
        Vector3 skyPosition = emitSource.position + Vector3.up * skyOffset;

        switch (projectileSO.emitStartingPosition)
        {
            case EmitStartingPosition.Self:
                return selfPosition;
            case EmitStartingPosition.Sky:
                return skyPosition;
            default: return selfPosition;

        }

    }


    #endregion

    protected void SetTargetData(IDamagable target)
    {
        this.target = target;
    }



    protected void SetDamageMultiplier(float multiplier)=> damageMultiplier = multiplier;

    public virtual void StartEmit() => IsEmitting = true;
    public void EndEmit() => IsEmitting = false;

    private void OnValidate()
    {
        skyOffset = 4f;
    }

    public virtual void Emit()
    {
       
        projectileSO.attackSO.Execute(this,projectileSO.amountToSpawn,projectileSO.spawnDelay);
        AudioEventPlayer.Play3DOneShot(projectileSO.ev_audio, gameObject, "ProjectileState",0);
       
    }

    public Coroutine EmitWithDelay(IEnumerator cor)
    {
        return StartCoroutine(cor);
    }


   




}

