using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    Collider col;
    ICollector collector;
    IPickable pickable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>(); 
    }

    public void Init(IPickable item)
    {
        pickable = item;    
    }
   
    public void EnableCollider() => col.enabled = true;

    public void DisableCollider()=> col.enabled = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInChildren<ICollector>() != null)
        {
           
            var _collector = other.GetComponentInChildren<ICollector>();
            collector = _collector;
            collector.PickableItem = pickable;
          
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (collector != null) {

            collector.PickableItem = null;  
            collector = null;
        }
    }

}
