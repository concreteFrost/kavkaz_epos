using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LockableDummy : MonoBehaviour, ITargetLockable
{
	[SerializeField] private Transform lockPoint;
    public Transform GetTargetTransform()=> lockPoint;

    [SerializeField] private float maxDistance;
    Vector3 startingPos;

    [SerializeField] private float speed = 5f;

    void Start()
    {
        startingPos = transform.position;   
       
    }

    private void Update()
    {
        transform.position = new Vector3(
            startingPos.x + SineAmount(),
            startingPos.y ,
            startingPos.z
            );
    }

    private float SineAmount()
    {
        return Mathf.Sin(Time.time * speed) * maxDistance;
    }

}
