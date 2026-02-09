using UnityEngine;

public class HumanoidStatsManager : MonoBehaviour
{
    public HumanoidStats Stats;
    public CharacterStatsController Controller;

    public void Init()
    {
        Stats.Init();

        HumanoidStatsControllerServices statsControllerServices = new HumanoidStatsControllerServices(Stats);
        Controller.Init(statsControllerServices);
    }
  
    
}
