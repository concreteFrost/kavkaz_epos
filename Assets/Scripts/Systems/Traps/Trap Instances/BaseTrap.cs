
using System;
using UnityEngine;

[Serializable]
public class TrapState
{
    public string id;
    public bool wasActivated;

    public bool wasDestroyed;
}

public abstract class BaseTrap : MonoBehaviour
{
    public bool wasActivated;
    public bool wasDestroyed;

    public string uid;

    [SerializeField] private UniqueId uniqueId;

    public virtual void Init()
    {

        uid = uniqueId.GetComponent<UniqueId>().uniqueId;
        ResetState();   
    }

    public virtual void Activate()
    {
        wasActivated = true;
    }

    public abstract void Deactivate();  

    public virtual void ResetState()
    {
        wasActivated = false;
        wasDestroyed = false;
    }

    public void LoadState(bool wasActivated, bool wasDestroyed)
    {
        this.wasActivated = wasActivated;
        this.wasDestroyed = wasDestroyed;

        // Destruction is terminal and must take precedence over the saved
        // activated state (a trap can be both activated and destroyed).
        if (wasDestroyed)
        {
            Deactivate();
            this.wasDestroyed = true;
            Destroy();
            return;
        }

        if (wasActivated)
        {
            Deactivate();
            return;
        }

        ResetState();
    }

    protected virtual void Destroy() => Deactivate();

   
}
