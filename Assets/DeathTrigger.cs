using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeMaxDamage();  
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f); // красный с прозрачностью

        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
