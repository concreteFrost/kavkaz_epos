using System;
using System.Collections;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
  
    [SerializeField] Animator anim;
    [SerializeField] AnimationInfoSO animationInfoSO;
    [SerializeField] GunEmitter emitter;

    [SerializeField] GameObject dummyProjectile;

    [Header("Launch Limits")]

    [SerializeField, Range(0f, 180f)] float launchAngle = 90f;
    [SerializeField, Min(0f)] float minLaunchDistance = 3f;
    [SerializeField, Min(0.1f)] float gizmoLength = 15f;

    [HideInInspector] public float minCooldown = 3;
    [HideInInspector] public float maxCooldown = 5;
   
    Coroutine coolDownCoroutine;

    bool canLaunch = true;

    IDamagable currTarget;
   

    public AnimationInfoSO AnimationInfo() => animationInfoSO;
  
    public bool CanLaunch() => canLaunch;

    private void Start()
    {
        dummyProjectile.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            var pl = FindAnyObjectByType<PlayerServiceLocator>();
            if (pl != null)
            {
                if (!IsTargetInLaunchRange(pl.GetComponentInChildren<IDamagable>())) return;
                TryLaunch(pl.GetComponentInChildren<IDamagable>());
            }
               
        }
    }

    public void TryLaunch(IDamagable d)
    {
        if (!canLaunch) return;

        currTarget = d;

        anim.SetTrigger("Launch");
        dummyProjectile.SetActive(true);

        if(coolDownCoroutine == null)
        {
            coolDownCoroutine = StartCoroutine(CooldownCoroutine());
        }
        
    }

    IEnumerator CooldownCoroutine()
    {
        canLaunch = false;

        float cooldown = UnityEngine.Random.Range(minCooldown, maxCooldown);
        yield return new WaitForSeconds(cooldown);
        canLaunch = true;
        coolDownCoroutine = null;
        currTarget = null;
    }

    #region Animator Controls
    public void PlayLaunchSound()
    {
      
    }

    public void Launch()
    {
        dummyProjectile.SetActive(false);
        emitter.EmitOnTarget(currTarget);
        //
    }

    #endregion

    public bool IsTargetInLaunchRange(IDamagable target)
    {
        if (target == null || target.IsDead) return false;

        Transform targetTransform = target.GetAimTransform();
        if (targetTransform == null) return false;

        
        Vector3 offset = targetTransform.position - transform.position;
        if (offset.sqrMagnitude < minLaunchDistance * minLaunchDistance ||
            offset.sqrMagnitude < Mathf.Epsilon) return false;

        return Vector3.Angle(transform.forward, offset) <= launchAngle * 0.5f;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Transform origin = aimOrigin != null ? aimOrigin : transform;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(origin.position, minLaunchDistance);

    //    Gizmos.color = Color.yellow;
    //    float length = Mathf.Max(gizmoLength, minLaunchDistance);
    //    float halfAngle = launchAngle * 0.5f * Mathf.Deg2Rad;
    //    Vector3 center = origin.position + origin.forward * (Mathf.Cos(halfAngle) * length);
    //    float radius = Mathf.Sin(halfAngle) * length;
    //    Vector3 previous = center + origin.right * radius;

    //    const int segments = 48;
    //    for (int i = 1; i <= segments; i++)
    //    {
    //        float angle = i * Mathf.PI * 2f / segments;
    //        Vector3 point = center +
    //            (origin.right * Mathf.Cos(angle) + origin.up * Mathf.Sin(angle)) * radius;
    //        Gizmos.DrawLine(previous, point);
    //        if (i % (segments / 4) == 0)
    //            Gizmos.DrawLine(origin.position, point);
    //        previous = point;
    //    }

    //    Gizmos.DrawRay(origin.position, origin.forward * length);
    //    if (currTarget != null && currTarget.GetAimTransform() != null)
    //    {
    //        Gizmos.color = IsTargetInLaunchRange(currTarget) ? Color.green : Color.red;
    //        Gizmos.DrawLine(origin.position, currTarget.GetAimTransform().position);
    //    }
    //}


}

