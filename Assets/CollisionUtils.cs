using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public static class CollisionUtils
{
    public static Collider[] GetCollisionInfo(Transform origin, Collider col)
    {
        switch (col)
        {
            case BoxCollider box:
                return Physics.OverlapBox(origin.position + box.center, box.size * 0.5f, origin.rotation);
               
            case SphereCollider sphere:
                return Physics.OverlapSphere(origin.position + sphere.center, sphere.radius);
               
            case CapsuleCollider capsule:
                Vector3 point1 = origin.position + capsule.center + Vector3.up * (capsule.height / 2 - capsule.radius);
                Vector3 point2 = origin.position + capsule.center - Vector3.up * (capsule.height / 2 - capsule.radius);
                return Physics.OverlapCapsule(point1, point2, capsule.radius);
               ;
            default:
                // Для MeshCollider можно использовать Physics.OverlapBox(mesh.bounds) или ComputePenetration
                Debug.LogWarning("Collider type not handled!");
                return null;
                
        }
    }
}
