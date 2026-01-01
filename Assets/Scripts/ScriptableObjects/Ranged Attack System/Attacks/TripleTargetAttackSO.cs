using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TripleTargetAttack", menuName = ProjectileConsts.PROJECTILE_ATTACK_PATH + "TripleTargetAttack")]
public class TripleTargetAttackSO : ProjectileAttackSO
{
    [SerializeField] float delay = 0.5f;

    [SerializeField] ProjectileAttackSO backupAttackSO;
    [SerializeField] ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {
        if(emitter.Target() == null)
        {
            backupAttackSO.Execute(emitter);
            return;
        }

        emitter.EmitWithDelay(DelayCoroutine(emitter));

    }

    IEnumerator DelayCoroutine(IEmitter emitter)
    {
        int spawnedAmount = 0; 
        while(spawnedAmount < 3 && emitter.Target() !=null)
        {
            Vector3 dir = (emitter.Origin().position - emitter.Target().position).normalized;

            var spread = base.SpreadRotation(emitter.Spread);
            dir = spread * dir;

            var data = emitter.Projectile().CreateData(moveSO, dir, emitter.Target());
            var bullet = emitter.NewProjectile(data);

            spawnedAmount++;
            yield return new WaitForSeconds(delay);
        }
    }
}
