using System;
using System.Collections;
using UnityEngine;

public interface IRagdollController
{
    void EnableRagdoll(Vector3 from,float force=0);
    void DisableRagdoll();

    void Knockout(Vector3 from, float force=0);
    void ForceStop();

    IEnumerator Recover();

    bool IsKnockedOut { get; set; }

    //event Action Recovered;
    event Action RecoveredInInvalidArea;

}
