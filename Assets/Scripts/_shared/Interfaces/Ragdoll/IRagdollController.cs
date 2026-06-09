using System;
using System.Collections;
using UnityEngine;

public interface IRagdollController
{
    Transform GetHipsTransform();

    void EnableRagdoll(Vector3 from,float force=0);
    void DisableRagdoll();

    void Knockout(Vector3 from, float force=0);
    void ForceStop();

    IEnumerator Recover();

    bool IsKnockedOut { get; set; }
    bool IsBonesMoving(float threshold=0.1f);

    //event Action Recovered;
    event Action KnockedOut;
    event Action RecoveredInInvalidArea;
    event Action Recovered;

}
