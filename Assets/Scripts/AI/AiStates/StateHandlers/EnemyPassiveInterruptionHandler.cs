using UnityEngine;

[System.Serializable]   
public class EnemyPassiveInterruptionHandler
{

    [SerializeField] private bool isInterrupted = false;

    public bool IsInterrupted() => isInterrupted;

    public void OnDamageTaken(Transform attackSource) { 
        
        //if(attackSource == null) return;
        Debug.Log(attackSource);    
        //var sourcePosition = attackSource.position;
        isInterrupted = true;

    }

    public void React(Animator animator)
    {
        animator.CrossFade("Look Around Start", 0f, 0);
        isInterrupted = false;
    }
}
