using UnityEngine;

public interface IRagdollController
{
    void EnableRagdoll();

    void DisableRagdoll();

    void Knockout(Vector3 dir, float force= 5f);
}
