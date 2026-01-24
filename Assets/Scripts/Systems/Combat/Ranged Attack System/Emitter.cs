using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour , IEmitter
{
  
    [SerializeField] ProjectileSO projectileSO;

    float spread = 0f;
    private bool canAttack = true;

    Transform target;

    #region IEmitter Contract
    public Transform Origin() => transform;
    public Transform Target() => target;
    public ProjectileSO Projectile() => projectileSO;
    public float Spread { get => spread; set => spread = value; }
    #endregion

   
    public void Emit(Transform target = null)
    {
        if (!canAttack) return;

        this.target = target;

        var attack = projectileSO.attackSO;

        attack.Execute(this);
        StartCoroutine(EmitCooldown(attack.cooldown));
    }

    public Coroutine EmitWithDelay(IEnumerator cor)
    {
        return StartCoroutine(cor);
    }

    public IProjectile NewProjectile(ProjectileData data)
    {
        Vector3 startPos = transform.position + transform.forward * 0.5f;

        GameObject clone = Instantiate(projectileSO.prefab, startPos, Quaternion.identity);
        var projectile = clone.GetComponent<IProjectile>();

        projectile.Init(data);
        return projectile;
    }

    private IEnumerator EmitCooldown(float cooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }


}
