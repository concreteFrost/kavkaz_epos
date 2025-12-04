using UnityEngine;

public class DefenceCollider : MonoBehaviour
{
    Collider col;
    IAttackSource owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider>();
        DisableCollider();
    }

    public void SetOwner(IAttackSource source)
    {
        owner =source;
    }

    public void ResetOwner()
    {
        owner = null;
    }

    public void EnableCollider()
    {

        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
        
    }
}
