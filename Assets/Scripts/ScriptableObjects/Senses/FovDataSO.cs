using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FOV Data", menuName = ScriptablePaths.SENSES_PATH + "/FOV/FOV Data")]
public class FovDataSO : ScriptableObject
{
    public float viewRadius = 20f;
    public float viewAngle = 70f;

    public LayerMask obstacleMask;
    public List<CharacterType> objectsToScan = new List<CharacterType>();   
}
