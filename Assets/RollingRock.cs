using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RollingRock : MonoBehaviour
{
    [SerializeField] private float stopVelocity = 0.1f;
    [SerializeField] private float stopAngularVelocity = 0.1f;
    private PhysicsMaterial rockMaterial;

    MeshRenderer mesh;
    DamageCollider damageCollider;
    Collider col;

    [SerializeField] DamageData damageData;
    [SerializeField] List<CharacterType> targetsToIgnore;

    private Rigidbody rb;
    private bool activated;

    Vector3 initialPos;
    private Quaternion initialRotation;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();  
        col = GetComponent<Collider>(); 
        damageCollider = GetComponentInChildren<DamageCollider>();
        damageCollider.Init();
        damageData.SetFinalDamage(10, 100);

        initialPos = transform.position;
        initialRotation = transform.rotation;

        rockMaterial = new PhysicsMaterial
        {
            bounciness = 0.2f,
            dynamicFriction = 0.8f,
            staticFriction = 0.9f,
            bounceCombine = PhysicsMaterialCombine.Maximum,
            frictionCombine = PhysicsMaterialCombine.Maximum
        };

        col.material = rockMaterial;
    }

    public void ActivateRock()
    {
        rb.mass = 100f;
        rb.linearDamping = 0.2f;
        rb.angularDamping = 2f;
        rb.useGravity = true;
        rb.isKinematic = false;
        col.enabled = true;

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints = RigidbodyConstraints.None;

        StartCoroutine(DelayedActivation());
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(0.5f);
        activated = true;
    }

    public void ResetRock()
    {
        if(hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);   
            hideCoroutine = null;   
        }

        transform.SetPositionAndRotation(initialPos, initialRotation);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = true;

        damageCollider.DisableCollider();
        damageCollider.isAttackRegistered = false;  
        mesh.enabled = true;

        
        activated = false;
    }

    private void FixedUpdate()
    {
        if (!activated) return;

        bool isMoving =
            rb.linearVelocity.sqrMagnitude > stopVelocity * stopVelocity ||
            rb.angularVelocity.sqrMagnitude > stopAngularVelocity * stopAngularVelocity;

        if (isMoving && !damageCollider.isAttackRegistered)
        {
            if (!damageCollider.isAttackRegistered)
            {
                damageCollider.EnableCollider(damageData, targetsToIgnore, null);
            }
            else
            {
                damageCollider.DisableCollider();
            }

        }

        if( !isMoving)
        {
            Debug.Log("stopped");

            if (hideCoroutine == null)
            {
                hideCoroutine = StartCoroutine(DisableRockCoroutine());
            }
        }



    }


    IEnumerator DisableRockCoroutine()
    {
        activated = false;

        damageCollider.DisableCollider();
        yield return new WaitForSeconds(5f);
        mesh.enabled = false;
        col.enabled = false;    
        hideCoroutine = null;
    }
}