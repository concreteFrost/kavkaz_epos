public class EnemyStateTrackerServices
{
    public HumanoidAIDamageController damageController;
 
    public HumanoidStats stats;

    public EnemyStateTrackerServices(HumanoidAIDamageController damageController, HumanoidStats stats)
    {
        this.damageController = damageController;
        this.stats = stats;
    }

}