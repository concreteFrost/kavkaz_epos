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
    [Tooltip("Боковое смещение выбросов влево и вправо, в метрах. Ноль отключает зигзаг.")]
    [SerializeField, Min(0f)] private float zigzagAmplitude = 0.25f;
    [Tooltip("Доля поворота линии атаки к цели. Ноль отключает помощь в прицеливании.")]
    [SerializeField, Range(0f, 1f)] private float aimAssistStrength = 0.35f;
    [Tooltip("Максимальный угол коррекции направления к цели, в градусах. Цели позади игнорируются.")]
    [SerializeField, Range(0f, 90f)] private float maxAimCorrectionAngle = 15f;
    [Tooltip("Максимальный спуск поверхности относительно предыдущего выброса, в метрах.")]
    [SerializeField, Min(0f)] private float groundProbeHeight = 2f;
    [Tooltip("Максимальный подъём поверхности относительно предыдущего выброса, в метрах.")]
    [SerializeField, Min(0f)] private float maxGroundRise = 0.5f;
    [Tooltip("Минимальная вертикальная составляющая нормали поверхности. Исключает стены и нижние стороны перекрытий.")]
    [SerializeField, Range(0f, 1f)] private float minGroundNormalY = 0.5f;
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
        var target = emitter.Target();
        if (target != null && aimAssistStrength > 0f)
        {
            Transform aimTransform = target.GetAimTransform();
            if (aimTransform != null)
            {
                Vector3 targetDirection = aimTransform.position - origin;
                targetDirection.y = 0f;
                if (targetDirection.sqrMagnitude > 0.0001f
                    && Vector3.Dot(forward, targetDirection) > 0f)
                {
                    targetDirection.Normalize();
                    float correctionAngle = Mathf.Min(
                        Vector3.Angle(forward, targetDirection) * aimAssistStrength,
                        maxAimCorrectionAngle);
                    forward = Vector3.RotateTowards(forward, targetDirection,
                        correctionAngle * Mathf.Deg2Rad, 0f);
                }
            }
        }

        Vector3 right = Vector3.Cross(Vector3.up, forward);
        float groundHeight = origin.y;
        float probeDistance = maxGroundRise + groundProbeHeight;
        var delay = spawnDelay > 0f ? new WaitForSeconds(spawnDelay) : null;
        for (int i = 0; i < count; i++)
        {
            Vector3 point = origin + forward * (firstDistance + spacing * i);
            float lateralOffset = (i % 2 == 0 ? -zigzagAmplitude : zigzagAmplitude);
            point += right * lateralOffset;
            point.y = groundHeight;
            Vector3 rayOrigin = point + Vector3.up * maxGroundRise;
            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit,
                    probeDistance, groundMask, QueryTriggerInteraction.Ignore)
                || hit.normal.y < minGroundNormalY)
                yield break;

            groundHeight = hit.point.y;

            projectile.CreateProjectile(
                hit.point,
                emitter.Target(),
                emitter.AttackSource(),
                Vector3.up,
                emitter.DamageMultiplier()
            );

            if (i < count - 1 && delay != null)
                yield return delay;
        }
    }
}
