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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.5f, .5f, 0f, 0.3f);

        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(
            transform.position,
            transform.rotation,
            transform.lossyScale);

        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = oldMatrix;
    }
}