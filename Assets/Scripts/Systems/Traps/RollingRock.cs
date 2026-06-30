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
    [SerializeField] private bool activated;

    Vector3 initialPos;
    private Quaternion initialRotation;

    private Coroutine hideCoroutine;
    private Coroutine activationCoroutine;

    public void Init()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        damageCollider = GetComponentInChildren<DamageCollider>();

        damageCollider.Init();

        rockMaterial = new PhysicsMaterial
        {
            bounciness = 0.2f,
            dynamicFriction = 0.8f,
            staticFriction = 0.9f,
            bounceCombine = PhysicsMaterialCombine.Maximum,
            frictionCombine = PhysicsMaterialCombine.Maximum
        };

        col.material = rockMaterial;

        activated = false;

    }

    private void Update()
    {
        if (!activated) return;

        bool isMoving =
            rb.linearVelocity.sqrMagnitude > stopVelocity * stopVelocity ||
            rb.angularVelocity.sqrMagnitude > stopAngularVelocity * stopAngularVelocity;

        if (isMoving)
        {
            damageCollider.EnableCollider(damageData, null, null);
        }

        if ( !isMoving)
        {
           

            if (hideCoroutine == null)
            {
                hideCoroutine = StartCoroutine(DisableRockCoroutine());
            }
        }


    }


    public void ActivateRock(float impulseForce, Vector3 fwd)
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

        rb.AddForce(fwd * impulseForce, ForceMode.Impulse);
        rb.AddTorque(transform.right * 10f, ForceMode.Impulse);

        damageData.SetFinalDamage(70, 100);

        activationCoroutine = StartCoroutine(DelayedActivation());
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(0.7f);
        activated = true;
    }

    public void ResetRock()
    {
        if(hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);   
            hideCoroutine = null;   
        }

        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
            activationCoroutine = null;
        }

        transform.SetPositionAndRotation(initialPos, initialRotation);

        rb.useGravity = false;
        rb.isKinematic = true;
        col.enabled = true;

        damageCollider.DisableCollider();
        mesh.enabled = true;
        activated = false;
    }

    public void HideRock()
    {
        mesh.enabled = false;
        col.enabled = false;
    }

  

    IEnumerator DisableRockCoroutine()
    {
        activated = false;

        damageCollider.DisableCollider();
        yield return new WaitForSeconds(5f);
        HideRock();  
        hideCoroutine = null;
    }
}