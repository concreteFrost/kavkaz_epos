using UnityEngine;
using UnityEngine.AI;


public class AiRagdollController : BaseRagdollController
{

    HumanoidAgentController agentController;


    public AiRagdollController(MonoBehaviour ctx, Animator anim, HumanoidAgentController agent, Rigidbody[] rbs, Transform self)
    {
        this.agentController = agent;
        base.Init(ctx,anim, rbs, self);

        col = self.GetComponent<Collider>();    
        
    }

    public override void EnableRagdoll(float force, Transform from)
    {
        col.enabled = false;
        anim.enabled = false;

        agentController.DisableAgent();

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        ApplyImpulseFromSource(force,from);

     
    }

    public override void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        col.enabled = true;
        anim.enabled = true;

        if (agentController.IsOnBakedArea())
            agentController.EnableAgent();

        else
            InvokeInvalidRecover();

    }


   
}

