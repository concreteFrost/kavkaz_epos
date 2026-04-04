using System;
using UnityEngine;

public class BossArenaActivator : MonoBehaviour
{
    public Action ArenaEntered;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerManager>() != null)
        {
            Debug.Log("arena activated");
            ArenaEntered?.Invoke();
        }
           
    }
}
