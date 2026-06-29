
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    public bool wasActivated;

    public string uid;

    [SerializeField] private UniqueId uniqueId;


    private void Start()    
    {
        Init();
    }
    public virtual void Init()
    {
        uid = uniqueId.GetComponent<UniqueId>().uniqueId;
        Deactivate();
    }

    public virtual void Activate()
    {
        wasActivated = true;
        Debug.Log("activating trap");
    }

    public virtual void Deactivate()
    {
        wasActivated = false;
    }
   
}
