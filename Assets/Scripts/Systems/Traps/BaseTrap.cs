
using System;
using UnityEngine;

[Serializable]
public class TrapState
{
    public string id;
    public bool wasActivated;
}

public abstract class BaseTrap : MonoBehaviour
{
    public bool wasActivated;

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
    }

    public void LoadState(bool wasActivated)
    {
        this.wasActivated = wasActivated;

        if (wasActivated)
        {
            Deactivate();
            return;
        }
        else
        {
            ResetState();
        }

        //ResetState();
    }
   
}
