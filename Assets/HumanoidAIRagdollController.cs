using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class HumanoidAIRagdollController : MonoBehaviour, IRagdollController
{
    Animator anim;
    Rigidbody[] rigidbodies;
    NavMeshAgent agent;
    Transform self;

    public System.Action KnockedOut;
    public void Init(Animator anim, NavMeshAgent agent, Rigidbody[] rbs, Transform self)
    {
        this.self = self;
        this.anim = anim;
        this.agent = agent;
       
        rigidbodies = rbs;

      
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        KnockedOut?.Invoke();
        anim.enabled = false;
        
        agent.ResetPath();
        agent.enabled = false;
        foreach (var rigidbody in rigidbodies)
        {
            //rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
      
    }

    public void DisableRagdoll()
    {
        anim.enabled = true;
        
        agent.enabled = true;
        agent.ResetPath();

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;   
        }
       
    }

    public void Knockout(Vector3 dir, float force)
    {
       
        EnableRagdoll();

        foreach(var rb in rigidbodies)
        {
            Vector3 randomOffset = new Vector3(
                  Random.Range(-.5f, .5f),
                 Random.Range(-.5f, .5f),
                 Random.Range(-.5f, .5f));

            rb.AddForce((dir + randomOffset) * force, ForceMode.Impulse);
        }

        StartCoroutine(RecoverFromKnockoutWhenStopped());
    }

    private IEnumerator RecoverFromKnockoutWhenStopped(float minTime = 1f, float threshold = 0.1f)
    {
        yield return new WaitForSeconds(minTime);

        bool moving = true;
        while (moving)
        {
            moving = false;
            foreach (var rb in rigidbodies)
            {
                if (rb.linearVelocity.sqrMagnitude > threshold * threshold) // заменили linearVelocity
                {
                    moving = true;
                    break;
                }
            }
            yield return null;
        }

       
        SyncTransformPosition();    
        DisableRagdoll();
        anim.CrossFade("Get Up", 0f);


    
    }

    private void SyncTransformPosition()
    {
        if (rigidbodies.Length == 0) return;

        Vector3 center = Vector3.zero;

        foreach(var rb in rigidbodies)
            center += rb.position;

        center /= rigidbodies.Length;

        self.position = center;    
    }


    private void MoveToNavMesh()
    {
        if (NavMesh.SamplePosition(self.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            self.position = hit.position;
            self.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal));
        }
    }


}
