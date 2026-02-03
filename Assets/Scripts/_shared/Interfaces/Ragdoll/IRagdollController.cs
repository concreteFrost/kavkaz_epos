using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface IRagdollController
{
    void EnableRagdoll(float force=0, Transform from=null);
    void DisableRagdoll();

    void Knockout(float force, Transform from);
    void ForceStop();

    IEnumerator Recover();

    bool IsKnockedOut { get; set; }

    //event Action Recovered;
    event Action RecoveredInInvalidArea;

}
