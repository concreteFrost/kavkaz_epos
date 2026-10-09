using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSource_", menuName =  ScriptablePaths.COMBAT_PATH + "/Attack Source")]
public class AttackSourceSO : ScriptableObject
{
   public List<CharacterType> characterTypes = new List<CharacterType>(); 
}
