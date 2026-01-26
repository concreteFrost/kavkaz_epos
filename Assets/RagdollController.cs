using UnityEngine;

public abstract class RagdollController : MonoBehaviour
{
   
    protected Animator anim;


    public abstract void EnableRagdoll();
    public abstract void DisableRagdoll();
   
}
