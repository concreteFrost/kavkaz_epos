using FMODUnity;
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

    [HideInInspector] public float minCooldown = 3;
    [HideInInspector] public float maxCooldown = 5;
   
    Coroutine coolDownCoroutine;

    bool canLaunch = true;

    IDamagable currTarget;

    [Header("Аудио")]
    [SerializeField] private EventReference ev_charge;
   

    public AnimationInfoSO AnimationInfo() => animationInfoSO;
  
    public bool CanLaunch() => canLaunch;

    private void Start()
    {
        dummyProjectile.SetActive(false);
    }


    public void TryLaunch(IDamagable d)
    {
        if (!canLaunch) return;

        currTarget = d;

        anim.SetTrigger("Launch");
        AudioEventPlayer.Play3DOneShot(ev_charge, gameObject);
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

   

}

