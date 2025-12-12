using System;
using UnityEngine;

public class LockableDummy : MonoBehaviour, ITargetLockable
{
	[SerializeField] private Transform lockPoint;
    public Transform GetTargetTransform()=> lockPoint;

}
