using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public float attackTime = 1f;
}

[CreateAssetMenu(fileName = "AttackSet", menuName = "Scriptable Systems/Combat/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<Attack> attackList;
    public float maxComboDelay = 1.3f;
    public AttackType attackType;
}
