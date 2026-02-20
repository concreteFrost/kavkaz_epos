using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "TripleTargetAttack", menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/TripleTargetAttack")]
public class MultipleTargetAttackSO : ProjectileAttackSO
{
    [SerializeField] float delay = 0.5f;

    [SerializeField] ProjectileAttackSO backupAttackSO;
    [SerializeField] ProjectileMoveSO moveSO;
    public override void Execute(IEmitter emitter)
    {
        if(emitter.Target() == null)
        {
            if(backupAttackSO == this)
            {
                Debug.Log("cannot use the same attack as backup");
                return;
            }
            backupAttackSO.Execute(emitter);
            return;
        }

        emitter.EmitWithDelay(DelayCoroutine(emitter));

    }

    IEnumerator DelayCoroutine(IEmitter emitter)
    {
        int count = 0;
        int spawnAmount = emitter.Projectile().amountToSpawn;

        if(spawnAmount <= 0)
        {
            spawnAmount = 1;
        }
       
        while(count < spawnAmount && emitter.Target() !=null)
        {
           
            var dir = (emitter.Target().GetAimTransform().position - emitter.Origin().position).normalized;
            
            ProjectileDirection directionData = new ProjectileDirection()
            {
                MoveBehaviour = moveSO,
                baseDir = dir,
            };


            var bullet = emitter.NewProjectile(directionData);

            count++;
            yield return new WaitForSeconds(delay);
        }
    }
}
