using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldState
{
    public Dictionary<string,LevelState> levels = new Dictionary<string,LevelState>();  
}
