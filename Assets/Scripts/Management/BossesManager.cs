using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossesManager : MonoBehaviour
{
    public List<BossArenaController> bosses = new List<BossArenaController>();

    public void Init()
    {
        bosses = GetComponentsInChildren<BossArenaController>().ToList();

        foreach (BossArenaController manager in bosses) { manager.Init(); }
    }

    public List<BossArenaState> SaveBossesState()
    {
        var list = new List<BossArenaState>();

        foreach (var arena in bosses)
        {
            var state = new BossArenaState();
            state.arenaId = arena.state.arenaId;
            state.bossKilled = arena.state.bossKilled;
            list.Add(state);
        }

        return list;
    }

    public void LoadBossesState(LevelState levelState)
    {
        var bossesStates = levelState.bossArenaStates;

        foreach (var state in bossesStates)
        {
            var match = bosses.Find((x) => x.state.arenaId == state.arenaId);

            if (match != null)
            {
                Debug.Log("found area to load");
                match.LoadData(state);
            }
        }
    }
}
