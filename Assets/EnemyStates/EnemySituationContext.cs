using UnityEngine;

public class EnemySituationContext
{

    public Transform Target;
    public float Distance;
    public bool CanReach;
    public bool IsVisible;

    public void Update(EnemyBrainContext context)
    {
        if (context.fov.currentTarget == null)
        {
            Target = null;
            return;
        }

        Target = context.fov.currentTarget.GetOrigin();
        Distance = Vector3.Distance(context.self.position, Target.position);
        CanReach = NavAgentUtils.HasCompletePath(context.self.position, Target.position);
        IsVisible = context.fov.IsTargetVisible(
            context.fov.currentTarget.GetAimTransform()
        );
    }

    public void SetDestination()
    {

    }
}
