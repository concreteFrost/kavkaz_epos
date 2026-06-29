using UnityEngine;

public class TrapActivator : MonoBehaviour
{

    [SerializeField] BaseTrap trap;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {

            if (trap.wasActivated) return;

            trap.Activate();    
        }
    }
}