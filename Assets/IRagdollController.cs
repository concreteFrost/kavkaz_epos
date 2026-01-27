using System.Collections;
using UnityEngine;

public interface IRagdollController
{
    void EnableRagdoll();
    void DisableRagdoll();

    void Knockout();

    IEnumerator Recover();

    bool IsRecovering { get; set; }

}
