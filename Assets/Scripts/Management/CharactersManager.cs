using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public List<EnemyServiceLocator> enemies = new List<EnemyServiceLocator>();

    public void Init()
    {
        enemies.Clear();
        enemies.AddRange(GetComponentsInChildren<EnemyServiceLocator>());

        foreach (EnemyServiceLocator locator in enemies)
        {
            locator.Init();
        }
    }

    public List<EnemyState> SaveEnemies()
    {
        List<EnemyState> states = new List<EnemyState>();   
        foreach(var enemy in enemies)
        {
            EnemyState state = new EnemyState();
            
            Vector3 position = enemy.transform.position;

            state.enemyId = enemy.uid;

            state.enemyPosition[0] = position.x;
            state.enemyPosition[1] = position.y;
            state.enemyPosition[2] = position.z;

            state.statsData = enemy.statsManager.SaveStatsData();
            state.effectData = enemy.statsModifier.SaveEffectData();

            states.Add(state);  
        }

        return states;
    }

    public void LoadCharactersData(LevelState levelState)
    {
        var enemieDatas = levelState.enemyDatas;

        foreach (var enemy in enemieDatas)
        {
            var match = enemies.Find((x) => x.uid == enemy.enemyId);

            if(match != null)
            {
                match.lifecycle.Respawn();

                match.transform.position = new Vector3(enemy.enemyPosition[0], enemy.enemyPosition[1], enemy.enemyPosition[2]);
                match.statsManager.LoadStatsData(enemy.statsData);
                match.statsManager.LoadStatsData(enemy.statsData);

                if(match.statsManager.Health.Current <= 0)
                {
                    match.lifecycle.PerformDeath();  
                }
               
            }
        }
    }


    public void RespawnAllCharacters()
    {
        foreach(var character in enemies)
        {
            character.lifecycle.Respawn();
        }
    }

}
