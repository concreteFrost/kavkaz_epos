using UnityEngine;
using UnityEngine.AI;

public static class NavAgentUtils
{
    public static bool TryGetRandomReachablePoint(
        Vector3 origin,
        int agentTypeId,
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
                if (HasCompletePath(origin, hit.position, agentTypeId))
                {
                    result = hit.position;
                    return true;
                }
            }
        }

        result = Vector3.zero;
        return false;
    }

    public static bool HasCompletePath(
        Vector3 from,
        Vector3 to,
        int agentTypeId)
    {
        NavMeshPath path = new NavMeshPath();

        NavMeshQueryFilter filter = new NavMeshQueryFilter
        {
            agentTypeID = agentTypeId,
            areaMask = NavMesh.AllAreas
        };

        return NavMesh.CalculatePath(
                   from,
                   to,
                   filter,
                   path)
               && path.status == NavMeshPathStatus.PathComplete;
    }

    public static float GetPathLength(
     Vector3 start,
     Vector3 end,
     int agentTypeId
        )
    {
        var filter = new NavMeshQueryFilter
        {
            agentTypeID = agentTypeId,
            areaMask = NavMesh.AllAreas
        };

        var path = new NavMeshPath();

        if (!NavMesh.CalculatePath(start, end, filter, path) ||
            path.status != NavMeshPathStatus.PathComplete)
        {
            return Mathf.Infinity;
        }

        float length = 0f;
        for (int i = 1; i < path.corners.Length; i++)
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);

        return length;
    }
}