public class EnemyStateTrackerServices
{
    public HumanoidAIDamageController damageController;
    public HumanoidAIPushReceiver pushReceiver;
    public HumanoidStats stats;

    public EnemyStateTrackerServices(HumanoidAIDamageController damageController, HumanoidAIPushReceiver pushReceiver, HumanoidStats stats)
    {
        this.damageController = damageController;
        this.pushReceiver = pushReceiver;
        this.stats = stats;
    }

}