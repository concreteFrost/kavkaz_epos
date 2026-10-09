using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ProjectileAttack_GroundTrace",
    menuName = ScriptablePaths.PROJECTILE_ATTACK_PATH + "/Ground Trace Attack"
)]
public class GroundTraceAttackSO : ProjectileAttackSO
{
    //[SerializeField, Range(1, 10)] private int maxProjectiles = 10;
    [SerializeField, Min(0f)] private float firstDistance = 1f;
    [SerializeField, Min(0f)] private float spacing = 1f;
    [SerializeField, Min(0f)] private float groundProbeHeight = 20f;
    [SerializeField] private LayerMask groundMask = ~0;

    public override void Execute(IEmitter emitter, int amount, float spawnDelay)
    {
       
        emitter.EmitWithDelay(TraceRoutine(emitter,amount, spawnDelay));
    }

    private IEnumerator TraceRoutine(IEmitter emitter, int count, float spawnDelay)
    {
        var projectile = emitter.Projectile();
        Vector3 forward = emitter.Origin().forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.0001f)
            forward = Vector3.forward;
        else
            forward.Normalize();

        Vector3 origin = emitter.Origin().position;
        for (int i = 0; i < count; i++)
        {
            Vector3 point = origin + forward * (firstDistance + spacing * i);
            Vector3 rayOrigin = point + Vector3.up * groundProbeHeight;
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundProbeHeight * 2f, groundMask))
                point = hit.point;

            projectile.CreateProjectile(
                point,
                emitter.Target(),
                emitter.AttackSource(),
                Vector3.up,
                emitter.DamageMultiplier()
            );

            if (i < count - 1 && spawnDelay > 0f)
                yield return new WaitForSeconds(spawnDelay);
        }
    }
}
