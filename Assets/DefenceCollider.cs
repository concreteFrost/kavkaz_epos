using UnityEngine;

public class DefenceCollider : MonoBehaviour
{
    Collider col;
    string owner;
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
