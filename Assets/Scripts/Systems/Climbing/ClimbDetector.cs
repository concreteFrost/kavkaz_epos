using UnityEngine;


public class ClimbDetector : MonoBehaviour
{
    [SerializeField] float detectDistance = 0.3f;
    [SerializeField] LayerMask climbLayer;

    public bool TryGetClimbable(out IClimable climbable, out RaycastHit hit)
    {
        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            detectDistance,
            climbLayer))
        {
           
            climbable = hit.collider.GetComponent<IClimable>();
            return climbable != null;
        }

        climbable = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
