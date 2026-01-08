using UnityEngine;
using UnityEngine.AI;

public static class NavAgentUtils
{
    public static bool TryGetRandomReachablePoint(
        Vector3 origin,
        float radius,
        int maxAttempts,
        out Vector3 result)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 rnd = Random.insideUnitCircle * radius;
            Vector3 candidate =
                origin + new Vector3(rnd.x, 0f, rnd.y);

            if (NavMesh.SamplePosition(
                    candidate,
                    out NavMeshHit hit,
                    2f,
                    NavMesh.AllAreas))
            {
                if (HasCompletePath(origin, hit.position))
                {
                    result = hit.position;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }

    public static bool HasCompletePath(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();

        return NavMesh.CalculatePath(
                   from,
                   to,
                   NavMesh.AllAreas,
                   path)
               && path.status == NavMeshPathStatus.PathComplete;
    }
}
