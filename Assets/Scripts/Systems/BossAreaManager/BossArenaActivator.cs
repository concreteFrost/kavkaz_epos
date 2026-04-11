using System;
using UnityEngine;

public class BossArenaActivator : MonoBehaviour
{
    public Action ArenaEntered;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerManager>() != null)
        {
           
            ArenaEntered?.Invoke();
        }
           
    }

    private void OnDrawGizmos()
    {
        var color = Color.red;

        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
