using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider col;
    string owner;

    bool isAttackRegistered = false;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void SetOwner(string _owner)
    {
        owner = _owner;
    }

    public void ResetOwner()
    {
        owner=null;
    }

    public void EnableCollider()
    {
        
        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
        isAttackRegistered = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!isAttackRegistered)
        {
            Debug.Log(other.name);
            isAttackRegistered = true;
        }
       
       
    }

    

}
