public class EnemyStateTrackerServices
{
    public HumanoidAIDamageController damageController;
 
    public HumanoidStatsManager statsManager;

    public EnemyStateTrackerServices(HumanoidAIDamageController damageController, HumanoidStatsManager statsManager)
    {
        this.damageController = damageController;
        this.statsManager = statsManager;
    }

}