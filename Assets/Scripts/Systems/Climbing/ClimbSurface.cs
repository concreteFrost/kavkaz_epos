using UnityEngine;

public class ClimbSurface : MonoBehaviour , IClimable
{
    Collider col;

    public float ClimbOffset()=>col.bounds.center.normalized.z;   

    private void Awake()
    {
        col = GetComponent<Collider>();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;
       
        Gizmos.DrawCube(Vector3.zero ,Vector3.one);
    }
}
