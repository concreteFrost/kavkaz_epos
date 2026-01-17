using UnityEngine;

[System.Serializable]   
public class EnemyPassiveInterruptionHandler
{
    private Vector3 interruptionDir;
    private float interruptionTimer = 0;
    private float maxInterruptionTimer = 2f;
    [SerializeField] private bool isInterrupted = false;


    public Vector2 GetInterruptionDirection() => interruptionDir;

    public void Interrupt(Vector3 dir)
    {
        interruptionDir = dir;
        isInterrupted = true;
        interruptionTimer = 0f;
    }

    public void UpdateInterruption()
    {
        interruptionTimer += Time.deltaTime;

        if (interruptionTimer >= maxInterruptionTimer)
        {
            ResetInterruption();
        }
    }

    public void ResetInterruption()
    {
        isInterrupted = false;
        interruptionTimer = 0f;
        interruptionDir = Vector3.zero;
    }

    public bool IsInterrupted() => isInterrupted;

    public void OnDamageTaken(Transform attackSource) { 
        
        if(attackSource == null) return;

        var sourcePosition = attackSource.position; 

        interruptionDir = sourcePosition;
        isInterrupted = true;
        interruptionTimer = 0f;
    }
}
