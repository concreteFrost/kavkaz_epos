using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FovDataSO", menuName = "Scriptable Systems/FOV/FovDataSO")]
public class FovDataSO : ScriptableObject
{
    public float viewRadius = 20f;
    public float viewAngle = 70f;

    public LayerMask obstacleMask;
    public List<CharacterType> objectsToScan = new List<CharacterType>();   
}
