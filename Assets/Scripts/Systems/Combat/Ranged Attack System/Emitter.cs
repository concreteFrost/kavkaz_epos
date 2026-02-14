using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour , IEmitter
{
  
    [SerializeField] ProjectileSO projectileSO;

    float spread = 0f;

    [SerializeField] protected Transform emitSource;
    protected Transform self;
    Transform target;


    #region IEmitter Contract

    public bool IsEmitting { get; set; }
    public Transform Origin() => transform;
    public Transform Target() => target;
    public ProjectileSO Projectile() => projectileSO;
    public float Spread { get => spread; set => spread = value; }
    #endregion

   
    public void Emit()
    {
        //if (IsEmitting) return;

        var attack = projectileSO.attackSO;

        attack.Execute(this);
        StartCoroutine(EmitCooldown(attack.cooldown));
    }

    public void SetEmitTarget(Transform target = null)
    {
        this.target = target;
    }

    public Coroutine EmitWithDelay(IEnumerator cor)
    {
        return StartCoroutine(cor);
    }

    public IProjectile NewProjectile(ProjectileData data)
    {
        Vector3 startPos = emitSource.position + self.forward * 0.5f;

        GameObject clone = Instantiate(projectileSO.prefab, startPos, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();

        projectile.Init(data);
        return projectile;
    }

    private IEnumerator EmitCooldown(float cooldown)
    {
        IsEmitting = true;
        yield return new WaitForSeconds(cooldown);
        IsEmitting = false;
    }

   

    

}

