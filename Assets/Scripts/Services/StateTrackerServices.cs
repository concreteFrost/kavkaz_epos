public class EnemyStateTrackerServices
{
    public HumanoidAIDamageController damageController;
 
    public CharacterStatsController statsManager;

    public EnemyStateTrackerServices(HumanoidAIDamageController damageController, CharacterStatsController statsManager)
    {
        this.damageController = damageController;
        this.statsManager = statsManager;
    }

}