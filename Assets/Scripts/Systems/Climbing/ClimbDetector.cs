using UnityEngine;
using static UnityEngine.UI.Image;


public class ClimbDetector : MonoBehaviour
{
    [SerializeField] float detectDistance = 0.3f;
    [SerializeField] LayerMask climbLayer;

    public bool TryGetClimbable(out IClimable climbable, out RaycastHit hit)
    {
        Debug.DrawRay(transform.position, transform.forward * detectDistance, Color.red);

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
