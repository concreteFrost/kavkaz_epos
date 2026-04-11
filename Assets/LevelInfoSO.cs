using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Level Info", menuName = ScriptablePaths.LEVEL_PATH + "/Level Info")]
public class LevelInfoSO : WithIdSO
{
    public string levelName;
}
