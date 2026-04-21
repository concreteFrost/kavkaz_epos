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

[Serializable]
public class FriendlyNpcState {
    public string npcId;
    public float[] npcPosition = new float[3];
    public bool wasIntroduced;
    public List<DialogueState> npcQuestsState = new List<DialogueState>();

}

[Serializable]
public class CharactersState
{
    public List<EnemyState> enemyStates = new List<EnemyState>();   
    public List<FriendlyNpcState> friendlyNpcStates = new List<FriendlyNpcState>(); 
}