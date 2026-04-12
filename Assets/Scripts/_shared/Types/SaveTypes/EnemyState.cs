using System.Collections.Generic;
using System;

[Serializable]
public class EnemyState
{
    public string enemyId;
    public float[] enemyPosition = new float[3];    

    public CharacterStatsData statsData;
    public List<SavedEffectData> effectData;

}