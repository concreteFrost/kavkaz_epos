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
        GizmoDrawer.DrawWithCube(Color.red, transform, transform.localScale);
    }
}
