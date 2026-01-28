using UnityEngine;
using UnityEngine.AI;


public class AiRagdollController : BaseRagdollController
{

    NavMeshAgent agent;
    

    public AiRagdollController(Animator anim, NavMeshAgent agent, Rigidbody[] rbs, Transform self)
    {
        this.agent = agent;
        base.Init(anim, rbs, self);
        
    }

    public override void EnableRagdoll()
    {
        anim.enabled = false;
        agent.ResetPath();
        agent.enabled = false;

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public override void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        anim.enabled = true;
        agent.enabled = true;
        agent.ResetPath();



    }

    public override void Knockout()
    {
        IsRecovering = true;
        EnableRagdoll();
    }


   
}

